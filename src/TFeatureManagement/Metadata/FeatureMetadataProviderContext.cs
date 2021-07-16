using System;
using System.Collections.Generic;
using System.Linq;

namespace TFeatureManagement.Metadata
{
    public class FeatureMetadataProviderContext<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
    {
        public FeatureMetadataProviderContext(TFeature feature, IEnumerable<object> attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            Feature = feature;
            FeatureMetadata = new TFeatureMetadata
            {
                Feature = feature
            };
            Attributes = attributes.ToArray();
        }

        public TFeature Feature { get; }

        public TFeatureMetadata FeatureMetadata { get; }

        public IReadOnlyList<object> Attributes { get; }
    }
}