﻿using DiscoverValidationTest.Model.Animals.Interface;

namespace DiscoverValidationTest.Model.Animals
{
    //BigFoot doesn't have any validation
    public class BigFoot : IAnimal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool CanFly { get; set; }
        public string Type { get; set; }
    }
}
