using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeCore.Application;
using ValidationAttributeCore.Model;
using ValidationAttributeCoreTest.Model.Animals;

namespace ValidationAttributeCoreTest
{




    [TestClass]
    public class StressTest
    {
        ////for 100,1000,10000,100000000000
    }

    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
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
        private readonly List<object> _animals;

        [TestMethod]
        public void ValidationOneDogElement()
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
        public void ValidationOneCatElement()
        {
            //Arrange
            var cat = new Cat("Max", 1, false, "Mammal");

            //Act
            var target = DiscoverValidator.ValidateEntity(cat);

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(ValidData<Cat>));
        }

        [TestMethod]
        public void ValidationMultipleElements()
        {
            //Act
            var results = DiscoverValidator.ValidateMultipleEntities(_animals);
            var resultsOneEntity = DiscoverValidator.ValidateEntity(new List<Dog>()
            {
                new Dog("Max", 6, false, "Mammal"),
                new Dog("", 4, false, "Mammal"),
                new Dog("Bobby", 8, true, "Mammal"),
            });
            var b = results.GetDataOfEntityType<Dog>();
            //var a = results.GetValidDataCastedPre(results.ValidDataList);
            //var target4 = DiscoverValidator.GetValidDate(target);
            var valid = results.GetDataOfEntityType<Dog>();
            var valid2 = results.GetValidDataOfType<Dog>();
            var valid3 = results.GetInvalidDataOfType<Dog>();
            var valid4 = results.GetNotValidatableDataOfType<Dog>();
            //Assert
        }
    }
}
