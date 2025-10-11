namespace TFeatureManagement;

/// <summary>
/// Allows for specifying whether a feature is enabled by default.
/// </summary>
/// <remarks>
/// If an environment name is not specified then the instance will specify whether the feature is enabled by default
/// for all environments except if there is an attribute instance for specific environment names.
/// 
/// This attribute can be used multiple times to allow for specifying whether a feature is enabled by default for all
/// environments as well as per environment. However, it should only be used once for each environment name otherwise
/// an exception will be thrown at runtime.
/// </remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class FeatureIsEnabledDefaultAttribute : Attribute
{
    public FeatureIsEnabledDefaultAttribute(
        bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public FeatureIsEnabledDefaultAttribute(
        string environmentName,
        bool isEnabled)
        : this(isEnabled)
    {
        EnvironmentName = environmentName;
        IsEnabled = isEnabled;
    }

    /// <summary>
    /// Gets the environment name.
    /// </summary>
    public string? EnvironmentName { get; }

    /// <summary>
    /// Gets whether the feature is enabled by default.
    /// </summary>
    public bool IsEnabled { get; }
}
