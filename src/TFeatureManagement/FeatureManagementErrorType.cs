namespace TFeatureManagement;

public enum FeatureManagementErrorType
{
    /// <summary>
    /// The feature being evaluated has multiple <see cref="FeatureIsEnabledEnvironmentDefaultAttribute"/>
    /// for the current environment.
    /// </summary>
    MultipleIsEnabledEnvironmentDefaultAttributes
}
