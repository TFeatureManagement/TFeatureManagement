using System;
using System.Collections.Generic;

namespace TFeatureManagement.Metadata
{
    /// <summary>
    /// Used to manage feature cleanup.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureCleanupManager<TFeature>
        where TFeature : struct, Enum
    {
        /// <summary>
        /// Gets feature cleanup dates.
        /// </summary>
        /// <typeparam name="TFeatureCleanupDate">The feature cleanup date attribute type.</typeparam>
        /// <returns>The feature cleanup dates.</returns>
        public IDictionary<TFeature, TFeatureCleanupDate> GetFeatureCleanupDates<TFeatureCleanupDate>()
            where TFeatureCleanupDate : Attribute, IFeatureCleanupDate;

        /// <summary>
        /// Gets a list of feature names registered in the feature manager that do not have a matching value in the
        /// specified feature enum.
        /// </summary>
        /// <returns>
        /// An enumerator which provides asynchronous iteration over the feature names registered in the feature manager
        /// that do not have a matching value in the specified feature enum.
        /// </returns>
        IAsyncEnumerable<string> GetFeatureNamesNotInFeatureEnumAsync();
    }
}