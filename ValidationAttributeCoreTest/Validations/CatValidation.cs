using ValidationAttributeCore.CustomAttribute;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCoreTest.Model.Animals;

namespace ValidationAttributeCoreTest.Validations
{
    [ValidateEntity(typeof(Cat))]
    public class CatValidation : AbstractAttributeValidator<Cat>, IFindValidation
    {
        public CatValidation()
        {
            RuleFor(cat => cat.Type).NotEmpty().WithMessage("Please specify a type");
            RuleFor(cat => cat.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(cat => cat.Age).GreaterThan(-1).WithMessage("Please specify a valid age");
            RuleFor(cat => cat.CanFly).NotEqual(true).WithMessage("Cats can't fly");
        }
    }
}