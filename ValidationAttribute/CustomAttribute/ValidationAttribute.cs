using System;
using ValidationAttribute.Exceptions;
using ValidationAttribute.GenericValidator;

namespace ValidationAttribute.CustomAttribute
{
    public class ValidationAttribute : Attribute
    {
        public IAttributeValidator Validator { get; set; }

        public ValidationAttribute(Type validator)
        {
            if (!typeof(IAttributeValidator).IsAssignableFrom(validator))
                throw new ValidationAttributeException(validator.Name);

            Validator = (IAttributeValidator) Activator.CreateInstance(validator);
        }
    }
}
