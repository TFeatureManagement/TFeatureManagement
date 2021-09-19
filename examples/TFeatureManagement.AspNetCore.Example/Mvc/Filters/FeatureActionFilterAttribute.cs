using System;
using System.Collections.Generic;
using TFeatureManagement.AspNetCore.Example.Models;
using TFeatureManagement.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Example.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FeatureActionFilterAttribute : Attribute, IFeatureActionFilterMetadata<Feature>
    {
        public FeatureActionFilterAttribute(params Feature[] features)
            : this(RequirementType.All, features)
        {
        }

        public FeatureActionFilterAttribute(RequirementType requirementType, params Feature[] features)
        {
            Features = features;
            RequirementType = requirementType;
        }

        public IEnumerable<Feature> Features { get; }

        public RequirementType RequirementType { get; }
    }
}