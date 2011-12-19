using System;
using System.Collections.Generic;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Represent a Pivotal Iteration
    /// </summary>
    public class Iteration
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public IList<Story> Stories { get; set; }
    }
}