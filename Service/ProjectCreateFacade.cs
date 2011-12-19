using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Facade that manages the project creation
    /// </summary>
    public class ProjectCreateFacade : FacadeItem<ProjectCreateFacade, ProjectsFacade, Repository.PivotalProjectRepository.ProjectXmlRequest>
    {
        public ProjectCreateFacade(ProjectsFacade parent, Repository.PivotalProjectRepository.ProjectXmlRequest project)
            : base(parent, project)
        {
            project.iteration_length = 3;
        }

        /// <summary>
        /// Set the project name
        /// </summary>
        /// <param name="name">project name</param>
        /// <returns>this</returns>
        public ProjectCreateFacade SetName(string name)
        {
            this.Item.name = name;

            return this;
        }

        /// <summary>
        /// Set the iteration length
        /// </summary>
        /// <param name="length">iteration length</param>
        /// <returns></returns>
        public ProjectCreateFacade SetIterationLength(int length)
        {
            this.Item.iteration_length = length;
            return this;
        }

        public ProjectCreateFacade SetStartDateTime(System.DateTime start)
        {
            this.Item.first_iteration_start_time = start;
            return this;
        }

        /// <summary>
        /// Save the project into Pivotal
        /// </summary>
        /// <returns>a facade that manage the new project</returns>
        public ProjectFacade Save()
        {
            var repo = new Repository.PivotalProjectRepository(this.RootFacade.Token);
            var p = repo.CreateProject(this.Item);

            return new ProjectFacade(this.ParentFacade, p);
        }

        
    }
}