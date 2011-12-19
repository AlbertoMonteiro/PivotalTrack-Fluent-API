using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Repository
{

    /// <summary>
    /// Repository that manage Pivotal Membership
    /// </summary>
    /// <see cref="https://www.pivotaltracker.com/help/api?version=v3#get_memberships"/>
    public class PivotalMembershipsRepository : PivotalTrackerRepositoryBase
    {
        public PivotalMembershipsRepository(Token token)
            : base(token)
        {   
        }

        #region DTOs
        /// <summary>
        /// DTO to receive Pivotal memberships list
        /// </summary>
        [XmlRoot("memberships")]
        public class MembershipsXmlResponse
        {
            [XmlElement("membership")]
            public Membership[] memberships;
        }
        #endregion

        public IEnumerable<Membership> GetAllMemberships(int projectId)
        {
            var path = string.Format("/projects/{0}/memberships", projectId);
            var memberships = this.RequestPivotal<MembershipsXmlResponse>(path, null, "GET");


            return memberships.memberships;

        }

        public Membership GetMembership(int projectId, int membershipId)
        {
            var path = string.Format("/projects/{0}/memberships/{1}", projectId, membershipId);
            var membership = this.RequestPivotal<Membership>(path, null, "GET");


            return membership;
        }

        public Membership AddMembership(Membership membership)
        {
            var path = string.Format("/projects/{0}/memberships", membership.ProjectRef.Id);
            var result = this.RequestPivotal<Membership>(path, membership, "POST");


            return result;

        }

        public Membership RemoveMembership(int projectId, int membershipId)
        {
            var path = string.Format("/projects/{0}/memberships/{1}", projectId, membershipId, "DELETE");
            var membership = this.RequestPivotal<Membership>(path, null, "GET");


            return membership;

        }
        public Membership RemoveMembership(Membership membership)
        {
            return RemoveMembership(membership.ProjectRef.Id, membership.Id);
        }
    }
}
