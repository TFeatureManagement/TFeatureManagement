using FluentAssertions;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureManagerSnapshotTests
{
    private FeatureManagerSnapshot<Feature> _underTest;

    private Mock<IFeatureManagerSnapshot> _baseFeatureManagerSnapshot;
    private Mock<IFeatureEnumConverter<Feature>> _featureEnumConverter;

    private readonly Func<Feature, string> _getFeatureName = (Feature feature) => feature.ToString();

    [TestInitialize]
    public void Setup()
    {
        _baseFeatureManagerSnapshot = new Mock<IFeatureManagerSnapshot>();
        _featureEnumConverter = new Mock<IFeatureEnumConverter<Feature>>();
        _featureEnumConverter
            .Setup(x => x.GetFeatureName(It.IsAny<Feature>()))
            .Returns(_getFeatureName);

        _underTest = new FeatureManagerSnapshot<Feature>(
            _baseFeatureManagerSnapshot.Object,
            _featureEnumConverter.Object);
    }

    [TestMethod]
    public void GetFeatureNamesAsync_CallsBaseGetFeatureNamesAsyncCorrectly()
    {
        _underTest.GetFeatureNamesAsync();

        _baseFeatureManagerSnapshot.Verify(x => x.GetFeatureNamesAsync(), Times.Once);
    }

    [TestMethod]
    public void GetFeatureNamesAsync_ReturnsBaseGetFeatureNamesAsyncResult()
    {
        var expectedFeatureNames = new Mock<IAsyncEnumerable<string>>();
        _baseFeatureManagerSnapshot.Setup(x => x.GetFeatureNamesAsync()).Returns(expectedFeatureNames.Object);

        var featureNames = _underTest.GetFeatureNamesAsync();

        featureNames.Should().Be(expectedFeatureNames.Object);
    }

    [TestMethod]
    public async Task IsEnabledAsync_CallsBaseIsEnabledAsyncCorrectly()
    {
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);

        await _underTest.IsEnabledAsync(expectedFeature);

        _baseFeatureManagerSnapshot.Verify(x => x.IsEnabledAsync(expectedFeatureName), Times.Once);
    }

    [TestMethod]
    public async Task IsEnabledAsync_ReturnsBaseIsEnabledAsyncResult()
    {
        _baseFeatureManagerSnapshot.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);

        var isEnabled = await _underTest.IsEnabledAsync(Feature.Test1);

        isEnabled.Should().BeTrue();
    }

    [TestMethod]
    public async Task IsEnabledAsync_CallsBaseIsEnabledAsyncCorrectly_WithContext()
    {
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);
        var expectedContext = new object();

        await _underTest.IsEnabledAsync(expectedFeature, expectedContext);

        _baseFeatureManagerSnapshot.Verify(x => x.IsEnabledAsync(expectedFeatureName, expectedContext), Times.Once);
    }

    [TestMethod]
    public async Task IsEnabledAsync_ReturnsBaseIsEnabledAsyncResult_WithContext()
    {
        var expectedFeature = Feature.Test1;
        var expectedContext = new object();
        _baseFeatureManagerSnapshot.Setup(x => x.IsEnabledAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

        var isEnabled = await _underTest.IsEnabledAsync(expectedFeature, expectedContext);

        isEnabled.Should().BeTrue();
    }
}