﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// An action filter that can be used to require all or any of a set of features to be enabled for an action to be
    /// enabled. If the required features are not enabled the registered <see
    /// cref="T:Microsoft.FeatureManagement.Mvc.IDisabledFeaturesHandler" /> will be invoked.
    /// </summary>
    public class FeatureActionFilter<TFeature> : IAsyncActionFilter, IOrderedFilter
        where TFeature : Enum
    {
        private readonly FeatureGateAttribute _featureGateAttribute;

        /// <summary>
        /// Creates an action filter that requires all the provided feature(s) to be enabled for the actions to be
        /// enabled.
        /// </summary>
        /// <param name="features">The features that should be enabled.</param>
        public FeatureActionFilter(IEnumerable<TFeature> features)
            : this(features, RequirementType.All)
        {
        }

        /// <summary>
        /// Creates an action filter that requires the provided feature(s) to be enabled for the actions to be enabled.
        /// The filter can be configured to require all or any of the provided feature(s) to be enabled.
        /// </summary>
        /// <param name="features">The features that should be enabled.</param>
        /// <param name="requirementType">
        /// Specifies whether all or any of the provided features should be enabled.
        /// </param>
        public FeatureActionFilter(IEnumerable<TFeature> features, RequirementType requirementType)
        {
            if (features?.Any() != true)
            {
                throw new ArgumentNullException(nameof(features));
            }

            Features = features;
            RequirementType = requirementType;

            _featureGateAttribute = new FeatureGateAttribute(requirementType, features);
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

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await _featureGateAttribute.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }
    }
}