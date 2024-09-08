#if NET6_0_OR_GREATER

using Microsoft.AspNetCore.Builder;
using TFeatureManagement.AspNetCore.Routing;

namespace TFeatureManagement.AspNetCore.Builder;

public static class RequireFeaturesForRoutingEndpointConventionBuilderExtension
{
    /// <summary>
    /// Requires that a feature be enabled for the endpoint to be matched during routing.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="feature">The feature that should be enabled.</param>
    /// <returns>The <see cref="IEndpointConventionBuilder"/>.</returns>
    public static TBuilder RequireFeatureForRouting<TBuilder, TFeature>(this TBuilder builder, TFeature feature)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        return builder.RequireFeatureForRouting(RequirementType.All, feature);
    }

    /// <summary>
    /// Requires that a feature be enabled for the endpoint to be matched during routing.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="feature">The features that should be enabled.</param>
    /// <returns>The <see cref="IEndpointConventionBuilder"/>.</returns>
    public static TBuilder RequireFeatureForRouting<TBuilder, TFeature>(this TBuilder builder, RequirementType requirementType, TFeature feature)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        return builder.RequireFeaturesForRouting(requirementType, new[] { feature });
    }

    /// <summary>
    /// Requires that a set of features be enabled for the endpoint to be matched during routing.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="features">The features that should be enabled.</param>
    /// <returns>The <see cref="IEndpointConventionBuilder"/>.</returns>
    public static TBuilder RequireFeaturesForRouting<TBuilder, TFeature>(this TBuilder builder, params TFeature[] features)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        return builder.RequireFeaturesForRouting(RequirementType.All, features);
    }

    /// <summary>
    /// Requires that a set of features be enabled for the endpoint to be matched during routing.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="features">The features that should be enabled.</param>
    /// <returns>The <see cref="IEndpointConventionBuilder"/>.</returns>
    public static TBuilder RequireFeaturesForRouting<TBuilder, TFeature>(this TBuilder builder, RequirementType requirementType, params TFeature[] features)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        builder.Add(endpointBuilder =>
        {
            endpointBuilder.Metadata.Add(new FeatureAttribute<TFeature>(requirementType, features));
        });

        return builder;
    }
}

#endif