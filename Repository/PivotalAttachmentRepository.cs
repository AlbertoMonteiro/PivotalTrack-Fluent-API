using System;
using System.Net;
using System.Xml.Serialization;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Repository
{
    /// <summary>
    /// Implement the repository that manages Attachment
    /// </summary>
    /// <see cref="https://www.pivotaltracker.com/help/api?version=v3#attachments"/>
    public class PivotalAttachmentRepository : PivotalTrackerRepositoryBase
    {
        public PivotalAttachmentRepository(Token token) : base(token)
        {

        }

        #region DTOs
        /// <summary>
        /// DTO to receive Pivotal XML Response
        /// </summary>
        [XmlRoot("attachment")]
        public class AttachmentXmlResponse
        {
            [XmlElement("id")]
            public int id { get; set; }
            [XmlElement("status")]
            public string status { get; set; }
        }
        #endregion

        public int UploadAttachment(int projectId, int storyId, byte[] data, string filename="upload", string contentType="application/octet-stream")
        {
            var path = string.Format("/projects/{0}/stories/{1}/attachments", projectId, storyId);
            var e = this.RequestPivotalUpload<AttachmentXmlResponse>(path, data, filename, contentType);

            return e.id;
        }

        public byte[] DownloadAttachment(Attachment a)
        {
            if (a == null || String.IsNullOrWhiteSpace(a.Url))
                return null;

            //using (var lClient = new System.Net.WebClient())
            //{
            //    lClient.Headers.Add("X-TrackerToken", Token.ApiKey);
            //    return lClient.DownloadData(a.Url);
            //}

            return this.RequestPivotalDownload(a.Url);
        }

       
    }
}