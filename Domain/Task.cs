using System;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// A Pivotal Task.
    /// </summary>
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}