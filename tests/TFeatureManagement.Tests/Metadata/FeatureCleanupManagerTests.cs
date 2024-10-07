using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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

    private IFeatureManager<Feature> _featureManager;
    private IFeatureNameProvider<Feature> _featureNameProvider;

    [TestInitialize]
    public void Setup()
    {
        _featureManager = Substitute.For<IFeatureManager<Feature>>();
        _featureNameProvider = Substitute.For<IFeatureNameProvider<Feature>>();
        _featureNameProvider.GetFeatureName(Arg.Any<Feature>()).Returns(x => x[0].ToString());

        _underTest = new FeatureCleanupManager<Feature>(
            _featureManager,
            _featureNameProvider);
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
        _featureManager.GetFeatureNamesAsync(Arg.Any<CancellationToken>()).Returns(GetFeatureNamesAsync());

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
        _featureManager.GetFeatureNamesAsync(Arg.Any<CancellationToken>()).Returns(GetFeatureNamesIncludingFeatureNamesNotInFeatureEnumAsync());

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