using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureVariantManagerSnapshotTests
{
    private FeatureVariantManagerSnapshot<Feature> _underTest;

    private IVariantFeatureManagerSnapshot _baseFeatureVariantManagerSnapshot;
    private IFeatureNameProvider<Feature> _featureNameProvider;

    private readonly Func<Feature, string> _getFeatureName = (Feature feature) => feature.ToString();

    [TestInitialize]
    public void Setup()
    {
        _baseFeatureVariantManagerSnapshot = Substitute.For<IVariantFeatureManagerSnapshot>();
        _featureNameProvider = Substitute.For<IFeatureNameProvider<Feature>>();
        _featureNameProvider.GetFeatureName(Arg.Any<Feature>()).Returns(x => _getFeatureName((Feature)x[0]));

        _underTest = new FeatureVariantManagerSnapshot<Feature>(
            _baseFeatureVariantManagerSnapshot,
            _featureNameProvider);
    }

    [TestMethod]
    public async Task GetVariantAsync_CallsBaseGetVariantAsyncCorrectly_CalledWithoutTargetingContext()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);
        var expectedCancellationToken = new CancellationToken(true);
        _baseFeatureVariantManagerSnapshot.GetVariantAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(default(Variant));

        // Act
        await _underTest.GetVariantAsync(expectedFeature, expectedCancellationToken);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CA2012 // Use ValueTasks correctly
        _baseFeatureVariantManagerSnapshot.Received()
            .GetVariantAsync(
                expectedFeatureName,
                expectedCancellationToken);
#pragma warning restore CA2012 // Use ValueTasks correctly
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    public async Task GetVariantAsync_ReturnsNullResult_CalledWithoutTargetingContextAndBaseResultIsNull()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedCancellationToken = new CancellationToken(true);
        _baseFeatureVariantManagerSnapshot.GetVariantAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(default(Variant));

        // Act
        var featureVariant = await _underTest.GetVariantAsync(expectedFeature, expectedCancellationToken);

        // Assert
        featureVariant.Should().BeNull();
    }

    [TestMethod]
    public async Task GetVariantAsync_ReturnsBaseGetVariantAsyncResult_CalledWithoutTargetingContextAndBaseResultIsNotNull()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedCancellationToken = new CancellationToken(true);
        var expectedFeatureVariantName = Guid.NewGuid().ToString();
        var expectedConfigurationSection = new ConfigurationSection(new ConfigurationRoot([]), "test");
        var expectedBaseFeatureVariant = new Variant
        {
            Name = expectedFeatureVariantName,
            Configuration = expectedConfigurationSection
        };
        _baseFeatureVariantManagerSnapshot.GetVariantAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(expectedBaseFeatureVariant);

        // Act
        var featureVariant = await _underTest.GetVariantAsync(expectedFeature, expectedCancellationToken);

        // Assert
        featureVariant.Should().NotBeNull();
        featureVariant.Name.Should().Be(expectedFeatureVariantName);
        featureVariant.Configuration.Should().Be(expectedConfigurationSection);
    }

    [TestMethod]
    public async Task GetVariantAsync_CallsBaseGetVariantAsyncCorrectly_CalledWithTargetingContext()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);
        var expectedContext = Substitute.For<ITargetingContext>();
        expectedContext.UserId.Returns(Guid.NewGuid().ToString());
        expectedContext.Groups.Returns([]);
        var expectedCancellationToken = new CancellationToken(true);

        // Act
        await _underTest.GetVariantAsync(expectedFeature, expectedContext, expectedCancellationToken);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CA2012 // Use ValueTasks correctly
        _baseFeatureVariantManagerSnapshot.Received()
            .GetVariantAsync(
                expectedFeatureName,
                Arg.Is<Microsoft.FeatureManagement.FeatureFilters.ITargetingContext>(c => c.UserId.Equals(expectedContext.UserId) && c.Groups.Equals(expectedContext.Groups)),
                expectedCancellationToken);
#pragma warning restore CA2012 // Use ValueTasks correctly
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    public async Task GetVariantAsync_ReturnsNullResult_CalledWithTargetingContextAndBaseResultIsNull()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedContext = Substitute.For<ITargetingContext>();
        var expectedCancellationToken = new CancellationToken(true);
        _baseFeatureVariantManagerSnapshot.GetVariantAsync(Arg.Any<string>(), Arg.Any<Microsoft.FeatureManagement.FeatureFilters.ITargetingContext>(), Arg.Any<CancellationToken>()).Returns(default(Variant));

        // Act
        var featureVariant = await _underTest.GetVariantAsync(expectedFeature, expectedContext, expectedCancellationToken);

        // Assert
        featureVariant.Should().BeNull();
    }

    [TestMethod]
    public async Task GetVariantAsync_ReturnsBaseGetVariantAsyncResult_CalledWithTargetingContextAndBaseResultIsNotNull()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedContext = Substitute.For<ITargetingContext>();
        var expectedCancellationToken = new CancellationToken(true);
        var expectedFeatureVariantName = Guid.NewGuid().ToString();
        var expectedConfigurationSection = new ConfigurationSection(new ConfigurationRoot([]), "test");
        var expectedBaseFeatureVariant = new Variant
        {
            Name = expectedFeatureVariantName,
            Configuration = expectedConfigurationSection
        };
        _baseFeatureVariantManagerSnapshot.GetVariantAsync(Arg.Any<string>(), Arg.Any<Microsoft.FeatureManagement.FeatureFilters.ITargetingContext>(), Arg.Any<CancellationToken>()).Returns(expectedBaseFeatureVariant);

        // Act
        var featureVariant = await _underTest.GetVariantAsync(expectedFeature, expectedContext, expectedCancellationToken);

        // Assert
        featureVariant.Should().NotBeNull();
        featureVariant.Name.Should().Be(expectedFeatureVariantName);
        featureVariant.Configuration.Should().Be(expectedConfigurationSection);
    }
}