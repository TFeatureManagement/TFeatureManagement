using FluentAssertions;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureManagerTests
{
    private FeatureManager<Feature> _underTest;

    private IVariantFeatureManager _baseFeatureManager;
    private IFeatureNameProvider<Feature> _featureNameProvider;

    private readonly Func<Feature, string> _getFeatureName = (Feature feature) => feature.ToString();

    [TestInitialize]
    public void Setup()
    {
        _baseFeatureManager = Substitute.For<IVariantFeatureManager>();
        _featureNameProvider = Substitute.For<IFeatureNameProvider<Feature>>();
        _featureNameProvider.GetFeatureName(Arg.Any<Feature>()).Returns(x => _getFeatureName((Feature)x[0]));

        _underTest = new FeatureManager<Feature>(
            _baseFeatureManager,
            _featureNameProvider);
    }

    [TestMethod]
    public void GetFeatureNamesAsync_CallsBaseGetFeatureNamesAsyncCorrectly()
    {
        // Arrange and Act
        _underTest.GetFeatureNamesAsync();

        // Assert
        _baseFeatureManager.Received().GetFeatureNamesAsync();
    }

    [TestMethod]
    public void GetFeatureNamesAsync_ReturnsBaseGetFeatureNamesAsyncResult()
    {
        // Arrange
        var expectedFeatureNames = Substitute.For<IAsyncEnumerable<string>>();
        _baseFeatureManager.GetFeatureNamesAsync().Returns(expectedFeatureNames);

        // Act
        var featureNames = _underTest.GetFeatureNamesAsync();

        // Assert
        featureNames.Should().Be(expectedFeatureNames);
    }

    [TestMethod]
    [DynamicData(nameof(TargetingContextOptions))]
    public async Task IsEnabledAsync_CallsBaseIsEnabledAsyncCorrectly(FeatureTargetingContextOption targetingContextOption)
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);
        var expectedContext = targetingContextOption == FeatureTargetingContextOption.WithTargetingContext ? new object() : null;
        var expectedCancellationToken = new CancellationToken(true);

        // Act
        if (targetingContextOption == FeatureTargetingContextOption.WithTargetingContext)
        {
            await _underTest.IsEnabledAsync(expectedFeature, expectedContext, expectedCancellationToken);
        }
        else
        {
            await _underTest.IsEnabledAsync(expectedFeature, expectedCancellationToken);
        }

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CA2012 // Use ValueTasks correctly
        if (targetingContextOption == FeatureTargetingContextOption.WithTargetingContext)
        {
            _baseFeatureManager
                .Received()
                .IsEnabledAsync(expectedFeatureName, expectedContext, expectedCancellationToken);
        }
        else
        {
            _baseFeatureManager
                .Received()
                .IsEnabledAsync(expectedFeatureName, expectedCancellationToken);
        }
#pragma warning restore CA2012 // Use ValueTasks correctly
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    [DynamicData(nameof(TargetingContextOptions))]
    public async Task IsEnabledAsync_ReturnsBaseIsEnabledAsyncResult(FeatureTargetingContextOption targetingContextOption)
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedContext = targetingContextOption == FeatureTargetingContextOption.WithTargetingContext ? new object() : null;
        var expectedCancellationToken = new CancellationToken(true);

        if (targetingContextOption == FeatureTargetingContextOption.WithTargetingContext)
        {
            _baseFeatureManager
                .IsEnabledAsync(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<CancellationToken>())
                .Returns(true);
        }
        else
        {
            _baseFeatureManager
                .IsEnabledAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(true);
        }

        // Act
        var isEnabled = targetingContextOption == FeatureTargetingContextOption.WithTargetingContext ?
            await _underTest.IsEnabledAsync(expectedFeature, expectedContext, expectedCancellationToken) :
            await _underTest.IsEnabledAsync(expectedFeature, expectedCancellationToken);

        // Assert
        isEnabled.Should().BeTrue();
    }

    public static IEnumerable<object[]> TargetingContextOptions
    {
        get
        {
            return
            [
                [FeatureTargetingContextOption.WithTargetingContext],
                [FeatureTargetingContextOption.WithoutTargetingContext]
            ];
        }
    }
}