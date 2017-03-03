using System;

namespace DiscoverValidation.Exceptions
{
    internal class DiscoverValidationCreatingDataException : Exception
    {
        public DiscoverValidationCreatingDataException(string message,
            Exception innerException) : base(message, innerException)
        {
        }
    }

    internal class DiscoverValidationCreatingValidatorException : Exception
    {
        public DiscoverValidationCreatingValidatorException(string message) : base(message)
        {
        }
    }
}