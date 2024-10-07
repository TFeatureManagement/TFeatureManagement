using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;

namespace TFeatureManagement.DependencyInjection;

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
        var builder = new FeatureManagementBuilder<TFeature>(services.AddFeatureManagement());
        builder.Services.TryAddSingleton<IFeatureManager<TFeature>, FeatureManager<TFeature>>();
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
        var builder = new FeatureManagementBuilder<TFeature>(services.AddFeatureManagement(configuration));
        builder.Services.TryAddSingleton<IFeatureManager<TFeature>, FeatureManager<TFeature>>();
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
        var builder = new FeatureManagementBuilder<TFeature>(services.AddScopedFeatureManagement());
        builder.Services.TryAddScoped<IFeatureManager<TFeature>, FeatureManager<TFeature>>();
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
        var builder = new FeatureManagementBuilder<TFeature>(services.AddScopedFeatureManagement(configuration));
        builder.Services.TryAddScoped<IFeatureManager<TFeature>, FeatureManager<TFeature>>();
        builder.AddCoreServices();

        return builder;
    }

    private static IFeatureManagementBuilder<TFeature> AddCoreServices<TFeature>(this IFeatureManagementBuilder<TFeature> builder)
        where TFeature : struct, Enum
    {
        builder.Services.TryAddScoped<IFeatureManagerSnapshot<TFeature>, FeatureManagerSnapshot<TFeature>>();

        builder.Services.TryAddSingleton<IFeatureNameProvider<TFeature>, FeatureNameProvider<TFeature>>();
        builder.Services.TryAddSingleton<IFeatureEnumParser<TFeature>, FeatureEnumParser<TFeature>>();

        return builder;
    }
}