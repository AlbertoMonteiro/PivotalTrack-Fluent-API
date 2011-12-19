using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Facade that manages project memberships 
    /// </summary>
    public class MembershipsFacade : Facade<MembershipsFacade, ProjectFacade>
    {
        private Repository.PivotalMembershipsRepository _repository;
        public MembershipsFacade(ProjectFacade parent)
            :base(parent)
        {
            _repository = new Repository.PivotalMembershipsRepository(RootFacade.Token);
        }


        /// <summary>
        /// Apply an action on the loaded membership list
        /// </summary>
        /// <param name="action">action that accept membeship list</param>
        /// <returns>This</returns>
        public MembershipsFacade All(Action<IEnumerable<Membership>> action)
        {
            action(_repository.GetAllMemberships(this.ParentFacade.Item.Id));
            return this;
        }

        /// <summary>
        /// Add a new membership into the project
        /// </summary>
        /// <param name="membership">a initialized membership (email is mandatory)</param>
        /// <returns>This</returns>
        public MembershipsFacade Add(Membership membership)
        {

            membership.ProjectRef.Name = this.ParentFacade.Item.Name;
            membership.ProjectRef.Id = this.ParentFacade.Item.Id;
            _repository.AddMembership(membership);

            return this;
        }
        
        /// <summary>
        /// Add a new membership that is return by a Factory
        /// </summary>
        /// <param name="creator">factory that accept the project object and create the membership</param>
        /// <returns>This</returns>
        public MembershipsFacade Add(Func<Project, Membership> creator)
        {
            var m = creator(this.ParentFacade.Item);
            return Add(m);
        }

        /// <summary>
        /// Remove a membership with a selector
        /// </summary>
        /// <param name="selector">Selector that accept a projet and returns a membership</param>
        /// <returns></returns>
        public MembershipsFacade Remove(Func<Project, Membership> selector)
        {
            _repository.RemoveMembership(selector(this.ParentFacade.Item));
            return this;
        }
    }
}
