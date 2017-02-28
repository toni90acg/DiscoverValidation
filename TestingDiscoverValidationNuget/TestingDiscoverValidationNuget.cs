using DiscoverValidation.Application;
using DiscoverValidationTest.Model.Animals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
