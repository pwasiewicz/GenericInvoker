namespace GenericInvoker.Exceptions
{
    using System;

    public class NoCaseHandlerException : Exception
    {
        public Type TargetType { get; private set; }

        internal NoCaseHandlerException(Type targetType)
            : this(targetType, string.Format("No case found for {0}", targetType))
        {
        }

        internal NoCaseHandlerException(Type targetType, string msg)
            : base(msg)

        {
            this.TargetType = targetType;
        }
    }
}
