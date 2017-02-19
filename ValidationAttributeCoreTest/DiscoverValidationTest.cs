using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeCore.Application;
using ValidationAttributeCore.Model;
using ValidationAttributeCoreTest.Model.Animals;

namespace ValidationAttributeCoreTest
{
    [TestClass]
    public class DiscoverValidationTest
    {
        private readonly List<object> _animals;

        public DiscoverValidationTest()
        {
            _animals = new List<object>()
            {
                new Dog("Max", 6, false, "Mammal"),
                new Dog("", 4, false, "Mammal"),
                new Dog("Bobby", 8, true, "Mammal"),

                new Cat("Meow", 1, false, "Mammal"),
                new Cat("Calcetines", -1, false, "Mammal"),

                new Bird("Jimmy", 1, true, "Bird"),
                new Bird("Twetty", 2, false, ""),

                new BigFoot()
            };
        }

        [TestMethod]
        [TestCategory("Discover Validation - One Element")]
        public void ValidationOneValidDogElement()
        {
            //Arrange
            var dog = new Dog("Max", 1, false, "Mammal");

            //Act
            var target = DiscoverValidator.ValidateEntity(dog);
            
            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(ValidData<Dog>));
        }

        [TestMethod]
        [TestCategory("Discover Validation - One Element")]
        public void ValidationOneInvalidCatElement()
        {
            //Arrange
            var cat = new Cat("Max", 1, true, "Mammal");

            //Act
            var target = DiscoverValidator.ValidateEntity(cat);

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(InvalidData<Cat>));
            Assert.IsNotNull((target as InvalidData<Cat>)?.ValidationFailures);
        }

        [TestMethod]
        [TestCategory("Discover Validation - Multiple Elements")]
        public void ValidationMultipleElementsOfOneUniqueType()
        {
            //Act
            var resultsOneEntity = DiscoverValidator.ValidateEntity(new List<Dog>()
            {
                new Dog("Max", 6, false, "Mammal"),
                new Dog("", 4, false, "Mammal"),
                new Dog("Bobby", 8, true, "Mammal"),
            });
            //Assert
            Assert.IsNotNull(resultsOneEntity);
            Assert.AreEqual(1, resultsOneEntity.OfType<ValidData<Dog>>().Count());
            Assert.AreEqual(2, resultsOneEntity.OfType<InvalidData<Dog>>().Count());
        }

        [TestMethod]
        [TestCategory("Discover Validation - Multiple Elements")]
        public void ValidationMultipleElements()
        {
            //Act
            var results = DiscoverValidator.ValidateMultipleEntities(_animals);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.GetDataOfType<Dog>().Count);
            Assert.AreEqual(1, results.GetValidDataOfType<Dog>().Count);
            Assert.AreEqual(2, results.GetInvalidDataOfType<Dog>().Count);
            var a = results.GetInvalidDataOfType<Dog>();
            var b = a.First();
            var c = (InvalidData<Dog>) b;
            var d = c.ValidationFailures;
            Assert.IsNotNull(
                ((InvalidData<Dog>) results
                    .GetInvalidDataOfType<Dog>().First()).ValidationFailures);
        }
    }
}
