using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Serialization;
using System.Dynamic;
using System.Linq;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Repository
{
    /// <summary>
    /// This repository manages Pivotal Project
    /// </summary>
    /// <see cref="https://www.pivotaltracker.com/help/api?version=v3#manage_projects"/>
    public class PivotalProjectRepository : PivotalTrackerRepositoryBase
    {
        #region DTOs
        [XmlRoot("project")]
        public class ProjectXmlResponse
        {
            public int id { get; set; }
            public string name { get; set; }
            public int iteration_length { get; set; }
            public DayOfWeek week_start_day { get; set; }
            public string point_scale { get; set; }
            public string account { get; set; }
            public string velocity_scheme { get; set; }
            public int current_velocity { get; set; }
            public int initial_velocity { get; set; }
            public int number_of_done_iterations_to_show { get; set; }
            public string labels { get; set; }
            public bool allow_attachments { get; set; }
            public bool @public { get; set; }
            public bool use_https { get; set; }
            public bool bugs_and_chores_are_estimatable { get; set; }
            public bool commit_mode { get; set; }
            public DateTimeUTC first_iteration_start_time { get; set; }
            public DateTimeUTC last_activity_at { get; set; }

            [XmlArray("memberships")]
            [XmlArrayItem("membership")]
            public Membership[] memberships { get; set; }

            [XmlArray("integrations")]
            [XmlArrayItem("integration")]
            public Integration[] integrations { get; set; }
        }

        [XmlRoot("projects")]
        public class ProjectsXmlResponse
        {
            [XmlElement("project")]
            public Collection<ProjectXmlResponse> projects;
        }

        [XmlRoot("project")]
        public class ProjectXmlRequest
        {
            public string name { get; set; }
            public int iteration_length { get; set; }
            public DateTimeUTC first_iteration_start_time { get; set; }
        }
        #endregion


        public PivotalProjectRepository(Token token) : base(token)
        {   
        }

        public Project GetProject(int id)
        {
            var path = string.Format("/projects/{0}", id);
            var e = this.RequestPivotal<ProjectXmlResponse>(path, null, "GET");
            return PivotalProjectRepository.CreateProject(e);
        }

        protected static Project CreateProject(ProjectXmlResponse e)
        {
            var lProject = new Project();
            lProject.Account = e.account;
            lProject.CurrentVelocity = e.current_velocity;
            lProject.Id = e.id;
            lProject.InitialVelocity = e.initial_velocity;
            lProject.IsAttachmentAllowed = e.allow_attachments;
            lProject.IsBugAndChoresEstimables = e.bugs_and_chores_are_estimatable;
            lProject.IsCommitModeActive = e.commit_mode;
            lProject.IsPublic = e.@public;
            lProject.IterationLength = e.iteration_length;
            if (e.labels != null) e.labels.Split(',').ToList().ForEach(i => lProject.Labels.Add(i.Trim()));
            lProject.StartDate = e.first_iteration_start_time;
            lProject.LastActivityDate = e.last_activity_at;
            lProject.Name = e.name;
            lProject.NumberOfDoneIterationsToShow = e.number_of_done_iterations_to_show;
            lProject.PointScale = e.point_scale;
            lProject.UseHTTPS = e.use_https;
            lProject.VelocityScheme = e.velocity_scheme;
            lProject.WeekStartDay = e.week_start_day;

            foreach (var i in e.integrations)
                lProject.Integrations.Add(i);

            foreach (var m in e.memberships)
            {
                m.ProjectRef.Name = lProject.Name;
                m.ProjectRef.Id = lProject.Id;
                lProject.Memberships.Add(m);
            }

            return lProject;
        }

        public IEnumerable<Project> GetProjects()
        {
            const string path = "/projects";
            var e = this.RequestPivotal<ProjectsXmlResponse>(path, null, "GET");

            return e.projects.Select(p => PivotalProjectRepository.CreateProject(p)).ToList();

        }

        public Project CreateProject(Repository.PivotalProjectRepository.ProjectXmlRequest projectRequest)
       {
            var path = string.Format("/projects");
  

            var e = this.RequestPivotal<ProjectXmlResponse>(path, projectRequest, "POST");
            return PivotalProjectRepository.CreateProject(e);
            
        }

       
    }
}