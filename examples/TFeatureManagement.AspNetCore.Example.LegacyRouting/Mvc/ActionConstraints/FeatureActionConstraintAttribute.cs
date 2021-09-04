using System;
using System.Collections.Generic;
using TFeatureManagement.AspNetCore.Example.Models;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;

namespace TFeatureManagement.AspNetCore.Example.Mvc.ActionConstraints
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FeatureActionConstraintAttribute : Attribute, IFeatureActionConstraintMetadata<Feature>
    {
        public FeatureActionConstraintAttribute(params Feature[] features)
            : this(RequirementType.All, features)
        {
        }

        public FeatureActionConstraintAttribute(RequirementType requirementType, params Feature[] features)
        {
            Features = features;
            RequirementType = requirementType;
        }

        public IEnumerable<Feature> Features { get; }

        public RequirementType RequirementType { get; }

        public int Order { get; set; }
    }
}