namespace TFeatureManagement;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class FeatureIsEnabledEnvironmentDefaultAttribute : Attribute
{
    public FeatureIsEnabledEnvironmentDefaultAttribute(
        string environmentName,
        bool isEnabled)
    {
        EnvironmentName = environmentName;
        IsEnabled = isEnabled;
    }

    public string EnvironmentName { get; }

    public bool IsEnabled { get; }
}
