namespace TFeatureManagement.Tests;

public enum FeatureDefaultsProviderFeatureInvalidAttributes
{
    [FeatureIsEnabledDefault(FeatureDefaultsProviderTests.TestsEnvironmentName, true)]
    [FeatureIsEnabledDefault(FeatureDefaultsProviderTests.TestsEnvironmentName, true)]
    MultipleDefaultsForEnvironment
}
