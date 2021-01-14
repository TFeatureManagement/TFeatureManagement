using FeatureManagement.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureManagement
{
    public class FeatureCleanupManager<TFeature> : IFeatureCleanupManager<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IFeatureManager<TFeature> _featureManager;

        public FeatureCleanupManager(IFeatureManager<TFeature> featureManager)
        {
            _featureManager = featureManager;
        }

        public IDictionary<TFeature, IFeatureCleanupDate> GetFeatureCleanupDates()
        {
            var featureCleanupDates = new Dictionary<TFeature, IFeatureCleanupDate>();

            foreach (var feature in Enum.GetValues(typeof(TFeature)).Cast<TFeature>().ToList())
            {
                var featureMemberInfo = typeof(TFeature).GetMember(feature.ToString()).FirstOrDefault();
                if (featureMemberInfo != null)
                {
                    var featureCleanupDateAttribute = featureMemberInfo.GetCustomAttributes(typeof(FeatureCleanupDateAttribute), false).Cast<FeatureCleanupDateAttribute>().FirstOrDefault();
                    featureCleanupDates.Add(feature, featureCleanupDateAttribute);
                }
            }

            return featureCleanupDates;
        }

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