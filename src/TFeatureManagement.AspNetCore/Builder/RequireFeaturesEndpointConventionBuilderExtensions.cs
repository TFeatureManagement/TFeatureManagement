#if NET7_0_OR_GREATER

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using TFeatureManagement.AspNetCore.Http;

namespace TFeatureManagement.AspNetCore.Builder;

/// <summary>
/// Extensions to <see cref="IEndpointConventionBuilder" /> that require a set of features to be enabled for the endpoint to be enabled.
/// </summary>
public static class RequiresFeaturesEndpointConventionBuilderExtensions
{
    /// <summary>
    /// Adds a <see cref="FeatureEndpointFilter{TFeature}"/> to the endpoint(s).
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="feature">The feature that should be enabled.</param>
    /// <returns></returns>
    public static TBuilder RequireFeature<TBuilder, TFeature>(this TBuilder builder, TFeature feature)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        return builder.RequireFeature(RequirementType.All, feature);
    }

    /// <summary>
    /// Adds a <see cref="FeatureEndpointFilter{TFeature}"/> to the endpoint(s).
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="feature">The features that should be enabled.</param>
    /// <returns></returns>
    public static TBuilder RequireFeature<TBuilder, TFeature>(this TBuilder builder, RequirementType requirementType, TFeature feature)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        return builder.RequireFeatures(requirementType, new[] { feature });
    }

    /// <summary>
    /// Adds a <see cref="FeatureEndpointFilter{TFeature}"/> to the endpoint(s).
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="features">The features that should be enabled.</param>
    /// <returns></returns>
    public static TBuilder RequireFeatures<TBuilder, TFeature>(this TBuilder builder, params TFeature[] features)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        return builder.RequireFeatures(RequirementType.All, features);
    }

    /// <summary>
    /// Adds a <see cref="FeatureEndpointFilter{TFeature}"/> to the endpoint(s).
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="features">The features that should be enabled.</param>
    /// <returns></returns>
    public static TBuilder RequireFeatures<TBuilder, TFeature>(this TBuilder builder, RequirementType requirementType, params TFeature[] features)
        where TBuilder : IEndpointConventionBuilder
        where TFeature : struct, Enum
    {
        builder.AddEndpointFilter(new FeatureEndpointFilter<TFeature>(features, requirementType));

        return builder;
    }
}

#endif