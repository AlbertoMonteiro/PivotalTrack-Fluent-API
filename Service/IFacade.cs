using System;

namespace PivotalTracker.FluentAPI.Service
{
    /// <summary>
    /// Facade abstraction of the Fluent API
    /// </summary>
    public interface IFacade
    {
        /// <summary>
        /// Parent Facade
        /// </summary>
        /// <remarks>Cannot be null but for IPivotalTrackerFacade</remarks>
        IFacade ParentFacade { get; }

        /// <summary>
        /// Call it when you finish to used this Facade and continue with the Parent Facade
        /// </summary>
        /// <returns>Parent Facade for Fluent coding</returns>
        /// <seealso cref="IFacade.ParentFacade"/>
        IFacade Done();
        /// <summary>
        /// Do an action
        /// </summary>
        /// <param name="action">action to do</param>
        /// <returns>This for Fluent coding</returns>
        IFacade Do(Action action);
        /// <summary>
        /// Root Facade (Parent of all Facade)
        /// </summary>
        IPivotalTrackerFacade RootFacade { get; }
    }

   
    /// <summary>
    /// Generic Facade Abstraction that inherit IFacade
    /// </summary>
    /// <typeparam name="TCurrent">MUST be the type of the class that implement this Interface</typeparam>
    /// <typeparam name="TParent">Parent facade type if it is known</typeparam>
    public interface IFacade<out TCurrent, out TParent> : IFacade
        where TCurrent : IFacade<TCurrent, TParent>
        where TParent : IFacade
    {
        new TParent ParentFacade { get; }
        new TCurrent Do(Action action);

        /// <summary>
        /// Do an action and pass This facade to it
        /// </summary>
        /// <param name="action">Action to do with this</param>
        /// <returns>This for Fluent coding</returns>
        TCurrent DoWithFacade(Action<TCurrent> action);
        new TParent Done();
    }

    /// <summary>
    /// Generic Facade Abstraction that encapsulate actions on a specific Item of type TItem
    /// </summary>
    /// <typeparam name="TCurrent">MUST be the type of the class that implement this Interface</typeparam>
    /// <typeparam name="TParent">Parent facade type if it is known</typeparam>
    /// <typeparam name="TItem">Type of the Item on which actions will be applied</typeparam>
    public interface IFacade<out TCurrent, out TParent, out TItem> : IFacade<TCurrent, TParent>
        where TCurrent : IFacade<TCurrent, TParent>
        where TParent : IFacade
    {

        /// <summary>
        /// Item on which actions will be applied
        /// </summary>
        TItem Item { get; }

        /// <summary>
        /// Do an action with an Item and its Facade
        /// </summary>
        /// <param name="action">Action that accepts this.Item and this</param>
        /// <returns>This for Fluent coding</returns>
        TCurrent Do(Action<TCurrent, TItem> action);
        /// <summary>
        /// Do an action with this.Item
        /// </summary>
        /// <param name="action">Action that accepts this.Item</param>
        /// <returns>This for Fluent coding</returns>
        TCurrent Do(Action<TItem> action);
    }
}