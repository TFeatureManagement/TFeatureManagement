using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TFeatureManagement.Metadata
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

#if NET6_0_OR_GREATER
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
        public async IAsyncEnumerable<string> GetFeatureNamesNotInFeatureEnumAsync([EnumeratorCancellation]CancellationToken cancellationToken = default)
        {
            var featureKeys = Enum.GetValues(typeof(TFeature)).Cast<TFeature>().Select(f => f.ToString()).ToList();

            await foreach (var featureName in _featureManager.GetFeatureNamesAsync(cancellationToken))
            {
                if (!featureKeys.Contains(featureName))
                {
                    yield return featureName;
                }
            }
        }
    }
}