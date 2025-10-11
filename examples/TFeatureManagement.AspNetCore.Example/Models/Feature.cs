namespace TFeatureManagement.AspNetCore.Example.Models;

public enum Feature
{
    [FeatureIsEnabledDefault(true)]
    Example1,
    Example2,
    Example3,
    Example4
}