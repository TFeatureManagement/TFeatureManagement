using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Mvc.Filters;
using TFeatureManagement.AspNetCore.Mvc.Routing;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.AspNetCore.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds singleton <see cref="IFeatureManager{TFeature}"/> and required feature management services.
    /// </summary>
    /// <param name="services">The service collection that feature management services are added to.</param>
    /// <returns>
    /// A <see cref="IFeatureManagementBuilder{TFeature}" /> that can be used to customize feature management
    /// functionality.
    /// </returns>
    public static IFeatureManagementBuilder<TFeature> AddFeatureManagement<TFeature>(this IServiceCollection services)
        where TFeature : struct, Enum
    {
        var builder = TFeatureManagement.DependencyInjection.ServiceCollectionExtensions.AddFeatureManagement<TFeature>(services);
        builder.AddCoreServices();

        return builder;
    }

    /// <summary>
    /// Adds singleton <see cref="IFeatureManager{TFeature}"/> and required feature management services.
    /// </summary>
    /// <param name="services">The service collection that feature management services are added to.</param>
    /// <param name="configuration">
    /// A specific <see cref="IConfiguration" /> instance that will be used to obtain feature settings.
    /// </param>
    /// <returns>
    /// A <see cref="IFeatureManagementBuilder{TFeature}" /> that can be used to customize feature management
    /// functionality.
    /// </returns>
    public static IFeatureManagementBuilder<TFeature> AddFeatureManagement<TFeature>(this IServiceCollection services, IConfiguration configuration)
        where TFeature : struct, Enum
    {
        var builder = TFeatureManagement.DependencyInjection.ServiceCollectionExtensions.AddFeatureManagement<TFeature>(services, configuration);
        builder.AddCoreServices();

        return builder;
    }

    /// <summary>
    /// Adds scoped <see cref="IFeatureManager{TFeature}"/> and required feature management services.
    /// </summary>
    /// <param name="services">The service collection that feature management services are added to.</param>
    /// <returns>
    /// A <see cref="IFeatureManagementBuilder{TFeature}" /> that can be used to customize feature management
    /// functionality.
    /// </returns>
    public static IFeatureManagementBuilder<TFeature> AddScopedFeatureManagement<TFeature>(this IServiceCollection services)
        where TFeature : struct, Enum
    {
        var builder = TFeatureManagement.DependencyInjection.ServiceCollectionExtensions.AddScopedFeatureManagement<TFeature>(services);
        builder.AddCoreServices();

        return builder;
    }

    /// <summary>
    /// Adds scoped <see cref="IFeatureManager{TFeature}"/> and required feature management services.
    /// </summary>
    /// <param name="services">The service collection that feature management services are added to.</param>
    /// <param name="configuration">
    /// A specific <see cref="IConfiguration" /> instance that will be used to obtain feature settings.
    /// </param>
    /// <returns>
    /// A <see cref="IFeatureManagementBuilder{TFeature}" /> that can be used to customize feature management
    /// functionality.
    /// </returns>
    public static IFeatureManagementBuilder<TFeature> AddScopedFeatureManagement<TFeature>(this IServiceCollection services, IConfiguration configuration)
        where TFeature : struct, Enum
    {
        var builder = TFeatureManagement.DependencyInjection.ServiceCollectionExtensions.AddScopedFeatureManagement<TFeature>(services, configuration);
        builder.AddCoreServices();

        return builder;
    }

    private static IFeatureManagementBuilder<TFeature> AddCoreServices<TFeature>(this IFeatureManagementBuilder<TFeature> builder)
        where TFeature : struct, Enum
    {
        builder.Services.AddSingleton<IFeatureActionFilterFactory<TFeature>, FeatureActionFilterFactory<TFeature>>();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IFilterProvider, FeatureActionFilterProvider<TFeature>>());
        builder.Services.AddSingleton<IFeatureActionConstraintFactory<TFeature>, FeatureActionConstraintFactory<TFeature>>();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IActionConstraintProvider, FeatureActionConstraintProvider<TFeature>>());
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, FeatureActionConstraintMatcherPolicy<TFeature>>());

        return builder;
    }
}