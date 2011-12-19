using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Represent a Pivotal MemberShip
    /// </summary>
    /// <remarks>
    /// ProjectRef contains basic information on the Projet. You can retrieve detailed information with ProjectRef.Id
    /// </remarks>
    /// <seealso cref="Project"/>
    /// <seealso cref="MembershipRoleEnum"/>
    [XmlRoot("membership")]
    public class Membership
    {
        public Membership()
        {
            ProjectRef = new ProjectRef();
            Person = new Person();
        }
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("person")]
        public Person Person { get; set; }
        [XmlElement("role")]
        public MembershipRoleEnum MembershipRole { get; set; }
        [XmlElement("project")]
        public ProjectRef ProjectRef { get;  set; }
    }
}