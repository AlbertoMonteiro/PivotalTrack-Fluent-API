using System;
using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Represent a Pivotal Note on the Story identified by Note.StoryId
    /// </summary>
    public class Note
    {
        public int Id { get; set; }
        public int StoryId { get; set; }

        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime? NoteDate { get; set; }
        
    }
}