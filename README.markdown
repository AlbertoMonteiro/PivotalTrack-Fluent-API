What is it ?
=============

Pivotal Tracker FluentAPI is C# API that uses the Fluent pattern to connect to the PivotalTracker REST API.

How to use it ?
=============

First create the Pivotal Tracker Facade

	var token = new Token("APIKEY"); //get a pivotal API key from your Profile
	var Pivotal = new PivotalTrackerFacade(token);

List all stories

	Pivotal.Projects().Get(123456).Stories().All().Each(s=>Console.WriteLine("{0} : {1}", s.Name, s.Description));
	
List some stories
	
	var stories = Pivotal.Projects().Get(123456).Stories().Filter("label:ui state:delivered");
	
Create a story

	Pivotal.Projects().Get(123456).Stories()
		.Create()
			  .SetName("Hello World")
			  .SetType(StoryTypeEnum.Bug)
		.Save();
		
Complete sample
create a project
create a story
add a note
add an attachment
start a story
then retrieves all stories in started state

	byte[] someBytes = System.Text.Encoding.ASCII.GetBytes("Hello World"); //Some bytes
	Pivotal
		.Projects()
			.Create()
				.SetName("My first project")
				.SetIterationLength(3)
			.Save()
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
				.Filter("state:started") //search for stories
					.Do(stories =>
					{
						//do some display
						foreach (var s in stories)
						{
							Console.WriteLine("{0}: {1} ({2})", s.Id, s.Name, s.Type);
							foreach (var n in s.Notes)
								Console.WriteLine("\tNote {0} ({1}): {2}", n.Id, n.Description, n.NoteDate);
						}
					})
				.Done() //not mandatory, just for the symmetry :)
			.Done()
		.Done()
	.Done();

There is many other methods. Just download the code and let's follow the Fluent API :)

# More informations

API Implementation Status can be consulted to see how far the API is implemented
Developer Section give some coding notes about this project
Mantis Migration Tool : a complete use case using PivotalTracker FluentAPI
Pivotal Tracker Stories : to follow bugs, features, ...

# Follow the project

Pivotal project activities
Global codeplex activities
Source code activities
New version