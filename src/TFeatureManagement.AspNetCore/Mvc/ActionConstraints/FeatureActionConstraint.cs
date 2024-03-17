using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    /// <summary>
    /// An action constraint that can be used to require all or any of a set of features to be enabled for an action to
    /// be valid to be selected for the given request.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public class FeatureActionConstraint<TFeature> : IActionConstraint
        where TFeature : struct, Enum
    {
        /// <summary>
        /// Creates an action constraint that requires all the provided feature(s) to be enabled for the action to be
        /// valid to be selected.
        /// </summary>
        /// <param name="features">The features that should be enabled.</param>
        public FeatureActionConstraint(IEnumerable<TFeature> features)
            : this(features, RequirementType.All)
        {
        }

        /// <summary>
        /// Creates an action constraint that requires the provided feature(s) to be enabled for the action to be valid
        /// to be selected. The constraint can be configured to require all or any of the provided feature(s) to be
        /// enabled.
        /// </summary>
        /// <param name="features">The features that should be enabled.</param>
        /// <param name="requirementType">
        /// Specifies whether all or any of the provided features should be enabled.
        /// </param>
        public FeatureActionConstraint(IEnumerable<TFeature> features, RequirementType requirementType)
        {
            if (features?.Any() != true)
            {
                throw new ArgumentNullException(nameof(features));
            }

            Features = features;
            RequirementType = requirementType;
        }

        /// <summary>
        /// Gets the features that should be enabled.
        /// </summary>
        public IEnumerable<TFeature> Features { get; }

        /// <summary>
        /// Gets whether all or any features in <see cref="Features" /> should be enabled.
        /// </summary>
        public RequirementType RequirementType { get; }

        /// <inheritdoc />
        public int Order { get; set; }

        /// <inheritdoc />
        public bool Accept(ActionConstraintContext context)
        {
            var featureManager = context.RouteContext.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

            return Task.Run(() => featureManager.IsEnabledAsync(RequirementType, Features).AsTask()).GetAwaiter().GetResult();
        }
    }
}