#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TFeatureManagement.AspNetCore.Http;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.AspNetCore.Builder;

/// <summary>
/// Extensions to <see cref="IFeatureManagementBuilder{TFeature}" /> for registering a disabled endpoint handler.
/// </summary>
public static class UseDisabledEndpointHandlerFeatureManagementBuilderExtensions
{
    /// <summary>
    /// Registers a disabled endpoint handler. This will be invoked when an endpoint is disabled because it requires a
    /// set of features to be enabled but the features are not enabled.
    /// </summary>
    /// <param name="builder">The feature management builder.</param>
    /// <param name="disabledEndpointHandler">The disabled endpoint handler.</param>
    /// <returns>The feature management builder.</returns>
    public static IFeatureManagementBuilder<TFeature> UseDisabledEndpointHandler<TFeature>(this IFeatureManagementBuilder<TFeature> builder, IDisabledEndpointHandler<TFeature> disabledEndpointHandler)
        where TFeature : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(disabledEndpointHandler);

        builder.Services.AddSingleton(disabledEndpointHandler);

        return builder;
    }

    /// <summary>
    /// Registers an inline disabled endpoint handler. This will be invoked when an endpoint is disabled because it
    /// requires a set of features to be enabled but the features are not enabled.
    /// </summary>
    /// <param name="builder">The feature management builder.</param>
    /// <param name="handler">The inline handler for disabled endpoints.</param>
    /// <returns>The feature management builder.</returns>
    public static IFeatureManagementBuilder<TFeature> UseDisabledEndpointHandler<TFeature>(this IFeatureManagementBuilder<TFeature> builder, Action<IEnumerable<TFeature>, RequirementType, EndpointFilterInvocationContext> handler)
        where TFeature : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(handler);

        builder.UseDisabledEndpointHandler(new InlineDisabledEndpointHandler<TFeature>(handler));

        return builder;
    }
}

#endif