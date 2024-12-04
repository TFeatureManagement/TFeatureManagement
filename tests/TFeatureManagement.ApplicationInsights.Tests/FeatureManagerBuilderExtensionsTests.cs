using Microsoft.FeatureManagement;
using NSubstitute;
using TFeatureManagement.ApplicationInsights.DependencyInjection;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.ApplicationInsights.Tests;

public class FeatureManagerBuilderExtensionsTests
{
    private readonly IFeatureManagementBuilder<Feature> _underTest;

    private readonly IFeatureManagementBuilder _baseFeatureManagementBuilder;

    public FeatureManagerBuilderExtensionsTests()
    {
        _baseFeatureManagementBuilder = Substitute.For<IFeatureManagementBuilder>();

        _underTest = Substitute.For<IFeatureManagementBuilder<Feature>>();
        _underTest.BaseFeatureManagementBuilder.Returns(_baseFeatureManagementBuilder);
    }

    [Fact]
    public void AddApplicationInsightsTelemetry_CallsBaseAddApplicationInsightsTelemetryCorrectly()
    {
        // Arrange and Act
        _underTest.AddApplicationInsightsTelemetry();

        // Assert
        _baseFeatureManagementBuilder.Received().AddApplicationInsightsTelemetry();
    }
}
