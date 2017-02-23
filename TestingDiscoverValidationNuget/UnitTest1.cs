using System;
using DiscoverValidationCore.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeTest.Model.Animals;

namespace TestingDiscoverValidationNuget
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var results = DiscoverValidator.ValidateEntity(new Dog("Max", 6, false, "Mammal"));
        }
    }
}
