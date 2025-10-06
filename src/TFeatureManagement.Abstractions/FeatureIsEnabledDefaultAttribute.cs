namespace TFeatureManagement;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FeatureIsEnabledDefaultAttribute : Attribute
{
    public FeatureIsEnabledDefaultAttribute(
        bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public bool IsEnabled { get; }
}
