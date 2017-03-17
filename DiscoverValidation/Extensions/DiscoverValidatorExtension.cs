using System;
using DiscoverValidation.Model.ValidationResults;

namespace DiscoverValidation.Extensions
{
    internal static class DiscoverValidatorExtension
    {
        internal static EntityWithMultipleValidators AddValidatorType(
            this EntityWithMultipleValidators entityWithMultipleValidators, Type validatorType)
        {
            entityWithMultipleValidators.Validators.Add(validatorType);
            return entityWithMultipleValidators;
        }
    }
}