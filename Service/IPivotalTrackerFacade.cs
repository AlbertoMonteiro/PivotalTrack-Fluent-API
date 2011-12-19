using System.Net;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Root Facade that represent a connection to Pivotal
    /// </summary>
    public interface IPivotalTrackerFacade : IFacade<IPivotalTrackerFacade, IPivotalTrackerFacade>
    {
        Token Token { get; }
    }
}