using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.Application;
using DiscoverValidation.Model.Data;
using DiscoverValidationTest.Model.Animals;
using DiscoverValidationTest.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscoverValidationTest
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
            var isValid = target.IsValid();
            var validationFailures = target.GetValidationFailures();

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(ValidData<Dog>));
            Assert.IsTrue(isValid.Value);
            Assert.IsNull(validationFailures);
        }

        [TestMethod]
        [TestCategory("Discover Validation - One Element")]
        public void ValidationOneNotValidatedBirdElement()
        {
            //Arrange
            var bird = new Bird("Tweety", 1, false, "Mammal");

            //Act
            var target = DiscoverValidator.ValidateEntity(bird);
            var isValid = target.IsValid();
            var validationFailures = target.GetValidationFailures();

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(NotValidatedData<Bird>));
            Assert.IsNull(isValid);
            Assert.IsTrue(validationFailures.Any());
        }

        [TestMethod]
        [TestCategory("Discover Validation - One Element")]
        public void ValidationOneNotValidatableBigFoodElement()
        {
            //Arrange
            var bird = new BigFoot();

            //Act
            var target = DiscoverValidator.ValidateEntity(bird);
            var isValid = target.IsValid();
            var validationFailures = target.GetValidationFailures();

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(NotValidatableData<BigFoot>));
            Assert.IsNull(isValid);
            Assert.IsTrue(validationFailures.Any());
        }

        [TestMethod]
        [TestCategory("Discover Validation - One Element")]
        public void ValidationOneInvalidCatElement()
        {
            //Arrange
            var cat = new Cat("Max", 1, true, "Mammal");

            //Act
            var target = DiscoverValidator.ValidateEntity(cat);
            var isValid = target.IsValid();
            var validationFailures = target.GetValidationFailures();

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(InvalidData<Cat>));
            Assert.IsFalse(isValid.Value);
            Assert.IsTrue(validationFailures.Any());
        }

        [TestMethod]
        [TestCategory("Discover Validation - One Element")]
        public void ValidationOneValidBirdWithValidatorElement()
        {
            //Arrange
            var bird = new Bird("Tweety", 1, true, "Bird");

            //Act
            var target = DiscoverValidator.ValidateEntity(bird, typeof(BirdValidation2));
            var isValid = target.IsValid();
            var validationFailures = target.GetValidationFailures();

            //Assert
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(ValidData<Bird>));
            Assert.IsTrue(isValid.Value);
            Assert.IsNull(validationFailures);
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

            var allValidData = resultsOneEntity.OfType<ValidData<Dog>>();
            var allInvalidData = resultsOneEntity.OfType<InvalidData<Dog>>();


            var allNotValidatableData = resultsOneEntity.OfType<NotValidatableData<Dog>>();

            allNotValidatableData.ToList().ForEach(data =>
            {
                var entity = data.Entity;
                //Do whatever you want
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
            var results = DiscoverValidator.ValidateMultipleEntities(_animals); // Animals is a List<IAnimal>

           
            var allresultsOfDogs = results.GetDataOfType<Dog>();
            allresultsOfDogs.ToList().ForEach(data =>
            {
                var entity = data.Entity;
                //Do whatever you want
            });
            var allInvalidDataOfCats = results.GetInvalidDataOfType<Cat>();
            var allNotValidatableDataOfBigFoot = results.GetNotValidatableDataOfType<BigFoot>();
            
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
