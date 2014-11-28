namespace GenericInvoker
{
    using System;
    using System.Collections.Generic;

    public interface IInvokersResolver<TTarget>
    {
        /// <summary>
        /// Sets the invokers readonly collection.
        /// </summary>
        IEnumerable<InvokerBase<TTarget>> Invokers { set; }

        /// <summary>
        /// Produces new case invoker for specified target and action.
        /// </summary>
        /// <typeparam name="TCaseType">The type of the case.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="action">The action to perfrom when case type is reached.</param>
        /// <returns>Invoker base instance.</returns>
        InvokerBase<TTarget> NewCaseInvoker<TCaseType>(TTarget target, Action<TCaseType> action);

        /// <summary>
        /// News the case invoker.
        /// </summary>
        /// <typeparam name="TCaseType">The type of the case type.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="additionalPredicate">The additional predicate.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        InvokerBase<TTarget> NewCaseInvoker<TCaseType>(TTarget target, Func<TCaseType, bool> additionalPredicate,
                                                       Action<TCaseType> action);

        /// <summary>
        /// Produces new case invoker for specified target and action.
        /// </summary>
        /// <param name="type">The type of the case.</param>
        /// <param name="target">The target object.</param>
        /// <param name="action">The action to perfrom when case type is reached.</param>
        /// <returns>
        /// Invoker base instance.
        /// </returns>
        InvokerBase<TTarget> NewCaseInvoker(Type type, TTarget target, Action<TTarget> action);

        /// <summary>
        /// Determines whether this instance can add the specified invoker.
        /// </summary>
        /// <param name="invoker">The invoker.</param>
        /// <returns>True, if can add invoker instance, false otherwise.</returns>
        bool CanAdd(InvokerBase<TTarget> invoker);

        /// <summary>
        /// Resolves the case with invokers.
        /// </summary>
        /// <returns>True if succeed, false otherwise.</returns>
        bool Resolve();
    }
}
