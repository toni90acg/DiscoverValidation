using System;
using System.Collections.Generic;
using DiscoverValidationPortable.GenericValidator;
using DiscoverValidationPortable.Model.ValidationResults;

namespace DiscoverValidationPortable.Model.Context
{
    public class DiscoverValidatorContext
    {
        internal Dictionary<Type, Type> AllValidatorsDictionary;
        internal Dictionary<Type, IDiscoverValidator> ValidatorsInstancesDictionary;
        internal DiscoverValidationResults DiscoverValidationResults;

    }
}
