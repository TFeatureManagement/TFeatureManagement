using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TFeatureManagement.Metadata
{
    /// <inheritdoc cref="IFeatureCleanupManager{TFeature}" />
    public class FeatureMetadataManager<TFeature, TFeatureMetadata> : IFeatureMetadataManager<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
    {
        private readonly IReadOnlyList<IFeatureMetadataProvider<TFeature, TFeatureMetadata>> _featureMetadataProviders;

        public FeatureMetadataManager(IReadOnlyList<IFeatureMetadataProvider<TFeature, TFeatureMetadata>> featureMetadataProviders)
        {
            _featureMetadataProviders = featureMetadataProviders;
        }

        /// <inheritdoc />
        public IEnumerable<TFeatureMetadata> GetFeaturesAndMetadata()
        {
            var features = new List<TFeatureMetadata>();

            foreach (var feature in Enum.GetValues(typeof(TFeature)).Cast<TFeature>().ToList())
            {
                var featureMemberInfo = typeof(TFeature).GetMember(feature.ToString()).FirstOrDefault();
                if (featureMemberInfo != null)
                {
                    var context = new FeatureMetadataProviderContext<TFeature, TFeatureMetadata>(feature, featureMemberInfo.GetCustomAttributes());

                    foreach (var provider in _featureMetadataProviders)
                    {
                        provider.CreateFeatureMetadata(context);
                    }

                    features.Add(context.FeatureMetadata);
                }
            }

            return features;
        }
    }
}