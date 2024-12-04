using Microsoft.FeatureManagement;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.ApplicationInsights.DependencyInjection;

public static class FeatureManagementBuilderExtensions
{
    /// <summary>
    /// Adds application insights telemetry.
    /// </summary>
    /// <param name="builder">The feature management builder.</param>
    /// <returns>The feature management builder.</returns>
    public static IFeatureManagementBuilder<TFeature> AddApplicationInsightsTelemetry<TFeature>(this IFeatureManagementBuilder<TFeature> builder)
        where TFeature : struct, Enum
    {
        builder.BaseFeatureManagementBuilder.AddApplicationInsightsTelemetry();

        return builder;
    }
}
