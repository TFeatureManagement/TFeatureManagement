﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TFeatureManagement.AspNetCore.Mvc;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.AspNetCore.Builder;

/// <summary>
/// Extensions to <see cref="IFeatureManagementBuilder{TFeature}" /> for registering a disabled action handler.
/// </summary>
public static class UseDisabledActionHandlerFeatureManagementBuilderExtensions
{
    /// <summary>
    /// Registers a disabled action handler. This will be invoked when an MVC action is disabled because it requires a
    /// set of features to be enabled but the features are not enabled.
    /// </summary>
    /// <param name="builder">The feature management builder.</param>
    /// <param name="disabledActionHandler">The disabled action handler.</param>
    /// <returns>The feature management builder.</returns>
    public static IFeatureManagementBuilder<TFeature> UseDisabledActionHandler<TFeature>(this IFeatureManagementBuilder<TFeature> builder, IDisabledActionHandler<TFeature> disabledActionHandler)
        where TFeature : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(disabledActionHandler);

        builder.Services.AddSingleton(disabledActionHandler);

        return builder;
    }

    /// <summary>
    /// Registers an inline disabled action handler. This will be invoked when an MVC action is disabled because it
    /// requires a set of features to be enabled but the features are not enabled.
    /// </summary>
    /// <param name="builder">The feature management builder.</param>
    /// <param name="handler">The inline handler for disabled actions.</param>
    /// <returns>The feature management builder.</returns>
    [Obsolete("Use UseDisabledActionHandler overload that accepts a handler of type Action<IEnumerable<TFeature>, RequirementType, ActionExecutingContext> instead. This will be removed in an upcoming major release.")]
    public static IFeatureManagementBuilder<TFeature> UseDisabledActionHandler<TFeature>(this IFeatureManagementBuilder<TFeature> builder, Action<IEnumerable<TFeature>, ActionExecutingContext> handler)
        where TFeature : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(handler);

        builder.UseDisabledActionHandler(new InlineDisabledActionHandler<TFeature>(handler));

        return builder;
    }

    /// <summary>
    /// Registers an inline disabled action handler. This will be invoked when an MVC action is disabled because it
    /// requires a set of features to be enabled but the features are not enabled.
    /// </summary>
    /// <param name="builder">The feature management builder.</param>
    /// <param name="handler">The inline handler for disabled actions.</param>
    /// <returns>The feature management builder.</returns>
    public static IFeatureManagementBuilder<TFeature> UseDisabledActionHandler<TFeature>(this IFeatureManagementBuilder<TFeature> builder, Action<IEnumerable<TFeature>, RequirementType, ActionExecutingContext> handler)
        where TFeature : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(handler);

        builder.UseDisabledActionHandler(new InlineDisabledActionHandler<TFeature>(handler));

        return builder;
    }
}