﻿using System;

namespace DiscoverValidationCore.CustomAttribute
{
    public class ValidateEntityAttribute : Attribute
    {
        public Type Entity { get; set; }

        public ValidateEntityAttribute(Type entity)
        {
            Entity = entity;
        }
    }
}
