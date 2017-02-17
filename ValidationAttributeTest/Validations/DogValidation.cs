using FluentValidation;
using ValidationAttribute.GenericValidator;
using ValidationAttributeTest.Model.Animals;


namespace ValidationAttributeTest.Validations
{
    public class DogValidation : AbstractAttributeValidator<Dog>
    {
        public DogValidation()
        {
            RuleFor(dog => dog.Type).NotEmpty().WithMessage("Please specify a type");
            RuleFor(dog => dog.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(dog => dog.Age).GreaterThan(-1).WithMessage("Please specify a valid age");
            RuleFor(dog => dog.CanFly).NotEqual(true).WithMessage("Dogs can't fly");
        }
    }
}