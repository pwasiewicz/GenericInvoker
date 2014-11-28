namespace GenericInvoker.Resolvers
{
    using GenericInvoker.Invokers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class DefaultInvokersResolver<TTarget> : IInvokersResolver<TTarget>
    {
        public IEnumerable<InvokerBase<TTarget>> Invokers { set; private get; }

        public InvokerBase<TTarget> NewCaseInvoker<TCaseType>(TTarget target, Action<TCaseType> action)
        {
            return new ConcreteTypeInvoker<TCaseType, TTarget>(target, typeof(TCaseType), action);
        }

        public InvokerBase<TTarget> NewCaseInvoker<TCaseType>(TTarget target, Func<TCaseType, bool> additionalPredicate,
                                                              Action<TCaseType> action)
        {
            return new ConcreteTypeInvoker<TCaseType, TTarget>(target, typeof (TCaseType), additionalPredicate, action);
        }

        public InvokerBase<TTarget> NewCaseInvoker(Type type, TTarget target, Action<TTarget> action)
        {
            return new ConcreteTypeInvoker<TTarget, TTarget>(target, type, action);
        }

        public bool CanAdd(InvokerBase<TTarget> invoker)
        {
            return true;
            //return this.Invokers.All(inv => inv.CaseType != invoker.CaseType);
        }

        public bool Resolve()
        {
            var invokersList = this.Invokers as List<InvokerBase<TTarget>> ?? this.Invokers.ToList();

            return invokersList.Any() && invokersList.Any(invokerBase => invokerBase.Invoke());
        }
    }
}
