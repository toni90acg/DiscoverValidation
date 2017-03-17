using System;

namespace DiscoverValidation.Exceptions
{
    public sealed class DiscoverValidationCreatingDataException : Exception
    {
        public DiscoverValidationCreatingDataException(string message,
            Exception innerException) : base(message, innerException)
        {
        }
    }

    public sealed class DiscoverValidationCreatingValidatorException : Exception
    {
        public DiscoverValidationCreatingValidatorException(string message) : base(message)
        {
        }
    }
}