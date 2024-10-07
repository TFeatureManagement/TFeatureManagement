using FluentAssertions;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureManagerTests
{
    private FeatureManager<Feature> _underTest;

    private Mock<IFeatureManager> _baseFeatureManager;
    private Mock<IFeatureNameProvider<Feature>> _featureNameProvider;

    private readonly Func<Feature, string> _getFeatureName = (Feature feature) => feature.ToString();

    [TestInitialize]
    public void Setup()
    {
        _baseFeatureManager = new Mock<IFeatureManager>();
        _featureNameProvider = new Mock<IFeatureNameProvider<Feature>>();
        _featureNameProvider
            .Setup(x => x.GetFeatureName(It.IsAny<Feature>()))
            .Returns(_getFeatureName);

        _underTest = new FeatureManager<Feature>(
            _baseFeatureManager.Object,
            _featureNameProvider.Object);
    }

    [TestMethod]
    public void GetFeatureNamesAsync_CallsBaseGetFeatureNamesAsyncCorrectly()
    {
        _underTest.GetFeatureNamesAsync();

        _baseFeatureManager.Verify(x => x.GetFeatureNamesAsync(), Times.Once);
    }

    [TestMethod]
    public void GetFeatureNamesAsync_ReturnsBaseGetFeatureNamesAsyncResult()
    {
        var expectedFeatureNames = new Mock<IAsyncEnumerable<string>>();
        _baseFeatureManager.Setup(x => x.GetFeatureNamesAsync()).Returns(expectedFeatureNames.Object);

        var featureNames = _underTest.GetFeatureNamesAsync();

        featureNames.Should().Be(expectedFeatureNames.Object);
    }

    [TestMethod]
    public async Task IsEnabledAsync_CallsBaseIsEnabledAsyncCorrectly()
    {
        var expectedFeature = Feature.Test1;
        var expectedFeatureName = _getFeatureName(Feature.Test1);

        await _underTest.IsEnabledAsync(expectedFeature);

        _baseFeatureManager.Verify(x => x.IsEnabledAsync(expectedFeatureName), Times.Once);
    }

    [TestMethod]
    public async Task IsEnabledAsync_ReturnsBaseIsEnabledAsyncResult()
    {
        _baseFeatureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);

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

        _baseFeatureManager.Verify(x => x.IsEnabledAsync(expectedFeatureName, expectedContext), Times.Once);
    }

    [TestMethod]
    public async Task IsEnabledAsync_ReturnsBaseIsEnabledAsyncResult_WithContext()
    {
        var expectedFeature = Feature.Test1;
        var expectedContext = new object();
        _baseFeatureManager.Setup(x => x.IsEnabledAsync(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(true);

        var isEnabled = await _underTest.IsEnabledAsync(expectedFeature, expectedContext);

        isEnabled.Should().BeTrue();
    }
}