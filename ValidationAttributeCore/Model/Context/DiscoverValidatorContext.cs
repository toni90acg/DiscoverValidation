using System;
using System.Collections.Generic;
using ValidationAttributeCore.GenericValidator;
using ValidationAttributeCore.Model.ValidationResults;

namespace ValidationAttributeCore.Model.Context
{
    public class DiscoverValidatorContext
    {
        internal Dictionary<Type, Type> AllValidatorsDictionary;
        internal Dictionary<Type, IDiscoverValidator> ValidatorsInstancesDictionary;
        internal DiscoverValidationResults DiscoverValidationResults;

    }
}
