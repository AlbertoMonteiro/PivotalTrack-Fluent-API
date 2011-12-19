using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Represent a Pivtoal Integration
    /// </summary>
    [XmlRoot("integration")]
    public class Integration
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public string FieldLabel { get; set; }
        public bool IsActive { get; set; }
    }
}