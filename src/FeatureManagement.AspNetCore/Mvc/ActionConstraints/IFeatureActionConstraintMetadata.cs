using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;

namespace FeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    public interface IFeatureActionConstraintMetadata<TFeature> : IActionConstraintMetadata
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

        /// <inheritdoc cref="IActionConstraint.Order" />
        int Order { get; }
    }
}