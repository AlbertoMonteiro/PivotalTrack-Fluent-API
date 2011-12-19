using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Repository
{
    /// <summary>
    /// Base class that all repositories can inherit.
    /// </summary>
    /// 
    public class PivotalTrackerRepositoryBase : IPivotalTrackerRepository
    {
        private const int REQUEST_PAUSE = 2000; //milliseconds
        /// <summary>
        /// PivotalTrackerRepositoryBase Constructor
        /// </summary>
        /// <param name="token">Token for the Pivotal API Connection</param>
        public PivotalTrackerRepositoryBase(Token token)
        {
            if (token == null || String.IsNullOrWhiteSpace(token.ApiKey) || String.IsNullOrWhiteSpace(token.BaseUrl))
                throw new ArgumentNullException("token");


            this.Token = token;
            
        }

        public Token Token { get; protected set; }

        /// <summary>
        /// This method send a request to Pivotal and deserialize the XML Result into T
        /// </summary>
        /// <remarks>Sometimes Pivotal cannot answer to the request (ex: to much load). So this method do 3 retries with a pause of 2 seconds between</remarks>
        /// <typeparam name="T">The Pivotal XML Response will be deserialized into T. Must be conformed to the Pivotal API</typeparam>
        /// <param name="path">Relative URL (path) of the Pivotal API (ex:/projects/PROJECT_ID/stories/STORY_ID)</param>
        /// <param name="data">object that will be serialized to the Request stream (usefull for PUT request)</param>
        /// <param name="method">method used to send the requestPivotal</param>
        /// <returns>the deserialized Pivotal XML Response</returns>
        protected T RequestPivotal<T>(string path, dynamic data, string method = "POST")
            where T : class
        {
            //Sometimes Pivotal Fails, so let's retry several times
            int nTries = 2;

            while(nTries > 0)
            {
                Uri lUri = GetPivotalURI(path);
                WebRequest lRequest;

                lRequest = WebRequest.Create(lUri);
                lRequest.Headers.Add("X-TrackerToken", this.Token.ApiKey);
                lRequest.Headers.Add("Accepts", "application/json");
                lRequest.ContentType = "application/json";
                lRequest.Method = method;
                string debug = "";
                if (data != null)
                    using (var stream = new MemoryStream())
                    {
                        var writerSettings = new XmlWriterSettings { CloseOutput = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true };
                        using (var xmlWriter = XmlWriter.Create(stream, writerSettings))
                        {
                            var xmlNamespace = new XmlSerializerNamespaces();
                            xmlNamespace.Add("", "");
                            var lRequestSerializer = new XmlSerializer(data.GetType());
                            lRequestSerializer.Serialize(xmlWriter, data, xmlNamespace);
                            xmlWriter.Flush();
                            debug = Encoding.UTF8.GetString(stream.GetBuffer());
                            stream.WriteTo(lRequest.GetRequestStream());
                        }
                    }
                else
                    if (method == "PUT")
                        lRequest.ContentLength = 0; //Force to 0 if no data and PUT Method to avoid Pivotal Exception
                
                try
                {
                    return Deserialize<T>(lRequest.GetResponse().GetResponseStream());
                }
                catch (WebException e)
                {
                    using (var r = new StreamReader(e.Response.GetResponseStream()))
                    {
                        Console.WriteLine(r.ReadToEnd());
                    }
                    nTries--;
                    if (nTries == 0)
                        throw;
                    System.Threading.Thread.Sleep(REQUEST_PAUSE);
                }
            }

            throw new ArgumentOutOfRangeException("REQUEST_PAUSE", "must be greater than 0"); //Cannot be reached 
        }

        /// <summary>
        /// Transform a path into an absolute Pivotal API URL
        /// </summary>
        /// <param name="path">Relative path to the REST API</param>
        /// <returns>the absolute URL</returns>
        private Uri GetPivotalURI(string path)
        {
            if (!String.IsNullOrWhiteSpace(path) && path[0] == '/') //Manage leading slash
                path = path.Remove(0, 1);

            return new Uri(new Uri(Token.BaseUrl), path);
        }


        /// <summary>
        /// Deserialize the content stream into T
        /// </summary>
        /// <typeparam name="T">Deserialized Type</typeparam>
        /// <param name="stream">Stream that contains the serialized object</param>
        /// <returns>deserialized object of type T</returns>
        protected T Deserialize<T>(Stream stream)
        {
            var lSerializer = new XmlSerializer(typeof(T));
            try
            {
                return (T)lSerializer.Deserialize(stream);
            }
            catch (WebException e)
            {
#if DEBUG
                using (var r = new StreamReader(e.Response.GetResponseStream()))
                {
                    Console.WriteLine(r.ReadToEnd());
                }
#endif
                throw;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#endif
                throw;
            }
        }

        


        //TODO: Bug in Pivotal : do not work (http://gsfn.us/t/26f14)
        /// <summary>
        /// Download a Pivotal Attachment
        /// </summary>
        /// <remarks>Bug in Pivotal : do not work (http://gsfn.us/t/26f14)</remarks>
        /// <param name="url">absolute URL to the attachment</param>
        /// <returns>data downloaded</returns>
        protected byte[] RequestPivotalDownload(string url)
        {
            var lReq = WebRequest.Create(url);
            lReq.Headers.Add("X-TrackerToken", Token.ApiKey);

            using (var stream = new MemoryStream())
            {
                lReq.GetResponse().GetResponseStream().CopyTo(stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Helper method to transform an url into a acceptable url (not special cars, etc...)
        /// </summary>
        /// <param name="url">Url to sanitize</param>
        /// <returns>the sanitized Url</returns>
        static string UrlSanitize(string url)
        {
            url = System.Text.RegularExpressions.Regex.Replace(url, @"\s+", "-");
            string stFormD = url.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString());
        }

        /// <summary>
        /// Upload a file to Pivotal and rtrive the result object
        /// </summary>
        /// <typeparam name="T">Type of the serialized object in the Pivotal XML Response</typeparam>
        /// <param name="path">Relative path to the REST API (ex: /projects/PROJECT_ID/stories/STORY_ID/attachments)</param>
        /// <param name="data">data to upload</param>
        /// <param name="filename">filename of the uploaded data (will be the name that appear in Pivotal Story)</param>
        /// <param name="contentType">data content-type</param>
        /// <returns>Deserialized object from the Pivotal XML Response</returns>
        protected T RequestPivotalUpload<T>(string path, byte[] data, string filename="upload", string contentType="application/octet-stream")
        {
            Uri uri = GetPivotalURI(path);

            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            string ufilename = UrlSanitize(filename);
            postParameters.Add("Filedata", new FormUpload.FileParameter(data, ufilename, contentType));            

            NameValueCollection headers = new NameValueCollection();
            headers.Add("X-TrackerToken", Token.ApiKey);

            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(uri.ToString(), "PivotalTracker.FluentAPI UserAgent", postParameters, headers);
            return Deserialize<T>(webResponse.GetResponseStream());


            #region Do not work with WebClient, i keep the code
            
            //Uri uri = new Uri("http://www.postbin.org/1jto9yk");

            //try
            //{
            //    using (var lClient = new WebClient())
            //    {
            //        lClient.Headers.Add("X-TrackerToken", this.Token.ApiKey);
            //        using (var stream = new MemoryStream(lClient.UploadFile(uri, "POST", @"c:\temp\test.png")))
            //        {
            //            return Deserialize<T>(stream);
            //        }
            //    }
            //}
            //catch (WebException e)
            //{
            //    using (var r = new StreamReader(e.Response.GetResponseStream()))
            //    {
            //        Console.WriteLine(r.ReadToEnd());
            //    }
            //    throw;
            //}
            #endregion
        }

      
    }
}
