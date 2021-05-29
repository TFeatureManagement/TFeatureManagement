using System;
using System.Collections.Generic;

namespace TFeatureManagement
{
    public interface IFeatureCleanupManager<TFeature>
        where TFeature : Enum
    {
        /// <summary>
        /// Gets feature cleanup dates.
        /// </summary>
        /// <returns>The feature cleanup dates.</returns>
        IDictionary<TFeature, IFeatureCleanupDate> GetFeatureCleanupDates();

        /// <summary>
        /// Gets a list of feature names registered in the feature manager that do not have a matching value in the
        /// specified feature enum.
        /// </summary>
        /// <returns>The feature names requiring cleanup.</returns>
        IAsyncEnumerable<string> GetFeatureNamesNotInFeatureEnumAsync();
    }
}