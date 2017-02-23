using System;
using System.Collections.Generic;
using DiscoverValidationCore.GenericValidator;
using DiscoverValidationCore.Model.ValidationResults;

namespace DiscoverValidationCore.Model.Context
{
    public class DiscoverValidatorContext
    {
        internal Dictionary<Type, Type> AllValidatorsDictionary;
        internal Dictionary<Type, IDiscoverValidator> ValidatorsInstancesDictionary;
        internal DiscoverValidationResults DiscoverValidationResults;

    }
}
