using System;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Repository
{
    /// <summary>
    /// Interface that must be implement by all Repositories
    /// </summary>
    public interface IPivotalTrackerRepository
    {
        Token Token { get; }
    }
}