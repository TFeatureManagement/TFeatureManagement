using System;
using System.ComponentModel;
using System.Linq;

namespace TFeatureManagement.Metadata
{
    public class FeatureDescriptionProvider<TFeature, TFeatureMetadata> : IFeatureMetadataProvider<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
    {
        public void CreateFeatureMetadata(FeatureMetadataProviderContext<TFeature, TFeatureMetadata> context)
        {
            if (context.FeatureMetadata is IFeatureDescription featureDescription)
            {
                var descriptionAttribute = context.Attributes.OfType<DescriptionAttribute>().SingleOrDefault();
                var featureDescriptionAttribute = context.Attributes.OfType<IFeatureDescription>().SingleOrDefault();
                if (featureDescriptionAttribute != null)
                {
                    featureDescription.Description = featureDescriptionAttribute.Description;
                }
                else if (descriptionAttribute != null)
                {
                    featureDescription.Description = descriptionAttribute.Description;
                }
            }
        }
    }
}