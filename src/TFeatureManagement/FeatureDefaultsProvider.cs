using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace TFeatureManagement
{
    public class FeatureDefaultsProvider<TFeature> : IFeatureDefaultsProvider<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IFeatureNameProvider<TFeature> _featureNameProvider;
        private readonly IHostEnvironment _environment;

        public FeatureDefaultsProvider(
            IFeatureNameProvider<TFeature> featureNameProvider,
            IHostEnvironment environment)
        {
            _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));

            IsEnabledDefaults = new Dictionary<TFeature, bool>();
            IsEnabledDefaultsByFeatureName = new Dictionary<string, bool>();

#if NET8_0_OR_GREATER
            var features = Enum.GetValues<TFeature>();
#else
            var features = Enum
                .GetValues(typeof(TFeature))
                .Cast<TFeature>();
#endif
            foreach (var feature in features)
            {
                var featureName = feature.ToString();

                var featureFieldInfo = typeof(TFeature).GetField(featureName, BindingFlags.Public | BindingFlags.Static);
                if (featureFieldInfo != null)
                {
                    var isEnabledDefaultAttributes = featureFieldInfo
                        .GetCustomAttributes<FeatureIsEnabledDefaultAttribute>(false);

                    var isEnabledDefaultAttributeEnvironmentGroups = isEnabledDefaultAttributes
                        .GroupBy(x => x.EnvironmentName)
                        .Where(x => x.Count() > 1)
                        .ToList();
                    if (isEnabledDefaultAttributeEnvironmentGroups.Count > 0)
                    {
                        throw new FeatureManagementException(FeatureManagementErrorType.MultipleIsEnabledDefaultAttributes, $"The feature '{featureName}' has multiple is enabled default attributes for an environment(s).");
                    }

                    var isEnabledDefaultAttributeForEnvironment = isEnabledDefaultAttributes
                        .SingleOrDefault(x =>
                            x.EnvironmentName != null &&
                            x.EnvironmentName.Equals(_environment.EnvironmentName, StringComparison.OrdinalIgnoreCase));
                    if (isEnabledDefaultAttributeForEnvironment != null)
                    {
                        IsEnabledDefaults.Add(feature, isEnabledDefaultAttributeForEnvironment.IsEnabled);
                        IsEnabledDefaultsByFeatureName.Add(_featureNameProvider.GetFeatureName(feature), isEnabledDefaultAttributeForEnvironment.IsEnabled);
                    }
                    else
                    {
                        var isEnabledDefaultAttributeForAllEnvironments = isEnabledDefaultAttributes
                            .SingleOrDefault(x => x.EnvironmentName == null);
                        if (isEnabledDefaultAttributeForAllEnvironments != null)
                        {
                            IsEnabledDefaults.Add(feature, isEnabledDefaultAttributeForAllEnvironments.IsEnabled);
                            IsEnabledDefaultsByFeatureName.Add(_featureNameProvider.GetFeatureName(feature), isEnabledDefaultAttributeForAllEnvironments.IsEnabled);
                        }
                    }
                }
            }
        }

        public IDictionary<TFeature, bool> IsEnabledDefaults { get; }

        public IDictionary<string, bool> IsEnabledDefaultsByFeatureName { get; }
    }
}
