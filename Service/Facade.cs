using System;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Implementation of IFacade<TCurrent, TParent>
    /// </summary>
    /// <typeparam name="TCurrent"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public class Facade<TCurrent, TParent> : FacadeBase, IFacade<TCurrent, TParent>
        where TCurrent : IFacade<TCurrent, TParent>
        where TParent : IFacade
    {
        public Facade(IFacade parent)
            : base(parent)
        {
        }

        public virtual new TParent ParentFacade { get { return (TParent)base.ParentFacade; } }

        public virtual TCurrent DoWithFacade(Action<TCurrent> action)
        {
            action((TCurrent)(IFacade)this);
            return (TCurrent)(IFacade)this;
        }

        public virtual new TParent Done()
        {
            return (TParent)base.Done();
        }

        public virtual new TCurrent Do(Action action)
        {
            return (TCurrent)((IFacade)base.Do(action)) ;
        }

    }
}