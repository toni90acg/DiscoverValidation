﻿using DiscoverValidationTest.Model.Animals.Interface;

namespace DiscoverValidationTest.Model.Animals
{
    public class Cat : IAnimal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public bool CanFly { get; set; }
        public string Type { get; set; }

        public Cat(string name, int age, bool canFly, string type)
        {
            Name = name;
            Age = age;
            CanFly = canFly;
            Type = type;
        }
    }
}