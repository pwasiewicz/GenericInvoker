using System;

namespace GenericInvoker
{
    public abstract class InvokerBase<TTarget>
    {
        protected readonly TTarget Target;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokerBase{TTarget}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        protected InvokerBase(TTarget target)
        {
            this.Target = target;
        }

        /// <summary>
        /// Gets the type of the case.
        /// </summary>
        public abstract Type CaseType { get; }


        /// <summary>
        /// Invokes the behaviour of case for concrete type.
        /// </summary>
        /// <returns>True, if invoking suceedd, false otherwise.</returns>
        public abstract bool Invoke();
    }
}
