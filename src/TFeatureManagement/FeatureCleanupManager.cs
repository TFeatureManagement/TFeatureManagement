using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TFeatureManagement
{
    /// <inheritdoc cref="IFeatureCleanupManager{TFeature}" />
    public class FeatureCleanupManager<TFeature> : IFeatureCleanupManager<TFeature>
        where TFeature : Enum
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

            foreach (var feature in Enum.GetValues(typeof(TFeature)).Cast<TFeature>().ToList())
            {
                var featureMemberInfo = typeof(TFeature).GetMember(feature.ToString()).FirstOrDefault();
                if (featureMemberInfo != null)
                {
                    var featureCleanupDateAttribute = featureMemberInfo.GetCustomAttribute<TFeatureCleanupDate>(false);
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