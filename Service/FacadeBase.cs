using System;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Base implementation if IFacade. Inherit it to create a new Facade
    /// </summary>
    public class FacadeBase : IFacade
    {
        public FacadeBase(IFacade parent)
        {
            ParentFacade = parent;
        }
        public virtual IFacade ParentFacade { get; protected set; }

        public virtual IFacade Do(Action action)
        {
            if (action != null) action();

            return this;
        }

        public virtual IPivotalTrackerFacade RootFacade
        {
            get
            {
                IFacade f = this;
                do
                {
                    if (f is IPivotalTrackerFacade)
                        return f as IPivotalTrackerFacade;
                    f = f.ParentFacade;
                } while (f != null);

                return f as IPivotalTrackerFacade;
            }
        }

        public virtual IFacade Done()
        {
            return ParentFacade;
        }
    }
}