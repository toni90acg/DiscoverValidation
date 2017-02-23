using System;
using DiscoverValidationCore.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeTest.Model.Animals;

namespace TestingDiscoverValidationNuget
{
    [TestClass]
    public class TestingDiscoverValidationNuget
    {
        [TestMethod]
        public void DiscoverValidationNugetTest()
        {
            var results = DiscoverValidator.ValidateEntity(new Dog("Max", 6, false, "Mammal"));
            Assert.IsNotNull(results);
        }
    }
}
