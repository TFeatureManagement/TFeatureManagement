using System;
using System.Collections.Generic;
using System.Linq;

namespace TFeatureManagement.Metadata
{
    /// <summary>
    /// Used to manage feature metadata.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureMetadataManager<TFeature>
        where TFeature : Enum
    {
        /// <summary>
        /// Gets features and their metadata.
        /// </summary>
        /// <typeparam name="TFeatureMetadata">The feature metadata attribute type.</typeparam>
        /// <returns>The features and their metadata.</returns>
        IDictionary<TFeature, TFeatureMetadata> GetFeaturesAndMetadata<TFeatureMetadata>()
            where TFeatureMetadata : Attribute, IFeatureMetadata;

        /// <summary>
        /// Gets features grouped by category.
        /// </summary>
        /// <typeparam name="TFeatureCategory">The feature category enum type.</typeparam>
        /// <typeparam name="TFeatureCategoryAttribute">The feature category attribute type.</typeparam>
        /// <returns>The features grouped by category.</returns>
        public IEnumerable<IGrouping<TFeatureCategory?, TFeature>> GetFeaturesByCategory<TFeatureCategory, TFeatureCategoryAttribute>()
            where TFeatureCategory : struct, Enum
            where TFeatureCategoryAttribute : Attribute, IFeatureCategory<TFeatureCategory>;

        /// <summary>
        /// Gets features grouped by lifetime.
        /// </summary>
        /// <typeparam name="TFeatureLifetime">The feature lifetime enum type.</typeparam>
        /// <typeparam name="TFeatureLifetimeAttribute">The feature lifetime attribute type.</typeparam>
        /// <returns>The features grouped by lifetime.</returns>
        public IEnumerable<IGrouping<TFeatureLifetime?, TFeature>> GetFeaturesByLifetime<TFeatureLifetime, TFeatureLifetimeAttribute>()
            where TFeatureLifetime : struct, Enum
            where TFeatureLifetimeAttribute : Attribute, IFeatureLifetime<TFeatureLifetime>;

        /// <summary>
        /// Gets features grouped by team.
        /// </summary>
        /// <typeparam name="TFeatureTeam">The feature team enum type.</typeparam>
        /// <typeparam name="TFeatureTeamAttribute">The feature team attribute type.</typeparam>
        /// <returns>The features grouped by team.</returns>
        public IEnumerable<IGrouping<TFeatureTeam?, TFeature>> GetFeaturesByTeam<TFeatureTeam, TFeatureTeamAttribute>()
            where TFeatureTeam : struct, Enum
            where TFeatureTeamAttribute : Attribute, IFeatureTeam<TFeatureTeam>;
    }
}