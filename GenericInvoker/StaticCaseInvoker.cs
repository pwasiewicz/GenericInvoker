namespace GenericInvoker
{
    using GenericInvoker.Implementations;
    using GenericInvoker.Resolvers;

    public static class StaticCaseInvoker
    {
        public static ICaseMissing<TTarget> DetermineType<TTarget>(this TTarget target)
        {
            return new CaseInvoker<TTarget>(target, new DefaultInvokersResolver<TTarget>());
        }
    }
}
