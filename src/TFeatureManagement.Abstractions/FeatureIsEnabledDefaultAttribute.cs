namespace TFeatureManagement;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class FeatureIsEnabledDefaultAttribute : Attribute
{
    public FeatureIsEnabledDefaultAttribute(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public bool IsEnabled { get; }
}
