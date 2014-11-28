namespace GenericInvoker.Tests
{
    using System;
    using GenericInvoker.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CaseInvokerTests
    {
        [TestMethod, ExpectedException(typeof(NoCaseHandlerException))]
        public void MissingCase_DefaultBehaoviur_ThrowsNoCaseHandlerException()
        {
            const decimal targetDecimal = 1m;
            targetDecimal.DetermineType()
                         .When<int>(val => Assert.Fail())
                         .When<string>(value => Assert.Fail())
                         .Resolve();
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void MissingCase_CustomException_ThrowsInvalidOperationException()
        {
            const decimal targetDecimal = 1m;
            targetDecimal.DetermineType()
                         .WhenMissing<InvalidOperationException>()
                         .When<int>(val => Assert.Fail())
                         .When<string>(value => Assert.Fail())
                         .Resolve();
        }

        [TestMethod]
        public void MissingCase_CustomAction_InvokesAction()
        {
            const decimal targetDecimal = 1m;
            var invokedAction = false;

            targetDecimal.DetermineType()
                         .WhenMissing(target => { invokedAction = true; })
                         .When(typeof(int), value => Assert.Fail())
                         .When((string value) => Assert.Fail())
                         .Resolve();

            Assert.IsTrue(invokedAction);
        }

        [TestMethod]
        public void ExistingCase_GenericCases_InvokersPropers()
        {
            const string target = "a";

            target.DetermineType().When((int val) => Assert.Fail()).When<string>(val => { }).Resolve();
        }


        [TestMethod]
        public void ExistingCase_NonGenericCases_InvokersPropers()
        {
            const string target = "a";

            target.DetermineType().When<int>(val => Assert.Fail()).When(typeof (string), val =>
                                                                                         {
                                                                                             if (val.GetType() !=
                                                                                                 typeof (string))
                                                                                             {
                                                                                                 Assert.Fail();
                                                                                             }
                                                                                         }).Resolve();
        }

        [TestMethod]
        [ExpectedException(typeof (NoCaseHandlerException))]
        public void NoCases_DefualtBehaoviur_ThrowsNoCaseHandlerException()
        {
            const string target = "a";

            target.DetermineType().Resolve();
        }

        [TestMethod]
        public void SameCase_DifferentConditions_ExecutesProper()
        {
            const string target = "a";

            target.DetermineType()
                  .WhenMissing(t => Assert.Fail())
                  .When<string>(t => t == "b", t => Assert.Fail())
                  .When<string>(t => t == "a", t => { });
        }
    }
}
