using System;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Implementation if IFacadeItem<TCurrent, TParent, TItem>
    /// </summary>
    /// <typeparam name="TCurrent">MUST be the type of the class that implement this Interface</typeparam>
    /// <typeparam name="TParent">Parent facade type if it is known</typeparam>
    /// <typeparam name="TItem">Type of the Item on which actions will be applied</typeparam>
    public class FacadeItem<TCurrent, TParent, TItem> : Facade<TCurrent, TParent>, IFacade<TCurrent, TParent, TItem>
        where TCurrent : IFacade<TCurrent, TParent>
        where TParent : IFacade
        where TItem: class
    {
        
        public FacadeItem(TParent parent, TItem item)
            : base(parent)
        {
            if (item != null) Item = item;
        }

        public virtual TItem Item { get; protected set; }

        public virtual TCurrent Do(Action<TCurrent, TItem> action)
        {
            action((TCurrent) (IFacade) this, Item);

            return (TCurrent) (IFacade) this;
        }

        public virtual TCurrent Do(Action<TItem> action)
        {
            action(Item);

            return (TCurrent)(IFacade)this;
        }

       
    }
}