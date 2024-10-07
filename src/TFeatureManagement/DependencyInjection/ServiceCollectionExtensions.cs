using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;

namespace TFeatureManagement.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds required feature management services.
    /// </summary>
    /// <param name="services">The service collection that feature management services are added to.</param>
    /// <returns>
    /// A <see cref="IFeatureManagementBuilder{TFeature}" /> that can be used to customize feature management
    /// functionality.
    /// </returns>
    public static IFeatureManagementBuilder<TFeature> AddFeatureManagement<TFeature>(this IServiceCollection services)
        where TFeature : struct, Enum
    {
        var featureManagementBuilder = new FeatureManagementBuilder<TFeature>(services.AddFeatureManagement());
        featureManagementBuilder.AddCoreServices();

        return featureManagementBuilder;
    }

    /// <summary>
    /// Adds required feature management services.
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
        var featureManagementBuilder = new FeatureManagementBuilder<TFeature>(services.AddFeatureManagement(configuration));
        featureManagementBuilder.AddCoreServices();

        return featureManagementBuilder;
    }

    private static IFeatureManagementBuilder<TFeature> AddCoreServices<TFeature>(this IFeatureManagementBuilder<TFeature> featureManagementBuilder)
        where TFeature : struct, Enum
    {
        featureManagementBuilder.Services.AddSingleton<IFeatureManager<TFeature>, FeatureManager<TFeature>>();
        featureManagementBuilder.Services.AddScoped<IFeatureManagerSnapshot<TFeature>, FeatureManagerSnapshot<TFeature>>();

        featureManagementBuilder.Services.TryAddSingleton<IFeatureNameProvider<TFeature>, FeatureNameProvider<TFeature>>();
        featureManagementBuilder.Services.TryAddSingleton<IFeatureEnumParser<TFeature>, FeatureEnumParser<TFeature>>();

        return featureManagementBuilder;
    }
}