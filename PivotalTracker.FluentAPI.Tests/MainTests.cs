using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PivotalTracker.FluentAPI.Service;
using PivotalTracker.FluentAPI.Domain;
using System.IO;
using System.Net;

namespace PivotalTracker.FluentAPI.Tests
{
    [TestClass]
    public class MainTests
    {

        #region Properties
        static private PivotalTrackerFacade Pivotal { get; set; }
        public static Project Project { get; private set; }
        private static Story Story { get; set; }
        public static TestContext Context { get; set; }

        #endregion


        #region Helpers

        private static Project CreateNewProject(string name)
        {
           
            return Pivotal
                .Projects()
                    .Create()
                        .SetName(name)
                        .SetIterationLength(3)
                    .Save().Item;
        }

        private static Story CreateNewStory(string name, StoryTypeEnum type, string description)
        {
            return Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                            .Create()
                                .SetName(name)
                                .SetType(type)
                                .SetDescription(description)
                            .Save().Item;
        }
        #endregion


        //[ClassInitialize]
        //public static void ClassInitialize(TestContext context)
        //{

        //    Context = context;
        //    Pivotal = new PivotalTrackerFacade(new Token(Properties.Settings.Default.ApiKey));

        //    Project = Properties.Settings.Default.TestProjectId > 0 ? 
        //        Pivotal.Projects().Get(Properties.Settings.Default.TestProjectId).Item : 
        //        CreateNewProject("test" + DateTime.Now.Ticks.ToString());

        //    Story = CreateNewStory("test story", StoryTypeEnum.Feature, "Story test");

        //    //Uncomment to trace request in fiddler2
        //    //System.Net.WebRequest.DefaultWebProxy = new WebProxy("localhost", 8888);

        //}


        
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                            .All()
                                .Each(f=>{
                                    f.Delete();
                                });
                            

            //Do not exists, clean up must be done manually
            //or choose a project id in the settings to avoid creation

            if (Properties.Settings.Default.TestProjectId == 0)
            {
                Assert.Inconclusive("You must delete the project {0} with id={1} because PT do not allow project deletion. Prefer to launch tests in a specific project (cf. Settings)", Project.Name, Project.Id);
            }
            //    Pivotal
            //        .Projects()
            //            .Get(Project.Id)
            //                .Delete();

        }


        #region Stories Tests

        [TestMethod]
        public void GetAllStories()
        {
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                            .All()
                                .Do((f, s) =>
                                {
                                    Assert.IsTrue(true);
                                })
                                .Done()
                            .Done()
                        .Done()
                    .Done()
                .Done();
        }

        [TestMethod]
        public void UpdateStory()
        {
            const string DESCRIPTION = "test updated successfully";

            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                            .Get(Story.Id)
                                .Update(s =>
                                {
                                    s.Description = DESCRIPTION;
                                })
                            .Done()
                            .Get(Story.Id)
                                .Do((f, s) =>
                                {
                                    Assert.AreEqual(s.Description, DESCRIPTION);
                                })
                            .Done()
                        .Done()
                    .Done()
                .Done()
            .Done();

        }

        [TestMethod]
        public void CreateStory()
        {
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                            .Create()
                                .SetName("Im famous")
                                .SetType(StoryTypeEnum.Chore)
                                .SetDescription("test description")
                            .Save()
                            .Do(s =>
                            {
                                Assert.AreEqual(s.Name, "Im famous");
                                Assert.AreEqual(s.Type, StoryTypeEnum.Chore);
                                Assert.AreEqual(s.Description, "test description");
                                Assert.IsTrue(s.Id > 0);
                            })
                        .Done()
                    .Done()
                .Done()
            .Done();
        }

        [TestMethod]
        public void AddNoteToStory()
        {
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                            .Get(Story.Id)
                                .AddNote("YOUPI")
                            .Done()
                            .Get(Story.Id)
                                .Do(s =>
                                {
                                    Assert.AreEqual(1, s.Notes.Count(n => n.Description == "YOUPI"));
                                })
                            .Done()
                        .Done()
                    .Done()
                .Done()
            .Done();
        }

