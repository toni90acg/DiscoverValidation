using System;
using System.Collections.Generic;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.ValidationResults;

namespace DiscoverValidation.Model.Context
{
    public class DiscoverValidatorContext
    {
        internal Dictionary<Type, Type> ValidatorsTypesDictionary;
        internal List<EntityWithMultipleValidators> EntitiesWithMultiplesValidators;
        internal Dictionary<Type, IDiscoverValidator> ValidatorsInstancesDictionary;
        internal List<Pair<Type, IDiscoverValidator>> ValidatorsAlternativeInstances;
        internal DiscoverValidationResults DiscoverValidationResults;
    }
}
