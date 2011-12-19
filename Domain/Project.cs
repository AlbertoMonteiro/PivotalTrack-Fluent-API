using System;
using System.Collections.Generic;
using PivotalTracker.FluentAPI.Repository;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Rerpresent a Pivotal Project
    /// </summary>
    public class Project
    {
        public Project()
        {
            Memberships = new List<Membership>();
            Integrations = new List<Integration>();
            Labels = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int IterationLength { get; set; }
        public DayOfWeek WeekStartDay { get; set; }
        public string PointScale { get; set; }
        public string Account { get; set; }
        public string VelocityScheme { get; set; }
        public int CurrentVelocity { get; set; }
        public int InitialVelocity { get; set; }
        public int NumberOfDoneIterationsToShow { get; set; }
        public IList<string> Labels { get; set; }
        public bool IsAttachmentAllowed { get; set; }
        public bool IsPublic { get; set; }
        public bool UseHTTPS { get; set; }
        public bool IsBugAndChoresEstimables { get; set; }
        public bool IsCommitModeActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public IList<Integration> Integrations { get; private set; }
        public IList<Membership> Memberships { get; private set; }
    }
}