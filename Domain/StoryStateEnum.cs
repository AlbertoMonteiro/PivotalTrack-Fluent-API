using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// All possible state available in Pivotal
    /// </summary>
    public enum StoryStateEnum
    {
        [XmlEnum("unscheduled")]
        Unscheduled,
        [XmlEnum("unstarted")]
        Unstarted,
        [XmlEnum("started")]
        Started,
        [XmlEnum("finished")]
        Finished,
        [XmlEnum("delivered")]
        Delivered,
        [XmlEnum("accepted")]
        Accepted,
        [XmlEnum("rejected")]
        Rejected
    } ;
}