using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PivotalTracker.FluentAPI.Domain;
using PivotalTracker.FluentAPI.Repository;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Facade used to Create a story
    /// </summary>
    public class StoryCreationFacade : FacadeItem<StoryCreationFacade, StoriesProjectFacade, PivotalStoryRepository.StoryCreationXmlRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parent facade</param>
        /// <param name="item">The (empty) story instanciated by the parent Facade in the DTO format</param>
        public StoryCreationFacade(StoriesProjectFacade parent, PivotalStoryRepository.StoryCreationXmlRequest item) : base(parent, item)
        {
        }

        /// <summary>
        /// Set the story title
        /// </summary>
        /// <param name="name">title</param>
        /// <returns>This</returns>
        public StoryCreationFacade SetName(string name)
        {
            Item.name = name;
            return this;
        }

        /// <summary>
        /// Set the story type
        /// </summary>
        /// <param name="type">story type</param>
        /// <returns>This</returns>
        public StoryCreationFacade SetType(StoryTypeEnum type)
        {
            Item.story_type = type.ToString().ToLowerInvariant();

            return this;
        }

        /// <summary>
        /// Set the story Description
        /// </summary>
        /// <param name="description">story description</param>
        /// <returns>This</returns>
        public StoryCreationFacade SetDescription(string description)
        {
            Item.description = description;
            return this;
        }

        /// <summary>
        /// Save the story to Pivotal
        /// </summary>
        /// <returns>Parent facade</returns>
        public StoryFacade<StoriesProjectFacade> Save()
        {
            var repo = new Repository.PivotalStoryRepository(this.RootFacade.Token);
            var story = repo.AddStory(this.ParentFacade.ParentFacade.Item.Id, Item);
            return new StoryFacade<StoriesProjectFacade>(this.ParentFacade, story);
        }

        /// <summary>
        /// Set labels of the story.
        /// </summary>
        /// <param name="labels">all labels separated with a comma</param>
        /// <returns>This</returns>
        public StoryCreationFacade SetLabel(string labels)
        {
            Item.labels = labels;

            return this;
        }

        //Not used be cause can generate some exception
        //public StoryCreationFacade SetState(StoryStateEnum state)
        //{
        //    switch(state)
        //    {
        //        case StoryStateEnum.Accepted:
        //            throw new ArgumentOutOfRangeException("Today[2011-03-13], cannot create story with status " + state);
        //        default:
        //            break;
        //    }
        //    Item.current_state = state.ToString().ToLowerInvariant();
        //    return this;
        //}

        /// <summary>
        /// Set the owner of the story
        /// </summary>
        /// <param name="owner">owner name</param>
        /// <returns>This</returns>
        public StoryCreationFacade SetOwner(string owner)
        {
            Item.owned_by = owner;
            
            return this;
        }

        /// <summary>
        /// Set estimation point for this story
        /// </summary>
        /// <param name="i">point</param>
        /// <remarks>Be carefull to estimate what is estimable or you will get an exception</remarks>
        /// <returns>This</returns>
        public StoryCreationFacade SetEstimate(int points)
        {
            Item.estimate = points;
            return this;
        }
    }
}
