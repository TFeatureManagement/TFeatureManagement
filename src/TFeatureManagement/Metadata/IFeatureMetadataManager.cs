using System;
using System.Collections.Generic;

namespace TFeatureManagement.Metadata
{
    /// <summary>
    /// Used to manage feature metadata.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <typeparam name="TFeatureMetadata">The feature metadata type.</typeparam>
    public interface IFeatureMetadataManager<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
    {
        /// <summary>
        /// Gets features and their metadata.
        /// </summary>
        /// <returns>The features and their metadata.</returns>
        public IEnumerable<TFeatureMetadata> GetFeaturesAndMetadata();
    }
}