using System;
using System.Collections.Generic;

namespace PivotalTracker.FluentAPI.Domain
{

    /// <summary>
    /// Represent a Pivotal Story
    /// </summary>
    public class Story
    {
        public Story()
        {
            Attachments = new List<Attachment>();
            Tasks = new List<Task>();
            Notes = new List<Note>();
            Labels = new List<string>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public StoryTypeEnum Type { get; set; }
        public Uri Url { get; set; }
        /// <summary>
        /// The estimation must be a valid value from VelocityScheme
        /// </summary>
        /// <remarks>Chores and Bugs can be refused to be estimated (depends of the Project configuration)</remarks>
        public int Estimate { get; set; }
        /// <summary>
        /// State of the Story
        /// </summary>
        /// <remarks>Not all state can be set when you create a new story. You must create your story in 
        /// a StoryStateEnum.Unscheduled, Save it and Then update the state
        /// </remarks>
        public StoryStateEnum CurrentState { get; set; }
        public string Description{ get; set; }
        public string Name { get; set; }
        public string RequestedBy { get; set; }
        public string OwnedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public IList<string> Labels { get; private set; }

        public IList<Attachment> Attachments { get; private set;}
        public IList<Task> Tasks { get; private set; }
        public IList<Note> Notes { get; private set; }


        public DateTime UpdatedDate { get; set; }
    }
}