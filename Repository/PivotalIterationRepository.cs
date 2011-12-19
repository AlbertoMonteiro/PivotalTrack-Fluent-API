using System.Collections.Generic;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Repository
{
    //TODO: Implement IterationRepo
    /// <summary>
    /// Repository that manage Iteration.
    /// </summary>
    /// <remarks>Not yet implemented</remarks>
    /// <see cref="https://www.pivotaltracker.com/help/api?version=v3#get_iterations"/>
    public class PivotalIterationRepository : PivotalTrackerRepositoryBase
    {
        public PivotalIterationRepository(Token token) : base(token)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Iteration> GetIterations(int projectId)
        {
            return null;
        }

        public IEnumerable<Iteration> GetLimitedIterations(int projectId, int offset, int limit)
        {

            return null;
        }

    }
}