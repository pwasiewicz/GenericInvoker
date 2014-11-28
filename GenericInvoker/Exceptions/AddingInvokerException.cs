namespace GenericInvoker.Exceptions
{
    using System;

    public class AddingInvokerException : Exception
    {
        public AddingInvokerException()
            : base("Cannot add a specified invoker.")
        {
        }
    }
}
