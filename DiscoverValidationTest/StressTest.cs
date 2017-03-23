using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DiscoverValidation.Application;
using DiscoverValidationTest.Model.Animals;
using DiscoverValidationTest.Model.Animals.Interface;
using DiscoverValidationTest.Validations;
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
            var numberOfEntities = 50000;
            var animals = GenerateDogs(numberOfEntities);
            Debug.WriteLine($"Validating {numberOfEntities} entities");
            DiscoverValidator.Initialize(typeof(DogValidation).Assembly);
            DiscoverValidator.ValidateEntity(animals);

            var timeRegular11 = DateTime.Now;
            DiscoverValidator.ValidateEntity(animals);
            var timeRegular21 = DateTime.Now;
            var timeRegular1 = timeRegular21 - timeRegular11;
            Debug.WriteLine("timeRegular: " + timeRegular1);

            var timeParallel11 = DateTime.Now;
            DiscoverValidator.ValidateEntityParallel(animals);
            var timeParallel21 = DateTime.Now;
            var timeParallel1 = timeParallel21 - timeParallel11;
            Debug.WriteLine("timeParallel: " + timeParallel1);

            var timeParallel12 = DateTime.Now;
            DiscoverValidator.ValidateEntityParallel(animals);
            var timeParallel22 = DateTime.Now;
            var timeParallel2 = timeParallel22 - timeParallel12;
            Debug.WriteLine("timeParallel: " + timeParallel2);

            var timeRegular12 = DateTime.Now;
            DiscoverValidator.ValidateEntity(animals);
            var timeRegular22 = DateTime.Now;
            var timeRegular2 = timeRegular22 - timeRegular12;
            Debug.WriteLine("timeRegular: " + timeRegular2);

            var difference = (timeRegular2 + timeRegular1) - (timeParallel1 + timeParallel2);

            Debug.WriteLine($"timeParallel is {difference} faster than timeRegular");

            Assert.IsTrue((timeRegular2 + timeRegular1) > (timeParallel1 + timeParallel2));
        }

        [TestMethod]
        public void ValidateOneUniqueEntityTypeParallelVsAssync()
        {
            var numberOfEntities = 50000;
            var animals = GenerateDogs(numberOfEntities);
            Debug.WriteLine($"Validating {numberOfEntities} entities");
            DiscoverValidator.Initialize(typeof(DogValidation).Assembly);
            DiscoverValidator.ValidateEntity(animals);

            var timeRegular11 = DateTime.Now;
            DiscoverValidator.ValidateEntity(animals);
            var timeRegular21 = DateTime.Now;
            var timeRegular1 = timeRegular21 - timeRegular11;
            Debug.WriteLine("timeRegular: " + timeRegular1);

            var timeParallel11 = DateTime.Now;
            DiscoverValidator.ValidateEntityParallel(animals);
            var timeParallel21 = DateTime.Now;
            var timeParallel1 = timeParallel21 - timeParallel11;
            Debug.WriteLine("timeParallel: " + timeParallel1);

            var timeParallel12 = DateTime.Now;
            DiscoverValidator.ValidateEntityParallel(animals);
            var timeParallel22 = DateTime.Now;
            var timeParallel2 = timeParallel22 - timeParallel12;
            Debug.WriteLine("timeParallel: " + timeParallel2);

            var timeRegular12 = DateTime.Now;
            DiscoverValidator.ValidateEntity(animals);
            var timeRegular22 = DateTime.Now;
            var timeRegular2 = timeRegular22 - timeRegular12;
            Debug.WriteLine("timeRegular: " + timeRegular2);

            var difference = (timeRegular2 + timeRegular1) - (timeParallel1 + timeParallel2);

            Debug.WriteLine($"timeParallel is {difference} faster than timeRegular");

            Assert.IsTrue((timeRegular2 + timeRegular1) > (timeParallel1 + timeParallel2));
        }

        [TestMethod]
        [TestCategory("Times Regular vs Parallel")]
        public void ValidateEntitiesRegularVsParallel()
        {
            var numberOfEntities = 500;
            IList<IAnimal> animals = GenerateAnimals(numberOfEntities);
            Debug.WriteLine($"Validating {numberOfEntities} entities");
            DiscoverValidator.Initialize(typeof(CatValidation).Assembly);
            DiscoverValidator.ValidateMultipleEntities(animals);

            var timeParallel11 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntitiesParallel(animals);
            var timeParallel21 = DateTime.Now;
            var timeParallel1 = timeParallel21 - timeParallel11;
            Debug.WriteLine("timeParallel: " + timeParallel1);

            var timeRegular11 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntities(animals);
            var timeRegular21 = DateTime.Now;
            var timeRegular1 = timeRegular21 - timeRegular11;
            Debug.WriteLine("timeRegular: " + timeRegular1);

            var timeOriginalParallel12 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntitiesParallel(animals);
            var timeParallel22 = DateTime.Now;
            var timeParallel2 = timeParallel22 - timeOriginalParallel12;
            Debug.WriteLine("timeParallel: " + timeParallel2);
            
            var timeRegular12 = DateTime.Now;
            DiscoverValidator.ValidateMultipleEntities(animals);
            var timeRegular22 = DateTime.Now;
            var timeRegular2 = timeRegular22 - timeRegular12;
            Debug.WriteLine("timeRegular: " + timeRegular2);
            
            var difference = (timeRegular2 + timeRegular1) - (timeParallel2 + timeRegular1);

            Debug.WriteLine($"timeParallel is {difference} faster than timeRegular");

            //Assert.IsTrue((timeRegular2 + timeRegular1) > (timeRegular1 + timeParallel2));
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
                var a = _random.Next(1, 11);
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
                if (a >= 10)
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
