namespace GenericInvoker.Invokers
{
    using System;

    public sealed class ConcreteTypeInvoker<TCaseType, TTarget> : InvokerBase<TTarget>
    {
        private readonly Action<TCaseType> caseAction;

        private readonly Type caseType;

        private readonly Func<TCaseType, bool> additionalPredicate;

        public ConcreteTypeInvoker(TTarget target, Type caseType, Action<TCaseType> caseAction) : base(target)
        {
            if (caseAction == null)
            {
                throw new ArgumentNullException("caseAction");
            }

            this.caseAction = caseAction;
            this.caseType = caseType;
        }

        public ConcreteTypeInvoker(TTarget target, Type type, Func<TCaseType, bool> additionalPredicate,
                                   Action<TCaseType> action)
            : this(target, type, action)
        {
            this.additionalPredicate = additionalPredicate;
        }

        public override Type CaseType
        {
            get { return this.caseType; }
        }

        public override bool Invoke()
        {
            if (this.Target.GetType() != this.caseType)
            {
                return false;
            }

            var casteTypeObject = (TCaseType) (object) this.Target;

            if (this.additionalPredicate != null)
            {
                if (!this.additionalPredicate(casteTypeObject))
                {
                    return false;
                }
            }

            this.caseAction(casteTypeObject);

            return true;
        }
    }
}
