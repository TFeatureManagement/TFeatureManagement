using Microsoft.FeatureManagement;

namespace TFeatureManagement;

public class FeatureManagementOptions
{
    /// <summary>
    /// Gets or sets whether the <see cref="FeatureDefaultsFeatureDefinitionProviderDecorator{TFeature}"/> is enabled.
    /// </summary>
    /// <remarks>
    /// The <see cref="FeatureDefaultsFeatureDefinitionProviderDecorator{TFeature}"/> allows for specifying the
    /// defaults for a feature using attributes on the feature enum value. For example, the
    /// <see cref="FeatureIsEnabledDefaultAttribute"/> can be used to specify whether a feature is enabled by default
    /// if no <see cref="FeatureDefinition"/> exists for the feature.
    ///
    /// If the <see cref="FeatureDefaultsFeatureDefinitionProviderDecorator{TFeature}"/> is enabled, the decorator
    /// pattern is used to extend the functionality of the registered <see cref="IFeatureDefinitionProvider"/>.
    ///
    /// This functionality currently does not work if the <see cref="IFeatureDefinitionProvider"/> is registered as a
    /// keyed service.
    /// </remarks>
    public bool IsFeatureDefaultsFeatureDefinitionProviderDecoratorEnabled { get; set; }
}
