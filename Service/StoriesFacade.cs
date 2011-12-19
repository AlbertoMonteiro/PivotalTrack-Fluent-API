using System;
using System.Collections.Generic;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Facade that manage a stories list
    /// </summary>
    public class StoriesFacade : FacadeItem<StoriesFacade, StoriesProjectFacade, IEnumerable<Story>>
    {
        public StoriesFacade(StoriesProjectFacade parent, IEnumerable<Story> item) : base(parent, item)
        {
        }

        //TODO: Not tested
        /// <summary>
        /// Get the first story from the list that match the predicate
        /// </summary>
        /// <param name="predicate">predicate to match</param>
        /// <returns>facade that manage the story</returns>
        public StoryFacade<StoriesFacade> Get(Predicate<Story> predicate)
        {
            foreach (var item in this.Item)
            {
                if(predicate(item))
                    return new StoryFacade<StoriesFacade>(this, item);
            }

            return new StoryFacade<StoriesFacade>(this, null);
        }

        /// <summary>
        /// Do an action on the managed stories 
        /// </summary>
        /// <param name="action">action that accept the current facade</param>
        /// <returns></returns>
        public StoriesFacade Each(Action<StoryFacade<StoriesFacade>> action)
        {
            foreach (var s in Item)
            {
                action(new StoryFacade<StoriesFacade>(this, s));
            }
            return this;
        }

        /// <summary>
        /// Do an action on all managed stories then save the modifications
        /// </summary>
        /// <param name="action">action that accepts a story</param>
        /// <returns>This</returns>
        /// <remarks>saves are done after each action call</remarks>
        public StoriesFacade UpdateAll(Action<Story> action)
        {
            foreach (var s in Item)
            {
                StoryFacade<StoriesFacade> f = new StoryFacade<StoriesFacade>(this, s);
                f.Update(action);
            }
            return this;
        }
    }
}