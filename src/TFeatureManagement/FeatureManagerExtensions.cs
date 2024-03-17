﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TFeatureManagement.Extensions;

namespace TFeatureManagement
{
    public static class FeatureManagerExtensions
    {
        /// <summary>
        /// Checks whether a given set of features are enabled.
        /// </summary>
        /// <param name="features">The features to check.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns><see langword="true"/> if the features are enabled; otherwise, <see langword="false"/>.</returns>
        public static Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, IEnumerable<TFeature> features, CancellationToken cancellationToken = default)
            where TFeature : struct, Enum
        {
            return featureManager.IsEnabledAsync(RequirementType.All, features, cancellationToken);
        }
        /// <summary>
        /// Checks whether a given set of features are enabled.
        /// </summary>
        /// <param name="requirementType">
        /// Specifies whether to check if all or any of the given set of features are enabled.
        /// </param>
        /// <param name="features">The features to check.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns><see langword="true"/> if the features are enabled; otherwise, <see langword="false"/>.</returns>
        public static async Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, RequirementType requirementType, IEnumerable<TFeature> features, CancellationToken cancellationToken = default)
            where TFeature : struct, Enum
        {
            var isEnabled = requirementType == RequirementType.All || requirementType == RequirementType.NotAll ?
                await features.AllAsync(featureManager.IsEnabledAsync, cancellationToken).ConfigureAwait(false) :
                await features.AnyAsync(featureManager.IsEnabledAsync, cancellationToken).ConfigureAwait(false);

            if (requirementType == RequirementType.NotAny || requirementType == RequirementType.NotAll)
            {
                isEnabled = !isEnabled;
            }

            return isEnabled;
        }
    }
}