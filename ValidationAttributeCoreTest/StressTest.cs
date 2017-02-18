using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeCore.Application;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Interface;
using ValidationAttributeCoreTest.Model.Animals;
using ValidationAttributeCoreTest.Model.Animals.Interface;
using ValidationAttributeCoreTest.Validations;

namespace ValidationAttributeCoreTest
{
    [TestClass]
    public class StressTest
    {
        //for 100,1000,10000,100000000000
        private readonly List<Dog> _dogs;
        private readonly List<IAnimal> _animals;
        private readonly Random _random;

        public StressTest()
        {
            _dogs = new List<Dog>();
            _animals = new List<IAnimal>();
            _random = new Random();
        }

        [TestMethod]
        public void StressTest10000OnlyDogs()
        {
            //Arrange
            for (int i = 0; i < 10000; i++)
            {
                _dogs.Add(new Dog("Test", 1, RandomBool, "Mammal"));
            }
            
            //Act DiscoverValidation
            var time1 = DateTime.Now;
            var target = DiscoverValidator.ValidateEntity(_dogs);
            var time2 = DateTime.Now;

            var timeWithDiscoverValidation = time2 - time1;
            var validData = target.OfType<ValidData<Dog>>().Count();
            var invalidData = target.OfType<InvalidData<Dog>>().Count();

            //Act FluentValidation
            var time1Fv = DateTime.Now;
            var validator = new DogValidation();
            var results = new List<IData<object>>();
            foreach (var dog in _dogs)
            {
                var targetFv = validator.Validate(dog);
                if (targetFv.IsValid)
                {
                    results.Add(new ValidData<object>(dog));
                }
                else
                {
                    results.Add(new InvalidData<object>(dog, targetFv.Errors));
                }
            }
            var time2Fv = DateTime.Now;

            var timeWithFluentValidation = time2Fv - time1Fv;

            var difference = timeWithDiscoverValidation - timeWithFluentValidation;

            var intTimtDiscover = timeWithDiscoverValidation.Seconds*1000 + timeWithDiscoverValidation.Milliseconds;
            var intTimtFluent = timeWithFluentValidation.Seconds * 1000 + timeWithFluentValidation.Milliseconds;

            var totalTime = intTimtFluent + intTimtDiscover;
            double percFluent = (double)intTimtFluent / (double)totalTime;
            double percDiscover = (double)intTimtDiscover / (double)totalTime;

            double mult = (double)intTimtDiscover / (double)intTimtFluent;
            //Assert
            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void StressTest10000Animals()
        {
            //Arrange
            for (int i = 0; i < 3333; i++)
            {
                _animals.Add(new Dog("Test", 1, RandomBool, "Mammal"));
                _animals.Add(new Cat("Test", 1, RandomBool, "Mammal"));
                _animals.Add(new Bird("Test", 1, RandomBool, "Bird"));
            }

            //Act DiscoverValidation
            var time1 = DateTime.Now;
            var target = DiscoverValidator.ValidateMultipleEntities(_animals);
            var time2 = DateTime.Now;

            var timeWithDiscoverValidation = time2 - time1;
            var validData = target.GetValidDataOfType<Dog>().Count();
            var invalidData = target.GetInvalidDataOfType<Dog>().Count();

            //Act FluentValidation
            var time1Fv = DateTime.Now;

            var results = new List<IData<object>>();
            foreach (var animal in _animals)
            {
                if (animal.GetType() == typeof(Dog))
                {
                    var dogvalidator = new DogValidation();
                    var targetFv = dogvalidator.Validate(animal as Dog);
                    if (targetFv.IsValid)
                    {
                        results.Add(new ValidData<object>(animal));
                    }
                    else
                    {
                        results.Add(new InvalidData<object>(animal, targetFv.Errors));
                    }
                }
                if (animal.GetType() == typeof(Cat))
                {
                    var catvalidator = new CatValidation();
                    var targetFv = catvalidator.Validate(animal as Cat);
                    if (targetFv.IsValid)
                    {
                        results.Add(new ValidData<object>(animal));
                    }
                    else
                    {
                        results.Add(new InvalidData<object>(animal, targetFv.Errors));
                    }
                }
                if (animal.GetType() == typeof(Bird))
                {
                    var birdvalidator = new BirdValidation();
                    var targetFv = birdvalidator.Validate(animal as Bird);
                    if (targetFv.IsValid)
                    {
                        results.Add(new ValidData<object>(animal));
                    }
                    else
                    {
                        results.Add(new InvalidData<object>(animal, targetFv.Errors));
                    }
                }

            }
            var time2Fv = DateTime.Now;

            var timeWithFluentValidation = time2Fv - time1Fv;

            var difference = timeWithDiscoverValidation - timeWithFluentValidation;

            var intTimtDiscover = timeWithDiscoverValidation.Seconds * 1000 + timeWithDiscoverValidation.Milliseconds;
            var intTimtFluent = timeWithFluentValidation.Seconds * 1000 + timeWithFluentValidation.Milliseconds;

            var totalTime = intTimtFluent + intTimtDiscover;
            double percFluent = (double)intTimtFluent / (double)totalTime;
            double percDiscover = (double)intTimtDiscover / (double)totalTime;

            double mult = (double) intTimtDiscover/(double) intTimtFluent;

            //Assert
            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void StressTestAnimalsDisvantageWithFluentValidation()
        {
            var noStress = StressTestAnimals(5);
            var lowStress = StressTestAnimals(50);
            var mediumStress = StressTestAnimals(500);
            var highStress = StressTestAnimals(5000);

            Assert.IsNotNull(highStress);
        }

        public double StressTestAnimals(int number)
        {
            for (int i = 0; i < number; i++)
            {
                _animals.Add(new Dog("Test", 1, RandomBool, "Mammal"));
                _animals.Add(new Cat("Test", 1, RandomBool, "Mammal"));
                _animals.Add(new Bird("Test", 1, RandomBool, "Bird"));
            }

            //Act DiscoverValidation
            var time1 = DateTime.Now;
            var target = DiscoverValidator.ValidateMultipleEntities(_animals);
            var time2 = DateTime.Now;

            var timeWithDiscoverValidation = time2 - time1;
            var validData = target.GetValidDataOfType<Dog>().Count();
            var invalidData = target.GetInvalidDataOfType<Dog>().Count();

            //Act FluentValidation
            var time1Fv = DateTime.Now;

            var results = new List<IData<object>>();
            foreach (var animal in _animals)
            {
                if (animal.GetType() == typeof(Dog))
                {
                    var dogvalidator = new DogValidation();
                    var targetFv = dogvalidator.Validate(animal as Dog);
                    if (targetFv.IsValid)
                    {
                        results.Add(new ValidData<object>(animal));
                    }
                    else
                    {
                        results.Add(new InvalidData<object>(animal, targetFv.Errors));
                    }
                }
                if (animal.GetType() == typeof(Cat))
                {
                    var catvalidator = new CatValidation();
                    var targetFv = catvalidator.Validate(animal as Cat);
                    if (targetFv.IsValid)
                    {
                        results.Add(new ValidData<object>(animal));
                    }
                    else
                    {
                        results.Add(new InvalidData<object>(animal, targetFv.Errors));
                    }
                }
                if (animal.GetType() == typeof(Bird))
                {
                    var birdvalidator = new BirdValidation();
                    var targetFv = birdvalidator.Validate(animal as Bird);
                    if (targetFv.IsValid)
                    {
                        results.Add(new ValidData<object>(animal));
                    }
                    else
                    {
                        results.Add(new InvalidData<object>(animal, targetFv.Errors));
                    }
                }

            }
            var time2Fv = DateTime.Now;

            var timeWithFluentValidation = time2Fv - time1Fv;

            var difference = timeWithDiscoverValidation - timeWithFluentValidation;

            var intTimtDiscover = timeWithDiscoverValidation.Seconds * 1000 + timeWithDiscoverValidation.Milliseconds;
            var intTimtFluent = timeWithFluentValidation.Seconds * 1000 + timeWithFluentValidation.Milliseconds;

            var totalTime = intTimtFluent + intTimtDiscover;
            double percFluent = (double)intTimtFluent / (double)totalTime;
            double percDiscover = (double)intTimtDiscover / (double)totalTime;

            double mult = (double)intTimtDiscover / (double)intTimtFluent;
            return mult;
        }

        private int GetRandomNumber()
        {
            return _random.Next();
        }

        private bool RandomBool
        {
            get
            {
                var number = GetRandomNumber();
                return number % 2 != 0;
            }
        }
    }
}