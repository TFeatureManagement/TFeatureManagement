namespace TFeatureManagement;

public enum FeatureManagementErrorType
{
    /// <summary>
    /// A feature has multiple <see cref="FeatureIsEnabledDefaultAttribute"/> instances specified for an
    /// environment(s).
    /// </summary>
    MultipleIsEnabledDefaultAttributes
}
