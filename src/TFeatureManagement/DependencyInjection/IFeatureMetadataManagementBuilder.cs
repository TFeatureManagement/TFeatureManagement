using System;
using TFeatureManagement.Metadata;

namespace TFeatureManagement.DependencyInjection
{
    public interface IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
    {
        /// <summary>
        /// Adds the specified feature metadata provider to the list of feature metadata providers used to populate
        /// feature metadata.
        /// </summary>
        /// <typeparam name="TFeatureMetadataProvider">The type of the feature metadata provider.</typeparam>
        /// <returns>The feature metadata management builder.</returns>
        IFeatureMetadataManagementBuilder<TFeature, TFeatureMetadata> AddFeatureMetadataProvider<TFeatureMetadataProvider>()
            where TFeatureMetadataProvider : class, IFeatureMetadataProvider<TFeature, TFeatureMetadata>;
    }
}