using FluentAssertions;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureManagerSnapshotTests
{
    private FeatureManagerSnapshot<Feature> _underTest;

    private IFeatureManagerSnapshot _baseFeatureManagerSnapshot;
    private IFeatureNameProvider<Feature> _featureNameProvider;

    private readonly Func<Feature, string> _getFeatureName = (Feature feature) => feature.ToString();

    [TestInitialize]
    public void Setup()
    {
        _baseFeatureManagerSnapshot = Substitute.For<IFeatureManagerSnapshot>();
        _featureNameProvider = Substitute.For<IFeatureNameProvider<Feature>>();
        _featureNameProvider.GetFeatureName(Arg.Any<Feature>()).Returns(x => _getFeatureName((Feature)x[0]));

        _underTest = new FeatureManagerSnapshot<Feature>(
            _baseFeatureManagerSnapshot,
            _featureNameProvider);
    }

    [TestMethod]
    public void GetFeatureNamesAsync_CallsBaseGetFeatureNamesAsyncCorrectly()
    {
        // Arrange and Act
        _underTest.GetFeatureNamesAsync();

        // Assert
        _baseFeatureManagerSnapshot.Received().GetFeatureNamesAsync();
    }

    [TestMethod]
    public void GetFeatureNamesAsync_ReturnsBaseGetFeatureNamesAsyncResult()
    {
        // Arrange
        var expectedFeatureNames = Substitute.For<IAsyncEnumerable<string>>();
        _baseFeatureManagerSnapshot.GetFeatureNamesAsync().Returns(expectedFeatureNames);

        // Act
        var featureNames = _underTest.GetFeatureNamesAsync();

        // Assert
        featureNames.Should().Be(expectedFeatureNames);
    }

    [TestMethod]
    public async Task IsEnabledAsync_CallsBaseIsEnabledAsyncCorrectly()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);

        // Act
        await _underTest.IsEnabledAsync(expectedFeature);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _baseFeatureManagerSnapshot.Received().IsEnabledAsync(expectedFeatureName);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    public async Task IsEnabledAsync_ReturnsBaseIsEnabledAsyncResult()
    {
        // Arrange
        _baseFeatureManagerSnapshot.IsEnabledAsync(Arg.Any<string>()).Returns(true);

        // Act
        var isEnabled = await _underTest.IsEnabledAsync(Feature.Test1);

        // Assert
        isEnabled.Should().BeTrue();
    }

    [TestMethod]
    public async Task IsEnabledAsync_CallsBaseIsEnabledAsyncCorrectly_WithContext()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);
        var expectedContext = new object();

        // Act
        await _underTest.IsEnabledAsync(expectedFeature, expectedContext);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _baseFeatureManagerSnapshot.Received().IsEnabledAsync(expectedFeatureName, expectedContext);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    public async Task IsEnabledAsync_ReturnsBaseIsEnabledAsyncResult_WithContext()
    {
        // Arrange
        var expectedFeature = Feature.Test1;
        var expectedContext = new object();
        _baseFeatureManagerSnapshot.IsEnabledAsync(Arg.Any<string>(), Arg.Any<object>()).Returns(true);

        // Act
        var isEnabled = await _underTest.IsEnabledAsync(expectedFeature, expectedContext);

        // Assert
        isEnabled.Should().BeTrue();
    }
}