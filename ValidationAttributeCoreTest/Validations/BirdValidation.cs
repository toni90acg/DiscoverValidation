using FluentValidation;
using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCoreTest.Model.Animals;

namespace ValidationAttributeCoreTest.Validations
{
    [ValidateEntity(typeof(Bird))]
    public class BirdValidation : AbstractAttributeValidator<Bird>, IFindValidation
    {
        public BirdValidation()
        {
            RuleFor(bird => bird.Type).NotEmpty().WithMessage("Please specify a type");
            RuleFor(bird => bird.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(bird => bird.Age).GreaterThan(-1).WithMessage("Please specify a valid age");
            RuleFor(bird => bird.CanFly).NotEqual(false).WithMessage("Birds can fly");
        }
    }
}