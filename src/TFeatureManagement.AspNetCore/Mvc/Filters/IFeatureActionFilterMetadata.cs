using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;

namespace TFeatureManagement.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// An interface that identifies a type as metadata for a <see cref="FeatureActionFilter{TFeature}" />.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureActionFilterMetadata<TFeature> : IFilterMetadata
        where TFeature : Enum
    {
        /// <summary>
        /// Gets the features that should be enabled.
        /// </summary>
        IEnumerable<TFeature> Features { get; }

        /// <summary>
        /// Gets whether any or all features in <see cref="Features" /> should be enabled.
        /// </summary>
        RequirementType RequirementType { get; }
    }
}