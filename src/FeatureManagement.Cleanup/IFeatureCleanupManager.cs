using System;
using System.Collections.Generic;

namespace FeatureManagement
{
    public interface IFeatureCleanupManager<TFeature>
        where TFeature : Enum
    {
        /// <summary>
        /// Gets feature clean-up dates.
        /// </summary>
        /// <returns>The feature clean-up dates.</returns>
        IDictionary<TFeature, IFeatureCleanupDate> GetFeatureCleanupDates();

        /// <summary>
        /// Gets a list of feature names registered in the feature manager that do not have a
        /// matching value in the specified feature enum.
        /// </summary>
        /// <returns>The feature names requiring clean-up.</returns>
        IAsyncEnumerable<string> GetFeatureNamesNotInFeatureEnumAsync();
    }
}