using DiscoverValidation.GenericValidator;
using DiscoverValidationTest.Model.Animals;
using FluentValidation;

namespace DiscoverValidationTest.Validations
{
    public class BirdValidation : AbstractDiscoverValidator<Bird>
    {
        public BirdValidation()
        {
            RuleFor(bird => bird.Type).NotEmpty().WithMessage("Please specify a type");
            RuleFor(bird => bird.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(bird => bird.Age).GreaterThan(-1).WithMessage("Please specify a valid age");
            RuleFor(bird => bird.CanFly).NotEqual(false).WithMessage("Birds can fly");
        }
    }

    public class BirdValidation2 : AbstractDiscoverValidator<Bird>
    {
        public BirdValidation2()
        {
            RuleFor(bird => bird.Type).NotEmpty().WithMessage("Please specify a type");
            RuleFor(bird => bird.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(bird => bird.Age).GreaterThan(-1).WithMessage("Please specify a valid age");
            RuleFor(bird => bird.CanFly).NotEqual(false).WithMessage("Birds can fly");
        }
    }

    public class BirdValidation3 : AbstractDiscoverValidator<Bird>
    {
        public BirdValidation3()
        {
            RuleFor(bird => bird.Type).NotEmpty().WithMessage("Please specify a type");
            RuleFor(bird => bird.Name).NotEmpty().WithMessage("Please specify a name");
            RuleFor(bird => bird.Age).GreaterThan(-1).WithMessage("Please specify a valid age");
            RuleFor(bird => bird.CanFly).NotEqual(false).WithMessage("Birds can fly");
        }
    }
}