using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Represent a reference to a Project.
    /// </summary>
    /// <seealso cref="Project"/>
    public class ProjectRef
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
    }
}