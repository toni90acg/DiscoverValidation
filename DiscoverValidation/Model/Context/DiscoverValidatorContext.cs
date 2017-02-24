using System;
using System.Collections.Generic;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Model.Interface;
using DiscoverValidation.Model.ValidationResults;

namespace DiscoverValidation.Model.Context
{
    public class DiscoverValidatorContext
    {
        internal Dictionary<Type, Type> AllValidatorsDictionary;
        internal Dictionary<Type, IDiscoverValidator> ValidatorsInstancesDictionary;
        internal DiscoverValidationResults DiscoverValidationResults;
    }
}
