using System;
using System.IO;
using PivotalTracker.FluentAPI.Domain;
using PivotalTracker.FluentAPI.Repository;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Facade to manage a story
    /// </summary>
    /// <typeparam name="TParent">Parent facade Type. This Facade can have multiple Parent, so it stays generic</typeparam>
    public class StoryFacade<TParent> : FacadeItem<StoryFacade<TParent>, TParent, Story>
        where TParent:IFacade
    {
        private readonly PivotalStoryRepository _storyRepository;
        private readonly PivotalAttachmentRepository _attachRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent Facade</param>
        /// <param name="item">Story to manage</param>
        public StoryFacade(TParent parent, Story item)
            : base(parent, item)
        {
            _storyRepository = new Repository.PivotalStoryRepository(this.RootFacade.Token);
            _attachRepository = new PivotalAttachmentRepository(this.RootFacade.Token);
        }

        /// <summary>
        /// Call the action updater then send the modification to Pivotal
        /// </summary>
        /// <param name="updater">Action that accepts a the story to modified (this.Item)</param>
        /// <returns>This</returns>
        public StoryFacade<TParent> Update(Action<Story> updater)
        {
            updater(this.Item);
           
            this.Item = _storyRepository.UpdateStory(this.Item);

            return this;
        }
 
        /// <summary>
        /// Add a note to the managed story
        /// </summary>
        /// <param name="text">text of the note</param>
        /// <returns>This</returns>
        public StoryFacade<TParent> AddNote(string text)
        {
            var note = _storyRepository.AddNote(this.Item.ProjectId, this.Item.Id, text);
            this.Item.Notes.Add(note);
            return this;
        }
        
        /// <summary>
        /// Delete the managed StoryFacade
        /// </summary>
        /// <returns>Parent Facade</returns>
        public TParent Delete()
        {
            _storyRepository.DeleteStory(this.Item.ProjectId, this.Item.Id);
            return Done();
        }

        /// <summary>
        /// Upload an attachment to the managed story
        /// </summary>
        /// <param name="attachment">data to upload</param>
        /// <param name="fileName">attachment filename in Pivotal</param>
        /// <param name="contentType">data content-type </param>
        /// <returns>This</returns>
        public StoryFacade<TParent> UploadAttachment(byte[] attachment, string fileName="upload", string contentType="application/octet-stream")
        {
            using (var stream = new MemoryStream(attachment))
            {
                return UploadAttachment((s, output) => stream.WriteTo(output), fileName, contentType);
            }
        }

        //TODO: Deviner le mime-type
        /// <summary>
        /// Upload an attachment to the managed story. The developper write into the stream
        /// </summary>
        /// <param name="action">Action that accepts the story and the upload stream</param>
        /// <param name="fileName">attachment filename in Pivotal</param>
        /// <param name="contentType">data content-type </param>
        /// <returns>This</returns>
        public StoryFacade<TParent> UploadAttachment(Action<Story, Stream> action, string fileName = "upload", string contentType = "application/octet-stream")
        {
            using (var stream = new MemoryStream())
            {   
                action(this.Item, stream);
                _attachRepository.UploadAttachment(this.Item.ProjectId, this.Item.Id, stream.ToArray(), fileName, contentType);

                return this;
            }
        }

        /// <summary>
        /// Download a specific attachment
        /// </summary>
        /// <param name="a">attachment to download</param>
        /// <returns>attachment data</returns>
        public byte[] DownloadAttachment(Attachment a)
        {
            return _attachRepository.DownloadAttachment(a);
        }

        public StoryFacade<TParent> Move(int targetStoryId, bool before = true)
        {
            _storyRepository.MoveStory(this.Item.ProjectId, this.Item.Id, before ? PivotalStoryRepository.MovePositionEnum.Before : PivotalStoryRepository.MovePositionEnum.After, targetStoryId);

            return this;
        }

    }


}