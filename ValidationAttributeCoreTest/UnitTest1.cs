using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttributeCore.Application;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.Model;
using ValidationAttributeCore.Model.Interface;
using ValidationAttributeCoreTest.Model.Animals;

namespace ValidationAttributeCoreTest
{
    [TestClass]
    public class UnitTest1
    {
        private IList<object> _animals;
        [TestMethod]
        public void TestMethod1()
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
        public void ValidationMethod2()
        {
            //var a =  new BirdValidation();
            //var b = new DogValidation();
            //Act
            MyValidator.Validate(_animals);



        }

        [TestMethod]
        public void Validation()
        {
            //Act
            var target = new List<IData<>>();

            foreach (var animal in _animals)
            {
                var elementType = typeof(NotValidatableData<>);

                var constructedObject = elementType.MakeGenericType(animal.GetType());
                dynamic validator = Activator.CreateInstance(constructedObject);
                validator.Type = animal.GetType();
                var casted = Convert.ChangeType(animal, animal.GetType());
                validator.Entity2 = casted;

                var validationAttribute = animal.GetType()
                    .GetCustomAttribute<ValidateEntityAttribute>();

                if (validationAttribute == null)
                {
                    target.Add(new NotValidatableData()
                    {
                        Type = animal.GetType(),
                        Entity = animal
                    });
                    continue;
                }

                //var validationResult = validationAttribute.Entity.ValidateEntity(animal);

                if (validationResult.IsValid)
                {
                    target.Add(new ValidData()
                    {
                        Type = animal.GetType(),
                        Entity = animal
                    });
                }
                else
                {
                    target.Add(new InvalidData()
                    {
                        Type = animal.GetType(),
                        Entity = animal,
                        ValidationFailures = validationResult.Errors
                    });
                }
            }

            //Assert
            Assert.AreEqual(4, target.OfType<InvalidData>().Count());
            Assert.AreEqual(3, target.OfType<ValidData>().Count());
            Assert.AreEqual(1, target.OfType<NotValidatableData>().Count());
        }
    }
}
