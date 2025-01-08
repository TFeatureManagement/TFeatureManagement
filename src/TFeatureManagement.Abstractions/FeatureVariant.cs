using Microsoft.Extensions.Configuration;

namespace TFeatureManagement;

/// <summary>
/// A variant for a feature.
/// </summary>
public class FeatureVariant
{
    public FeatureVariant(string name, IConfigurationSection configuration)
    {
        Name = name;
        Configuration = configuration;
    }

    /// <summary>
    /// The name of the feature variant.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The configuration of the feature variant.
    /// </summary>
    public IConfigurationSection Configuration { get; set; }
}
