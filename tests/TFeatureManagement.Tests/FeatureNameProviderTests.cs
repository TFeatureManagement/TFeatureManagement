using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureNameProviderTests
{
    private FeatureNameProvider<Feature> _underTest;

    [TestInitialize]
    public void Setup()
    {
        _underTest = new FeatureNameProvider<Feature>();
    }

    [TestMethod]
    public void Constructor_InitialisesDictionaryWithFeatureNames()
    {
        // Arrange and Act
        var underTest = new FeatureNameProvider<Feature>();

        // Assert
        underTest.FeatureNames
            .Should()
            .HaveCount(2)
            .And
            .Equal(
                new KeyValuePair<Feature, string>(Feature.Test1, Feature.Test1.ToString()),
                new KeyValuePair<Feature, string>(Feature.Test2, Feature.Test2.ToString())
            );
    }

    [TestMethod]
    public void GetFeatureName_FeatureInFeatureEnum_ReturnsFeatureNameFromDictionary()
    {
        // Arrange
        var feature = Feature.Test1;
        var expectedFeatureName = feature.ToString();

        // Act
        var featureName = _underTest.GetFeatureName(feature);

        // Assert
        featureName.Should().Be(expectedFeatureName);
    }

    [TestMethod]
    public void GetFeatureName_FeatureNotInFeatureEnum_ReturnsFeatureToString()
    {
        // Arrange
        var feature = (Feature)123;
        var expectedFeatureName = feature.ToString();

        // Act
        var featureName = _underTest.GetFeatureName(feature);

        // Assert
        featureName.Should().Be(expectedFeatureName);
    }
}
