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
            Assert.AreEqual(1, resultsOneEntity.OfType<ValidData<Dog>>());
            Assert.AreEqual(1, resultsOneEntity.OfType<InvalidData<Dog>>());
        }

        [TestMethod]
        public void ValidationMultipleElements()
        {
            //ToDo On progres..
            //Act
            var results = DiscoverValidator.ValidateMultipleEntities(_animals);
            var b = results.GetDataOfEntityType<Dog>();
            var valid = results.GetDataOfEntityType<Dog>();
            var valid2 = results.GetValidDataOfType<Dog>();
            var valid3 = results.GetInvalidDataOfType<Dog>();
            var valid4 = results.GetNotValidatableDataOfType<Dog>();
            //Assert
            Assert.IsNotNull(results);

        }
    }
}
