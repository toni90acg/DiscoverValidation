using System;
using System.Collections.Generic;

namespace DiscoverValidation.Model.ValidationResults
{
    internal class EntityWithMultipleValidators
    {
        internal Type EntityType { get; set; }
        internal IList<Type> Validators { get; set; }

        public EntityWithMultipleValidators(Type entity)
        {
            EntityType = entity;
            Validators = new List<Type>();
        }
    }
}
