using ValidationAttributeCoreTest.Model.Animals.Interface;
using ValidationAttributeCoreTest.Validations;

namespace ValidationAttributeCoreTest.Model.Animals
{
    [MyValidation(typeof(BirdValidation))]
    public class Bird : IAnimal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public bool CanFly { get; set; }
        public string Type { get; set; }

        public Bird(string name, int age, bool canFly, string type)
        {
            Name = name;
            Age = age;
            CanFly = canFly;
            Type = type;
        }
    }
}