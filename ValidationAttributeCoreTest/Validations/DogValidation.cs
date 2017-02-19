using FluentValidation;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCoreTest.Model.Animals;

namespace ValidationAttributeCoreTest.Validations
{
    [ValidateEntity(typeof(Dog))]
    public class DogValidation : AbstractDiscoverValidator<Dog>
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