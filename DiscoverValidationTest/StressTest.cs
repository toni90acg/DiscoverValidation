using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DiscoverValidation.Application;
using DiscoverValidationTest.Model.Animals;
using DiscoverValidationTest.Model.Animals.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscoverValidationTest
{
    [TestClass]
    public class StressTest
    {
        readonly Random _random = new Random();

        [TestMethod]
        public void ValidateOneUniqueEntityType()
        {
            var numberOfEntities = 20000;
            var animals = GenerateDogs(numberOfEntities);
            Debug.WriteLine($"Validating {numberOfEntities} entities");

            var timeRegular11 = DateTime.Now;
            DiscoverValidator.ValidateEntity(animals);
            var timeRegular21 = DateTime.Now;
            var timeRegular1 = timeRegular21 - timeRegular11;
            Debug.WriteLine("timeRegular: " + timeRegular1);

            var timeAssync11 = DateTime.Now;
            DiscoverValidator.ValidateEntityAsync(animals);
            var timeAssync21 = DateTime.Now;
            var timeAssync1 = timeAssync21 - timeAssync11;
            Debug.WriteLine("timeAssync: " + timeAssync1);

            var timeAssync12 = DateTime.Now;
            DiscoverValidator.ValidateEntityAsync(animals);
            var timeAssync22 = DateTime.Now;
            var timeAssync2 = timeAssync22 - timeAssync12;
            Debug.WriteLine("timeAssync: " + timeAssync2);

            var timeRegular12 = DateTime.Now;
            DiscoverValidator.ValidateEntity(animals);
            var timeRegular22 = DateTime.Now;
            var timeRegular2 = timeRegular22 - timeRegular12;
            Debug.WriteLine("timeRegular: " + timeRegular2);

            var difference = (timeRegular2 + timeRegular1) - (timeAssync1 + timeAssync2);

            Debug.WriteLine($"timeAssync is {difference} faster than timeRegular");

            Assert.IsTrue((timeRegular2 + timeRegular1) > (timeAssync1 + timeAssync2));
        }

        [TestMethod]
        public void ValidateEntities()
        {
            var numberOfEntities = 20000;
            IList<IAnimal> animals = GenerateAnimals(numberOfEntities);
            Debug.WriteLine($"Validating {numberOfEntities} entities");

            var timeRegular11 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntities(animals);
            var timeRegular21 = DateTime.Now;
            var timeRegular1 = timeRegular21 - timeRegular11;
            Debug.WriteLine("timeRegular: " + timeRegular1);

            var timeAssync11 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntitiesAssync(animals);
            var timeAssync21 = DateTime.Now;
            var timeAssync1 = timeAssync21 - timeAssync11;
            Debug.WriteLine("timeAssync: " + timeAssync1);

            var timeAssync12 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntitiesAssync(animals);
            var timeAssync22 = DateTime.Now;
            var timeAssync2 = timeAssync22 - timeAssync12;
            Debug.WriteLine("timeAssync: " + timeAssync2);

            var timeRegular12 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntities(animals);
            var timeRegular22 = DateTime.Now;
            var timeRegular2 = timeRegular22 - timeRegular12;
            Debug.WriteLine("timeRegular: " + timeRegular2);

            var difference = (timeRegular2 + timeRegular1) - (timeAssync1 + timeAssync2);

            Debug.WriteLine($"timeAssync is {difference} faster than timeRegular");

            Assert.IsTrue((timeRegular2 + timeRegular1) > (timeAssync1 + timeAssync2));
        }

        private List<Dog> GenerateDogs(int numberOfAnimals)
        {
            var dogs = new List<Dog>();

            for (int i = 0; i < numberOfAnimals; i++)
            {
                dogs.Add(new Dog(GetRandomName(),GetRandomAge(),GetRandomBool(),GetRandomName()));
            }

            return dogs;
        }

        private List<Cat> GenerateCats(int numberOfAnimals)
        {
            var cats = new List<Cat>();

            for (int i = 0; i < numberOfAnimals; i++)
            {
                cats.Add(new Cat(GetRandomName(), GetRandomAge(), GetRandomBool(), GetRandomName()));
            }

            return cats;
        }

        private List<Bird> GenerateBirds(int numberOfAnimals)
        {
            var birds = new List<Bird>();

            for (int i = 0; i < numberOfAnimals; i++)
            {
                birds.Add(new Bird(GetRandomName(), GetRandomAge(), GetRandomBool(), GetRandomName()));
            }

            return birds;
        }

        private List<BigFoot> GenerateBigfoots(int numberOfAnimals)
        {
            var bigfoots = new List<BigFoot>();

            for (int i = 0; i < numberOfAnimals; i++)
            {
                bigfoots.Add(new BigFoot());
            }

            return bigfoots;
        }


        private List<IAnimal> GenerateAnimals(int numberOfAnimals)
        {
            var animals = new List<IAnimal>();
            for (int i = 0; i < numberOfAnimals; i++)
            {
                var a = _random.Next(1, 10);
                if (a == 1 || a == 2 || a == 3 || a == 4)
                {
                    animals.Add(GenerateDogs(1).First());
                }
                if (a == 5 || a == 6 || a == 7)
                {
                    animals.Add(GenerateCats(1).First());
                }
                if (a == 8 || a == 9)
                {
                    animals.Add(GenerateBirds(1).First());
                }
                if (a == 10)
                {
                    animals.Add(GenerateBigfoots(1).First());
                }
            }

            return animals;
        }

        private string GetRandomName()
        {
            var size = _random.Next(0, 6);
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private int GetRandomAge()
        {
            return _random.Next(-2, 20);
        }

        private bool GetRandomBool()
        {
            var p = _random.Next(1, 10);
            return p%2 == 0;
        }
    }
}
