using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeCore.Application;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCore.Model;
using ValidationAttributeCoreTest.Model.Animals;
using ValidationAttributeCoreTest.Model.Animals.Interface;
using ValidationAttributeCoreTest.Validations;

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
            var target = DiscoverValidator.ValidateElement(dog);
            
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
            var target = DiscoverValidator.ValidateElement(cat);

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(ValidData<Cat>));
        }

        [TestMethod]
        public void ValidationMultipleElements()
        {
            //Act
            var target = DiscoverValidator.ValidateAll(_animals);
            var target2 = DiscoverValidator.GetDataOfType<Bird>(target);

            //Assert
            Assert.IsNotNull(target);
            Assert.AreEqual(1, target.OfType<InvalidData<Bird>>().Count());
            Assert.AreEqual(2, target.OfType<InvalidData<Dog>>().Count());
        }
    }
}
