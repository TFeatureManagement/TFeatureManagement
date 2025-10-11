namespace TFeatureManagement.Tests;

public enum FeatureDefaultProviderFeature
{
    NoDefault,
    [FeatureIsEnabledDefault(true)]
    Default,
    [FeatureIsEnabledDefault(false)]
    [FeatureIsEnabledDefault(FeatureDefaultsProviderTests.TestsEnvironmentName, true)]
    DefaultForCurrentEnvironment,
    [FeatureIsEnabledDefault(FeatureDefaultsProviderTests.TestsEnvironmentName, true)]
    DefaultForCurrentEnvironmentOnly,
    [FeatureIsEnabledDefault(FeatureDefaultsProviderTests.TestsEnvironmentName, true)]
    [FeatureIsEnabledDefault("Other", false)]
    DefaultForCurrentAndOtherEnvironment,
    [FeatureIsEnabledDefault("Other", true)]
    DefaultForOtherEnvironmentOnly
}