        [TestMethod]
        public void DeleteStory()
        {
            Story s = CreateNewStory("to  be deleted", StoryTypeEnum.Feature, "delete me!");


            try
            {
                Pivotal
                    .Projects()
                        .Get(Project.Id)
                            .Stories()
                                .Get(s.Id)
                                .Delete()
                            .Done()
                            .Stories()
                                .Get(s.Id);
            }
            catch (System.Net.WebException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, ((HttpWebResponse)ex.Response).StatusCode);
            }
        }

        [TestMethod]
        public void FilterStories()
        {
            var i = 0;
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                            .Filter("state:unstarted")
                                .Do(stories =>
                                {
                                    i++;
                                })
                            .Done()
                        .Done()
                    .Done()
                .Done()
            .Done();

            Assert.IsTrue(i > 0);
        }

        [TestMethod]
        public void GetOneStory()
        {
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Stories()
                           .Get(Story.Id)
                           .Do(s => {
                               Assert.AreEqual(Story.Id, s.Id);
                               Assert.AreEqual(Story.Name, s.Name);                                
                           })
                          .Done()
                    .Done()
                .Done();
        }

        #endregion

        #region Project Tests

        [TestMethod]
        public void EntireTest()
        {
            byte[] someBytes = System.Text.Encoding.ASCII.GetBytes("Hello World"); //Some bytes

           Pivotal
                .Projects()
                    .Get(Project.Id) //ProjectId
                        .Stories()
                            .Create()
                                .SetName("This is my first story")
                                .SetType(StoryTypeEnum.Feature)
                                .SetDescription("i'am happy it's so easy !")
                                .Save()
                                    .AddNote("this is really amazing")
                                    .UploadAttachment(someBytes, "attachment.txt", "text/plain")
                                    .Update(story =>
                                    {
                                        story.Estimate = 3;
                                        story.OwnedBy = story.RequestedBy;
                                        story.CurrentState = StoryStateEnum.Started;
                                    })
                        .Done()
                        .Filter("state:started")
                            .Do(stories =>
                            {
                                foreach (var s in stories)
                                {
                                    Console.WriteLine("{0}: {1} ({2})", s.Id, s.Name, s.Type);
                                    foreach (var n in s.Notes)
                                        Console.WriteLine("\tNote {0} ({1}): {2}", n.Id, n.Description, n.NoteDate);
                                }
                            })
                        .Done()
                    .Done()
                .Done()
            .Done();
        }

        [TestMethod]
        public void CreateProject()
        {
            const string projectName = "test project creation";
            int id = 0;
            DateTime startDate = new DateTime(2011, 03, 01);
            Pivotal
               .Projects()
                   .Create()
                       .SetName(projectName)
                       .SetIterationLength(3)
                       .SetStartDateTime(startDate)
                   .Save()
                   .Do(p =>
                   {
                       Assert.AreNotEqual(0, p.Id);
                       id = p.Id;
                       Assert.AreEqual(p.Name, projectName);
                   })
                   .Done()
                   .Get(id) //reload
                       .Do(p =>
                       {
                           Assert.AreNotEqual(0, p.Id);
                           Assert.AreEqual(p.Name, projectName);
                           Assert.AreEqual(startDate, p.StartDate);
                       })
                   .Done()
              .Done()
           .Done();
        }

        [TestMethod]
        public void DeliverAllFinishedStories()
        {
            Story s = CreateNewStory("to  be finished", StoryTypeEnum.Feature, "finish me!");
           
                Pivotal
                    .Projects()
                        .Get(Project.Id)
                            .Stories()
                                .Get(s.Id)
                                .Update(story =>
                                {
                                    story.Estimate = 1;
                                    story.OwnedBy = story.RequestedBy;
                                    story.CurrentState = StoryStateEnum.Finished;
                                })
                                .Done()
                            .Done()
                            .DeliverAllFinishedStories()
                            .Stories()
                                .Get(s.Id)
                                    .Do(story =>
                                    {
                                        Assert.AreEqual(StoryStateEnum.Delivered, story.CurrentState);
                                    })
                                    .Delete();
        }

        #endregion

        #region AttachmentsTest

        private static StoryFacade<StoriesProjectFacade> UploadAttachment(string DATA)
        {
            return Pivotal
                            .Projects()
                                 .Get(Project.Id)
                                     .Stories()
                                         .Get(Story.Id)
                                             .UploadAttachment((s, stream) =>
                                             {
                                                 using (var writer = new StreamWriter(stream, Encoding.ASCII))
                                                 {
                                                     writer.WriteLine(DATA);
                                                 }
                                             })
                                         .Done()
                                         .Get(Story.Id);
        }
        [TestMethod]
        [Ignore]
        public void AddAttachmentThenDownloadThenCheckContent()
        {
            const string DATA = "This is an attachment";

            UploadAttachment(DATA)
                                .Do((f, s) =>
                                {
                                    Assert.AreEqual(1, s.Attachments.Count);
                                    Assert.AreNotEqual(0, s.Attachments[0].Id);

                                    //Download and check content
                                    var data = f.DownloadAttachment(s.Attachments[0]);
                                    string value = System.Text.Encoding.ASCII.GetString(data);

                                    Assert.AreEqual(DATA, value);


                                });
        }

        [TestMethod]
        public void AddAttachmentButDoNotCheckContent()
        {
            const string DATA = "This is an attachment";

            UploadAttachment(DATA)
                                .Do((f, s) =>
                                {
                                    Assert.AreEqual(1, s.Attachments.Count);
                                    Assert.AreNotEqual(0, s.Attachments[0].Id);
                                });
        }
        #endregion

        #region Membership
        [TestMethod]
        public void GetAllMemberships()
        {
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Membership()
                            .All(members =>
                            {
                                Assert.IsNotNull(members);
                                Assert.AreEqual(1, members.Count());
                                Assert.IsNotNull(members.First().Person);
                                Assert.IsNotNull(members.First().Person.Name);
                            });
        }

        [TestMethod]
        public void AddMembership()
        {
            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Membership()
                            .Add(p =>
                            {
                                var m = new Membership();
                                m.MembershipRole = MembershipRoleEnum.Member;
                                m.Person.Name = Properties.Settings.Default.NewMemberName;
                                m.Person.Email = Properties.Settings.Default.NewMemberEmail;

                                return m;
                            });
        }

        [TestMethod]
        public void RemoveMembership()
        {
            AddMembership();

            Pivotal
                .Projects()
                    .Get(Project.Id)
                        .Membership()
                            .Remove(p =>
                            {
                                return p.Memberships.Where(m => m.Person.Email == Properties.Settings.Default.NewMemberEmail).First();
                            })
                        .Done()
                        .Membership()
                            .All(members =>
                            {
                                Assert.AreEqual(0, members.Where(m => m.Person.Email == Properties.Settings.Default.NewMemberEmail).Count());
                            });
        }
        
        #endregion

    }
}
