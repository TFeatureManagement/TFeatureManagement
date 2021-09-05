using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFeatureManagement.Extensions;

namespace TFeatureManagement
{
    public static class FeatureManagerExtensions
    {
        /// <summary>
        /// Checks whether a given set of features are enabled.
        /// </summary>
        /// <param name="requirementType">
        /// Specifies whether all or any of the given set of features should be enabled.
        /// </param>
        /// <param name="features">The features to check.</param>
        /// <returns>True if the features are enabled, otherwise false.</returns>
        public static async Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, RequirementType requirementType, params TFeature[] features)
            where TFeature : struct, Enum
        {
            return await featureManager.IsEnabledAsync(requirementType, features.AsEnumerable()).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks whether a given set of features are enabled.
        /// </summary>
        /// <param name="requirementType">
        /// Specifies whether all or any of the given set of features should be enabled.
        /// </param>
        /// <param name="features">The features to check.</param>
        /// <returns>True if the features are enabled, otherwise false.</returns>
        public static async Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, RequirementType requirementType, IEnumerable<TFeature> features)
            where TFeature : struct, Enum
        {
            return requirementType == RequirementType.All ?
                await features.AllAsync(async feature => await featureManager.IsEnabledAsync(feature).ConfigureAwait(false)).ConfigureAwait(false) :
                await features.AnyAsync(async feature => await featureManager.IsEnabledAsync(feature).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}