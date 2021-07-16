using System;
using System.Linq;

namespace TFeatureManagement.Metadata
{
    public class FeatureCategoryProvider<TFeature, TFeatureMetadata, TFeatureCategory> : IFeatureMetadataProvider<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : FeatureMetadataBase<TFeature>, new()
        where TFeatureCategory : struct, Enum
    {
        public void CreateFeatureMetadata(FeatureMetadataProviderContext<TFeature, TFeatureMetadata> context)
        {
            if (context.FeatureMetadata is IFeatureCategory<TFeatureCategory> featureCategory)
            {
                var featureCategoryAttribute = context.Attributes.OfType<IFeatureCategory<TFeatureCategory>>().SingleOrDefault();
                if (featureCategoryAttribute != null)
                {
                    featureCategory.Category = featureCategoryAttribute.Category;
                }
            }
        }
    }
}