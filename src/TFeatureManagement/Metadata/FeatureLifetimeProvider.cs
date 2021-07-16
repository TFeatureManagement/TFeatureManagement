using System;
using System.Linq;

namespace TFeatureManagement.Metadata
{
    public class FeatureLifetimeProvider<TFeature, TFeatureMetadata, TFeatureLifetime> : IFeatureMetadataProvider<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : FeatureMetadataBase<TFeature>, new()
        where TFeatureLifetime : struct, Enum
    {
        public void CreateFeatureMetadata(FeatureMetadataProviderContext<TFeature, TFeatureMetadata> context)
        {
            if (context.FeatureMetadata is IFeatureLifetime<TFeatureLifetime> featureLifetime)
            {
                var featureLifetimeAttribute = context.Attributes.OfType<IFeatureLifetime<TFeatureLifetime>>().SingleOrDefault();
                if (featureLifetimeAttribute != null)
                {
                    featureLifetime.Lifetime = featureLifetimeAttribute.Lifetime;
                }
            }
        }
    }
}