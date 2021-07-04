using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TFeatureManagement
{
    /// <inheritdoc cref="IFeatureCleanupManager{TFeature}" />
    public class FeatureCleanupManager<TFeature> : IFeatureCleanupManager<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IFeatureManager<TFeature> _featureManager;

        /// <summary>
        /// Creates a feature cleanup manager.
        /// </summary>
        /// <param name="featureManager">The feature manager.</param>
        public FeatureCleanupManager(IFeatureManager<TFeature> featureManager)
        {
            _featureManager = featureManager;
        }

        /// <inheritdoc />
        public IDictionary<TFeature, TFeatureCleanupDate> GetFeatureCleanupDates<TFeatureCleanupDate>()
            where TFeatureCleanupDate : Attribute, IFeatureCleanupDate
        {
            var featureCleanupDates = new Dictionary<TFeature, TFeatureCleanupDate>();

#if NET5_0
            var features = Enum.GetValues<TFeature>();
#else
            var features = Enum.GetValues(typeof(TFeature)).Cast<TFeature>();
#endif

            foreach (var feature in features)
            {
                var featureFieldInfo = typeof(TFeature).GetField(feature.ToString());
                if (featureFieldInfo != null)
                {
                    var featureCleanupDateAttribute = featureFieldInfo.GetCustomAttribute<TFeatureCleanupDate>(false);
                    featureCleanupDates.Add(feature, featureCleanupDateAttribute);
                }
            }

            return featureCleanupDates;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<string> GetFeatureNamesNotInFeatureEnumAsync()
        {
            var featureKeys = Enum.GetValues(typeof(TFeature)).Cast<TFeature>().Select(f => f.ToString()).ToList();

            await foreach (var featureName in _featureManager.GetFeatureNamesAsync())
            {
                if (!featureKeys.Contains(featureName))
                {
                    yield return featureName;
                }
            }
        }
    }
}