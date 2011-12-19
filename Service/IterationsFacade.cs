using System;
using System.Collections.Generic;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Manage a project iterations list
    /// </summary>
    /// <remarks>Not yet implemented</remarks>
    public class IterationsFacade : Facade<IterationsFacade, ProjectFacade>
    {
        public IterationsFacade(ProjectFacade parent)
            : base(parent)
        {
        }

        //public IterationFacade FindIteration(Func<IEnumerable<Iteration>, Iteration> selector)
        //{
        //    return null;
            
        //}
        //TODO: Not tested
        public IterationsFacade GetIterations(int offset=1, int limit=10, Func<IEnumerable<Iteration>, Iteration> selector=null)
        {
            throw new NotImplementedException();
            
        }

    }
}