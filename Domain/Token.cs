using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PivotalTracker.FluentAPI.Domain
{
    /// <summary>
    /// Represent the Pivotal Token necessary for the connection to the Pivotal API
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The API Key can be generated in your Profile Page
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Base Url to connect to the Pivotal API
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Token Constructor
        /// </summary>
        /// <param name="apiKey">you API Key to connect to the Pivotal API</param>
        /// <param name="baseUrl">the base Url of the Pivotal API</param>
        public Token(string apiKey, string baseUrl = "http://www.pivotaltracker.com/services/v3/")
        {
            ApiKey = apiKey;
            BaseUrl = baseUrl;
        }
    }
}
