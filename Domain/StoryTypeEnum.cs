using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// All possible Story Type in Pivotal
    /// </summary>
    public enum StoryTypeEnum
    {
        [XmlEnum("feature")]
        Feature,
        [XmlEnum("bug")]
        Bug,
        [XmlEnum("chore")]
        Chore,
        [XmlEnum("release")]
        Release
    } ;
}