using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationAttribute.Model;
using ValidationAttribute.Model.Data;
using ValidationAttributeTest.Model.Animals;


namespace ValidationAttributeTest
{
    [TestClass]
    public class AnimalValidationAttribute
    {
        private readonly IList<IAnimal> _animals;
        public AnimalValidationAttribute()
        {
            _animals = new List<IAnimal>()
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
        public void Validation()
        {
            //Act
            var target = new List<IData>();

            foreach (var animal in _animals)
            {
                var validationAttribute = animal.GetType()
                    .GetCustomAttribute<ValidationAttribute.CustomAttribute.ValidationAttribute>();

                if (validationAttribute == null)
                {
                    target.Add(new NotValidatableData()
                    {
                        Type = animal.GetType(),
                        Entity = animal
                    });
                    continue;
                }

                var validationResult = validationAttribute.Validator.ValidateEntity(animal);

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
