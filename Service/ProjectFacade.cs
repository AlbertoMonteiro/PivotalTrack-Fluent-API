using System;
using PivotalTracker.FluentAPI.Domain;
using PivotalTracker.FluentAPI.Repository;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Facade that manage a project
    /// </summary>
    public class ProjectFacade : FacadeItem<ProjectFacade, ProjectsFacade, Project>
    {
        private readonly PivotalProjectRepository _repository;

        public ProjectFacade(ProjectsFacade parent, Project project)
            : base(parent, project)
        {
            _repository = new Repository.PivotalProjectRepository(this.RootFacade.Token);
        }

        /// <summary>
        /// Get a facade that manage membership of this project
        /// </summary>
        /// <returns>a facade for the project Membership</returns>
        public MembershipsFacade Membership()
        {
            return new MembershipsFacade(this);
        }

       

        //TODO: Not tested
        /// <summary>
        /// Get a facade for the project iterations
        /// </summary>
        /// <returns>a facade that manages project iterations</returns>
        /// <remarks>Not yet implemented</remarks>
        public IterationsFacade Iterations()
        {
            throw new NotImplementedException();
            
        }

        /// <summary>
        /// Get a facade to manage stories of this project
        /// </summary>
        /// <returns>a facade that will manage the stories</returns>
        public StoriesProjectFacade Stories()
        {
            var lFacade = new StoriesProjectFacade(this);
            return lFacade;
        }

        /// <summary>
        /// Set story state to Delivered for all stories that has a Finished state
        /// </summary>
        /// <returns>This</returns>
        public ProjectFacade DeliverAllFinishedStories()
        {
            var lStoryRepo = new Repository.PivotalStoryRepository(this.RootFacade.Token);
            lStoryRepo.DeliverAllFinishedStories(this.Item.Id);
            return this;
        }

        public ProjectFacade AcceptAllDeliveredStories()
        {
            this.Stories().Filter("state:delivered").UpdateAll(s =>
            {
                s.CurrentState = StoryStateEnum.Accepted;
            });

            return this;
        }

        
    }
}