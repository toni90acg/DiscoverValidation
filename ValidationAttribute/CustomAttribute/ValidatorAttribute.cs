using System;
using ValidationAttribute.GenericValidator;
using ValidationAttribute.Exceptions;

namespace ValidationAttribute.CustomAttribute
{
    public class ValidatorAttribute : Attribute
    {
        public IAttributeValidator Validator { get; set; }

        public ValidatorAttribute(Type validator)
        {
            if (!typeof(IAttributeValidator).IsAssignableFrom(validator))
                throw new ValidationAttributeException(validator.Name);

            Validator = (IAttributeValidator) Activator.CreateInstance(validator);
        }
    }
}
