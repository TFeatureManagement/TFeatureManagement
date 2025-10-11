using Microsoft.FeatureManagement;

namespace TFeatureManagement
{
    public class FeatureDefaultsFeatureDefinitionProviderDecorator<TFeature> : IFeatureDefinitionProvider
        where TFeature : struct, Enum
    {
        private readonly IFeatureDefinitionProvider _featureDefinitionProvider;
        private readonly IFeatureDefaultsProvider<TFeature> _featureDefaultsProvider;

        public FeatureDefaultsFeatureDefinitionProviderDecorator(
            IFeatureDefinitionProvider featureDefinitionProvider,
            IFeatureDefaultsProvider<TFeature> featureDefaultsProvider)
        {
            _featureDefinitionProvider = featureDefinitionProvider ?? throw new ArgumentNullException(nameof(featureDefinitionProvider));
            _featureDefaultsProvider = featureDefaultsProvider ?? throw new ArgumentNullException(nameof(featureDefaultsProvider));
        }

        public async Task<FeatureDefinition?> GetFeatureDefinitionAsync(string featureName)
        {
            var featureDefinition = await _featureDefinitionProvider.GetFeatureDefinitionAsync(featureName);
            if (featureDefinition == null &&
                _featureDefaultsProvider.IsEnabledDefaultsByFeatureName.TryGetValue(featureName, out bool isEnabledDefault))
            {
                return GetFeatureDefinition(featureName, isEnabledDefault);
            }

            return featureDefinition;
        }

        public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            var featureDefinitions = new List<FeatureDefinition>();

            await foreach (var featureDefinition in _featureDefinitionProvider.GetAllFeatureDefinitionsAsync())
            {
                featureDefinitions.Add(featureDefinition);
            }

            foreach (var isEnabledDefault in _featureDefaultsProvider.IsEnabledDefaultsByFeatureName)
            {
                if (!featureDefinitions.Any(x => x.Name.Equals(isEnabledDefault.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    featureDefinitions.Add(GetFeatureDefinition(isEnabledDefault.Key, isEnabledDefault.Value));
                }
            }

            foreach (var featureDefinition in featureDefinitions)
            {
                yield return featureDefinition;
            }
        }

        private static FeatureDefinition GetFeatureDefinition(string featureName, bool isEnabledDefault)
        {
            var featureDefinition = new FeatureDefinition
            {
                Name = featureName,
                Status = FeatureStatus.Conditional
            };

            if (isEnabledDefault)
            {
                featureDefinition.EnabledFor =
                [
                    new()
                    {
                        Name = "AlwaysOn"
                    }
                ];
            }

            return featureDefinition;
        }
    }
}
