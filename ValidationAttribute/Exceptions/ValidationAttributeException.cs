using System;

namespace ValidationAttribute.Exceptions
{
    public class ValidationAttributeException : Exception
    {
        public ValidationAttributeException(string validatorTypeName) : 
            base("The type: " + validatorTypeName + " doesn't implement de IAttributeValidator interface.")
        {
        }
    }
}