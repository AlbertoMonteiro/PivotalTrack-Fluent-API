using System.Xml.Serialization;
namespace PivotalTracker.FluentAPI.Domain
{

    /// <summary>
    /// Represent a Person in Pivotal
    /// </summary>
    /// <seealso cref="Membership"/>
    public class Person
    {
        [XmlElement("email")]
        public string Email { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("initials")]
        public string Initials { get; set; }
    }
}