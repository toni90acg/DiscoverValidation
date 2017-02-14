using ValidationAttributeTest.Model.Animals.Interface;
using ValidationAttributeTest.Validations;
using ValidationAttribute.CustomAttribute;

namespace ValidationAttributeTest.Model.Animals
{
    [MyValidationAttribute(typeof(CatValidation))]
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