namespace TFeatureManagement.Tests;

public enum FeatureDefaultProviderFeatureInvalidAttributes
{
    [FeatureIsEnabledEnvironmentDefault(FeatureDefaultProviderTests.TestsEnvironmentName, true)]
    [FeatureIsEnabledEnvironmentDefault(FeatureDefaultProviderTests.TestsEnvironmentName, true)]
    MultipleDefaultsForEnvironment
}
