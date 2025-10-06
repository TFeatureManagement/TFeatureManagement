using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace TFeatureManagement
{
    public class FeatureDefaultProvider<TFeature> : IFeatureDefaultProvider<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IHostEnvironment _hostEnvironment;

        public FeatureDefaultProvider(
            IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));

            IsEnabledDefaults = [];

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
                    var isEnabledEnvironmentDefaultAttributes = featureFieldInfo
                        .GetCustomAttributes<FeatureIsEnabledEnvironmentDefaultAttribute>(false)
                        .Where(x => x.EnvironmentName.Equals(_hostEnvironment.EnvironmentName, StringComparison.OrdinalIgnoreCase));
                    if (isEnabledEnvironmentDefaultAttributes.Count() > 1)
                    {
                        throw new FeatureManagementException(FeatureManagementErrorType.MultipleIsEnabledEnvironmentDefaultAttributes, $"The feature '{featureName}' has multiple is enabled environment default attributes for the current environment.");
                    }

                    var isEnabledEnvironmentDefaultAttribute = isEnabledEnvironmentDefaultAttributes.SingleOrDefault();
                    if (isEnabledEnvironmentDefaultAttribute != null)
                    {
                        IsEnabledDefaults.Add(feature, isEnabledEnvironmentDefaultAttribute.IsEnabled);
                    }
                    else
                    {
                        var isEnabledDefaultAttribute = featureFieldInfo.GetCustomAttribute<FeatureIsEnabledDefaultAttribute>(false);
                        if (isEnabledDefaultAttribute != null)
                        {
                            IsEnabledDefaults.Add(feature, isEnabledDefaultAttribute.IsEnabled);
                        }
                    }
                }
            }
        }

        internal Dictionary<TFeature, bool> IsEnabledDefaults { get; }

        public bool TryGetIsEnabledDefault(TFeature feature, out bool isEnabledDefault)
        {
            return IsEnabledDefaults.TryGetValue(feature, out isEnabledDefault);
        }
    }
}
