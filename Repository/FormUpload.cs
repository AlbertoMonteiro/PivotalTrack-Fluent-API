using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace PivotalTracker.FluentAPI.Repository
{
    /// <summary>
    /// This class is used to Upload File to Pivotal.
    /// </summary>
    /// <remarks>
    /// It has been found on Internet. But i can't really find who is the initial author. If he recognize himself i would be happy to write his name.
    /// The code has been refactored to accept custom headers.
    /// Moreover we can't use WebClient because Pivotal accepts only "Filedata" as the form name (http://community.pivotaltracker.com/pivotal/topics/cannot_upload_attachment_with_v3_api_from_c)
    /// </remarks>
    public static class FormUpload
    {
        private static readonly Encoding _encoding = Encoding.UTF8;
        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, NameValueCollection headers=null)
        {
            string formDataBoundary = "-----------------------------28947758029299";
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData, headers);
        }


        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, NameValueCollection headers)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            
            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }
            
            // Set up the request properties
            request.Method = "POST";
            if (headers != null)
                request.Headers.Add(headers);
            
            request.AllowAutoRedirect = false;
            request.Accept = "*/*";
            request.KeepAlive = true;
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.UnsafeAuthenticatedConnectionSharing = true;
            request.UseDefaultCredentials = false;
            request.ContentLength = formData.Length;  // We need to count how many bytes we're sending. 
            
            

            using (Stream requestStream = request.GetRequestStream())
            {
                // Push it out there
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            using (Stream formDataStream = new MemoryStream())
            {
                foreach (var param in postParameters)
                    if (param.Value is FileParameter)
                    {
                        FileParameter fileToUpload = (FileParameter)param.Value;
                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n", boundary, param.Key, fileToUpload.FileName ?? param.Key, fileToUpload.ContentType ?? "application/octet-stream");
                        formDataStream.Write(_encoding.GetBytes(header), 0, header.Length);
                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                        // Thanks to feedback from commenters, add a CRLF to allow multiple files to be uploaded
                        //formDataStream.Write(encoding.GetBytes("\r\n"), 0, 2);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", boundary, param.Key, param.Value);
                        formDataStream.Write(_encoding.GetBytes(postData), 0, postData.Length);
                    }
                // Add the end of the request
                string footer = String.Format("\r\n--{0}--\r\n", boundary);
                formDataStream.Write(_encoding.GetBytes(footer), 0, footer.Length);
                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();
                return formData;
            }
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
    }

}
