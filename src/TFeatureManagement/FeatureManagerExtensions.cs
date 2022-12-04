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
        /// <param name="features">The features to check.</param>
        /// <returns><see langword="true"/> if the features are enabled; otherwise, <see langword="false"/>.</returns>
        public static Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, params TFeature[] features)
            where TFeature : struct, Enum
        {
            return featureManager.IsEnabledAsync(RequirementType.All, features.AsEnumerable());
        }

        /// <summary>
        /// Checks whether a given set of features are enabled.
        /// </summary>
        /// <param name="features">The features to check.</param>
        /// <returns><see langword="true"/> if the features are enabled; otherwise, <see langword="false"/>.</returns>
        public static Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, IEnumerable<TFeature> features)
            where TFeature : struct, Enum
        {
            return featureManager.IsEnabledAsync(RequirementType.All, features);
        }

        /// <summary>
        /// Checks whether a given set of features are enabled.
        /// </summary>
        /// <param name="requirementType">
        /// Specifies whether to check if all or any of the given set of features are enabled.
        /// </param>
        /// <param name="features">The features to check.</param>
        /// <returns><see langword="true"/> if the features are enabled; otherwise, <see langword="false"/>.</returns>
        public static Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, RequirementType requirementType, params TFeature[] features)
            where TFeature : struct, Enum
        {
            return featureManager.IsEnabledAsync(requirementType, features.AsEnumerable());
        }

        /// <summary>
        /// Checks whether a given set of features are enabled.
        /// </summary>
        /// <param name="requirementType">
        /// Specifies whether to check if all or any of the given set of features are enabled.
        /// </param>
        /// <param name="features">The features to check.</param>
        /// <returns><see langword="true"/> if the features are enabled; otherwise, <see langword="false"/>.</returns>
        public static async Task<bool> IsEnabledAsync<TFeature>(this IFeatureManager<TFeature> featureManager, RequirementType requirementType, IEnumerable<TFeature> features)
            where TFeature : struct, Enum
        {
            var isEnabled = requirementType == RequirementType.All || requirementType == RequirementType.NotAll ?
                await features.AllAsync(featureManager.IsEnabledAsync).ConfigureAwait(false) :
                await features.AnyAsync(featureManager.IsEnabledAsync).ConfigureAwait(false);

            if (requirementType == RequirementType.NotAny || requirementType == RequirementType.NotAll)
            {
                isEnabled = !isEnabled;
            }

            return isEnabled;
        }
    }
}