﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TFeatureManagement.Metadata;
using TFeatureManagement.Tests.DataAnnotations;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureCleanupManagerTests
{
    private FeatureCleanupManager<Feature> _underTest;

    private Mock<IFeatureManager<Feature>> _featureManager;
    private Mock<IFeatureEnumConverter<Feature>> _featureEnumConverter;

    [TestInitialize]
    public void Setup()
    {
        _featureManager = new Mock<IFeatureManager<Feature>>();
        _featureEnumConverter = new Mock<IFeatureEnumConverter<Feature>>();
        _featureEnumConverter
            .Setup(x => x.GetFeatureName(It.IsAny<Feature>()))
            .Returns((Feature feature) => feature.ToString());

        _underTest = new FeatureCleanupManager<Feature>(
            _featureManager.Object,
            _featureEnumConverter.Object);
    }

    [TestMethod]
    public void GetFeatureCleanupDates_EnumHasValuesWithFeatureCleanupDate_ReturnsNonNullCleanupDateForValues()
    {
        // Arrange and Act
        var featureCleanupDates = _underTest.GetFeatureCleanupDates<TestFeatureCleanupDateAttribute>();

        // Assert
        featureCleanupDates.Should().ContainKey(Feature.Test1)
            .WhoseValue.CleanupDate.Should().NotBeNull();
    }

    [TestMethod]
    public void GetFeatureCleanupDates_EnumHasValuesWithoutFeatureCleanupDate_ReturnsNullCleanupDateForValues()
    {
        // Arrange and Act
        var featureCleanupDates = _underTest.GetFeatureCleanupDates<TestFeatureCleanupDateAttribute>();

        // Assert
        featureCleanupDates.Should().ContainKey(Feature.Test2)
            .WhoseValue.Should().BeNull();
    }

    [TestMethod]
    public async Task GetFeatureNamesNotInFeatureEnumAsync_FeatureNamesThatAreNotInFeatureEnumDoNotExist_ReturnsNoFeatureNames()
    {
        // Arrange
        _featureManager.Setup(x => x.GetFeatureNamesAsync(It.IsAny<CancellationToken>())).Returns(GetFeatureNamesAsync);

        // Act
        var featureNamesNotInFeatureEnum = new List<string>();
        await foreach (var featureName in _underTest.GetFeatureNamesNotInFeatureEnumAsync())
        {
            featureNamesNotInFeatureEnum.Add(featureName);
        }

        // Assert
        featureNamesNotInFeatureEnum.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFeatureNamesNotInFeatureEnumAsync_FeatureNamesThatAreNotInFeatureEnumExist_ReturnsFeatureNames()
    {
        // Arrange
        _featureManager.Setup(x => x.GetFeatureNamesAsync(It.IsAny<CancellationToken>())).Returns(GetFeatureNamesIncludingFeatureNamesNotInFeatureEnumAsync);

        // Act
        var featureNamesNotInFeatureEnum = new List<string>();
        await foreach (var featureName in _underTest.GetFeatureNamesNotInFeatureEnumAsync())
        {
            featureNamesNotInFeatureEnum.Add(featureName);
        }

        // Assert
        featureNamesNotInFeatureEnum.Should().Contain("Test3");
    }

    public static async IAsyncEnumerable<string> GetFeatureNamesAsync()
    {
        yield return nameof(Feature.Test2);

        await Task.CompletedTask.ConfigureAwait(false);
    }

    public static async IAsyncEnumerable<string> GetFeatureNamesIncludingFeatureNamesNotInFeatureEnumAsync()
    {
        yield return nameof(Feature.Test2);
        yield return "Test3";

        await Task.CompletedTask.ConfigureAwait(false);
    }
}