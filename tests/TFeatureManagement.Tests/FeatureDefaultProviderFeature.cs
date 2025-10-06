namespace TFeatureManagement.Tests;

public enum FeatureDefaultProviderFeature
{
    NoDefault,
    [FeatureIsEnabledDefault(true)]
    Default,
    [FeatureIsEnabledDefault(false)]
    [FeatureIsEnabledEnvironmentDefault(FeatureDefaultProviderTests.TestsEnvironmentName, true)]
    DefaultForCurrentEnvironment,
    [FeatureIsEnabledEnvironmentDefault(FeatureDefaultProviderTests.TestsEnvironmentName, true)]
    DefaultForCurrentEnvironmentOnly,
    [FeatureIsEnabledEnvironmentDefault(FeatureDefaultProviderTests.TestsEnvironmentName, true)]
    [FeatureIsEnabledEnvironmentDefault("Other", false)]
    DefaultForCurrentAndOtherEnvironment,
    [FeatureIsEnabledEnvironmentDefault("Other", true)]
    DefaultForOtherEnvironmentOnly
}
