using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TFeatureManagement.Metadata
{
    /// <inheritdoc cref="IFeatureCleanupManager{TFeature}" />
    public class FeatureMetadataManager<TFeature> : IFeatureMetadataManager<TFeature>
        where TFeature : Enum
    {
        /// <inheritdoc />
        public IDictionary<TFeature, TFeatureMetadata> GetFeaturesAndMetadata<TFeatureMetadata>()
            where TFeatureMetadata : Attribute, IFeatureMetadata
        {
            var features = new Dictionary<TFeature, TFeatureMetadata>();

            foreach (var feature in Enum.GetValues(typeof(TFeature)).Cast<TFeature>().ToList())
            {
                var featureMemberInfo = typeof(TFeature).GetMember(feature.ToString()).FirstOrDefault();
                if (featureMemberInfo != null)
                {
                    var featureMetadataAttribute = featureMemberInfo.GetCustomAttribute<TFeatureMetadata>(false);
                    features.Add(feature, featureMetadataAttribute);
                }
            }

            return features;
        }

        /// <inheritdoc />
        public IEnumerable<IGrouping<TFeatureCategory?, TFeature>> GetFeaturesByCategory<TFeatureCategory, TFeatureCategoryAttribute>()
            where TFeatureCategory : struct, Enum
            where TFeatureCategoryAttribute : Attribute, IFeatureCategory<TFeatureCategory>
        {
            var featureCategories = GetFeatureAttributes<TFeatureCategoryAttribute>();

            return featureCategories.GroupBy(x => x.Value?.Category, x => x.Key).OrderBy(x => x.Key?.ToString()).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IGrouping<TFeatureLifetime?, TFeature>> GetFeaturesByLifetime<TFeatureLifetime, TFeatureLifetimeAttribute>()
            where TFeatureLifetime : struct, Enum
            where TFeatureLifetimeAttribute : Attribute, IFeatureLifetime<TFeatureLifetime>
        {
            var featureLifetimes = GetFeatureAttributes<TFeatureLifetimeAttribute>();

            return featureLifetimes.GroupBy(x => x.Value?.Lifetime, x => x.Key).OrderBy(x => x.Key?.ToString()).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IGrouping<TFeatureTeam?, TFeature>> GetFeaturesByTeam<TFeatureTeam, TFeatureTeamAttribute>()
            where TFeatureTeam : struct, Enum
            where TFeatureTeamAttribute : Attribute, IFeatureTeam<TFeatureTeam>
        {
            var featureTeams = GetFeatureAttributes<TFeatureTeamAttribute>();

            return featureTeams.GroupBy(x => x.Value?.Team, x => x.Key).OrderBy(x => x.Key?.ToString()).ToList();
        }

        private static IDictionary<TFeature, TAttribute> GetFeatureAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            var features = new Dictionary<TFeature, TAttribute>();

            foreach (var feature in Enum.GetValues(typeof(TFeature)).Cast<TFeature>().ToList())
            {
                var featureMemberInfo = typeof(TFeature).GetMember(feature.ToString()).FirstOrDefault();
                var attribute = featureMemberInfo?.GetCustomAttribute<TAttribute>(false);
                features.Add(feature, attribute);
            }

            return features;
        }
    }
}