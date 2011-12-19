using System;
using System.IO;
using System.Net;
using PivotalTracker.FluentAPI.Repository;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Root facade that must be instanciated to Fluent code
    /// </summary>
    public class PivotalTrackerFacade : Facade<IPivotalTrackerFacade, IPivotalTrackerFacade>, IPivotalTrackerFacade
    {
        public Token Token { get; private set; }
        public HttpWebRequest CreateWebRequest(string format, params object[] parameters)
        {
            var url = String.Format(format, parameters);
            url = Path.Combine(Token.BaseUrl, url);
            return WebRequest.Create(url) as HttpWebRequest;
        }

        public PivotalTrackerFacade(Token token) : base(null)
        {
            Token = token;
        }

        /// <summary>
        /// Get a facade on all projects (not loaded)
        /// </summary>
        /// <returns>a facade that manage projects of the account</returns>
        public ProjectsFacade Projects()
        {
            return new ProjectsFacade(this);
        }



      
    }
}