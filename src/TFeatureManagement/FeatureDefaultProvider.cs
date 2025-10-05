using System.Reflection;

namespace TFeatureManagement
{
    public class FeatureDefaultProvider<TFeature> : IFeatureDefaultProvider<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

        public FeatureDefaultProvider(
            IFeatureNameProvider<TFeature> featureNameProvider)
        {
            _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));

            FeatureIsEnabledDefaults = [];

#if NET8_0_OR_GREATER
            var features = Enum.GetValues<TFeature>();
#else
        var features = Enum
            .GetValues(typeof(TFeature))
            .Cast<TFeature>();
#endif
            foreach (var feature in features)
            {
                var featureFieldInfo = typeof(TFeature).GetField(_featureNameProvider.GetFeatureName(feature));
                if (featureFieldInfo != null)
                {
                    var isEnabledDefaultAttribute = featureFieldInfo.GetCustomAttribute<FeatureIsEnabledDefaultAttribute>(false);
                    if (isEnabledDefaultAttribute != null)
                    {
                        FeatureIsEnabledDefaults.Add(feature, isEnabledDefaultAttribute.IsEnabled);
                    }
                }
            }
        }

        internal Dictionary<TFeature, bool> FeatureIsEnabledDefaults { get; private set; }

        public bool TryGetIsEnabledDefault(TFeature feature, out bool isEnabledDefault)
        {
            return FeatureIsEnabledDefaults.TryGetValue(feature, out isEnabledDefault);
        }
    }
}
