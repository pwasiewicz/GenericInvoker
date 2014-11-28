namespace GenericInvoker.Implementations
{
    using GenericInvoker.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class CaseInvoker<TTarget> : ICaseMissing<TTarget>
    {
        private readonly TTarget target;

        private Action<TTarget> missingCaseAction;

        private Func<Exception> missingCaseExceptionFactory;

        private readonly IList<InvokerBase<TTarget>> invokers;

        private readonly IInvokersResolver<TTarget> resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseInvoker{TTarget}"/> class.
        /// </summary>
        /// <param name="target">The target type to handle cases of.</param>
        private CaseInvoker(TTarget target)
        {
            this.target = target;
            this.invokers = new List<InvokerBase<TTarget>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseInvoker{TTarget}"/> class.
        /// </summary>
        /// <param name="target">The target type to handle cases of.</param>
        /// <param name="resolver">The resolver that handles the collection of invokers.</param>
        public CaseInvoker(TTarget target, IInvokersResolver<TTarget> resolver)
            : this(target)
        {
            this.resolver = resolver;
            this.resolver.Invokers = this.CastInvokersToReadonlyCollection();
        }

        /// <summary>
        /// Defines the action that should be invoked when target is of specified type.
        /// </summary>
        /// <typeparam name="T">The desired type of target.</typeparam>
        /// <param name="then">The action for perform.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        /// <exception cref="System.ArgumentNullException">then</exception>
        /// <exception cref="GenericInvoker.Exceptions.AddingInvokerException">
        /// Thrown when resolver do not allows to add invoker.
        /// </exception>
        public ICaseInvoker<TTarget> When<T>(Action<T> then)
        {
            if (then == null)
            {
                throw new ArgumentNullException("then");
            }

            var typeResolver = this.resolver.NewCaseInvoker(this.target, then);

            if (!this.resolver.CanAdd(typeResolver))
            {
                throw new AddingInvokerException();
            }

            this.invokers.Add(typeResolver);
            this.resolver.Invokers = this.CastInvokersToReadonlyCollection();

            return this;
        }

        /// <summary>
        /// Whens the specified additional predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="additionalPredicate">The additional predicate.</param>
        /// <param name="then">The then.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// additionalPredicate
        /// or
        /// then
        /// </exception>
        /// <exception cref="AddingInvokerException"></exception>
        public ICaseInvoker<TTarget> When<T>(Func<T, bool> additionalPredicate, Action<T> then)
        {
            if (additionalPredicate == null)
            {
                throw new ArgumentNullException("additionalPredicate");
            }

            if (then == null)
            {
                throw new ArgumentNullException("then");
            }

            var typeResolver = this.resolver.NewCaseInvoker(this.target, additionalPredicate, then);

            if (!this.resolver.CanAdd(typeResolver))
            {
                throw new AddingInvokerException();
            }

            this.invokers.Add(typeResolver);
            this.resolver.Invokers = this.CastInvokersToReadonlyCollection();

            return this;
        }

        /// <summary>
        ///  Defines the action that should be invoked when target is of specified type.
        /// </summary>
        /// <param name="type">The desried type of target.</param>
        /// <param name="then">The action for perform.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// then
        /// or
        /// type
        /// </exception>
        /// <exception cref="GenericInvoker.Exceptions.AddingInvokerException">
        /// Thrown when resolver do not allows to add invoker.
        /// </exception>
        public ICaseInvoker<TTarget> When(Type type, Action<TTarget> then)
        {
            if (then == null)
            {
                throw new ArgumentNullException("then");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var typeResolver = this.resolver.NewCaseInvoker(type, this.target, then);

            if (!this.resolver.CanAdd(typeResolver))
            {
                throw new AddingInvokerException();
            }

            this.invokers.Add(typeResolver);
            this.resolver.Invokers = this.CastInvokersToReadonlyCollection();

            return this;
        }

        /// <summary>
        /// Adds custom resolver to handle case.
        /// </summary>
        /// <param name="invoker">The invoker instance.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        /// <exception cref="System.ArgumentNullException">invoker</exception>
        /// <exception cref="GenericInvoker.Exceptions.AddingInvokerException">
        /// Thrown when resolver do not allows to add invoker.
        /// </exception>
        public ICaseInvoker<TTarget> WithResolver(InvokerBase<TTarget> invoker)
        {
            if (invoker == null)
            {
                throw new ArgumentNullException("invoker");
            }

            if (!this.resolver.CanAdd(invoker))
            {
                throw new AddingInvokerException();
            }

            this.invokers.Add(invoker);
            this.resolver.Invokers = this.CastInvokersToReadonlyCollection();

            return this;
        }

        /// <summary>
        /// Definies an action that should be invoked, when there is not case where type matches.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>Case invoker instance for easily chaining.</returns>
        public ICaseInvoker<TTarget> WhenMissing(Action<TTarget> action)
        {
            this.missingCaseAction = action;
            this.missingCaseExceptionFactory = null;

            return this;
        }

        /// <summary>
        /// Definies an exception type that should be thrown, when there is not case where type matches.
        /// Exception must have parameterless constructor.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to throw.</typeparam>
        /// <returns>Case invoker instance for easily chaining.</returns>
        public ICaseInvoker<TTarget> WhenMissing<TException>() where TException : Exception, new()
        {
            this.missingCaseExceptionFactory = () => new TException();
            this.missingCaseAction = null;

            return this;
        }

        /// <summary>
        /// Resolvers the specifed target and launches the matched invoker.
        /// </summary>
        public void Resolve()
        {
            if (this.resolver.Resolve())
            {
                return;
            }

            this.NoInvokers();
        }

        private void NoInvokers()
        {
            if (this.missingCaseAction != null)
            {
                this.missingCaseAction(this.target);
                return;
            }

            if (this.missingCaseExceptionFactory == null)
            {
                throw new NoCaseHandlerException(typeof (TTarget));
            }

            throw this.missingCaseExceptionFactory();
        }

        private IEnumerable<InvokerBase<TTarget>> CastInvokersToReadonlyCollection()
        {
            return new ReadOnlyCollection<InvokerBase<TTarget>>(this.invokers);
        }
    }
}
