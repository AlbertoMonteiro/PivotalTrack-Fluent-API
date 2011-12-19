using System;
using System.IO;
using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Represent an Pivotal Attachment
    /// </summary>
    [XmlRoot("attachment")]
    public class Attachment
    {
    
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("filename")]
        public string FileName { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("uploaded_by")]
        public string UploadedBy { get; set; }
        [XmlElement("uploaded_at")]
        public string UploadedDate { get; set; }
        [XmlElement("url")]
        public string Url {get;set;}

      
    }
}