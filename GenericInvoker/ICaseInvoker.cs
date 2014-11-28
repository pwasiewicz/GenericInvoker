namespace GenericInvoker
{
    using System;

    public interface ICaseInvoker<TTargetType>
    {
        /// <summary>
        /// Defines the action that should be invoked when target is of specified type.
        /// </summary>
        /// <typeparam name="T">The desired type of target.</typeparam>
        /// <param name="then">The action for perform.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        ICaseInvoker<TTargetType> When<T>(Action<T> then);

        /// <summary>
        /// Whens the specified additional case.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="additionalPredicate">The additional case.</param>
        /// <param name="then">The then.</param>
        /// <returns></returns>
        ICaseInvoker<TTargetType> When<T>(Func<T, bool> additionalPredicate, Action<T> then);

        /// <summary>
        ///  Defines the action that should be invoked when target is of specified type.
        /// </summary>
        /// <param name="type">The desried type of target.</param>
        /// <param name="then">The action for perform.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        ICaseInvoker<TTargetType> When(Type type, Action<TTargetType> then);

        /// <summary>
        /// Adds custom resolver to handle case.
        /// </summary>
        /// <param name="invoker">The invoker instance.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        ICaseInvoker<TTargetType> WithResolver(InvokerBase<TTargetType> invoker);

        /// <summary>
        /// Resolvers the specifed target and launches the matched invoker.
        /// </summary>
        void Resolve();
    }

    public interface ICaseMissing<TTargetType> : ICaseInvoker<TTargetType>
    {
        /// <summary>
        /// Definies an action that should be invoked, when there is not case where type matches.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        ICaseInvoker<TTargetType> WhenMissing(Action<TTargetType> action);

        /// <summary>
        /// Definies an exception type that should be thrown, when there is not case where type matches.
        /// Exception must have parameterless constructor.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to throw.</typeparam>
        /// <returns>Case invoker instance for easily chaining.</returns>
        ICaseInvoker<TTargetType> WhenMissing<TException>() where TException : Exception, new();
    }
}
