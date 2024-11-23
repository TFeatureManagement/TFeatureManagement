using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.Tests;

[TestClass]
public class FeatureManagementBuilderTests
{
    private FeatureManagementBuilder<Feature> _underTest;

    private IFeatureManagementBuilder _baseFeatureManagementBuilder;

    [TestInitialize]
    public void Setup()
    {
        _baseFeatureManagementBuilder = Substitute.For<IFeatureManagementBuilder>();

        _underTest = new FeatureManagementBuilder<Feature>(_baseFeatureManagementBuilder);
    }

    [TestMethod]
    public void AddFeatureFilter_CallsBaseAddFeatureFilterCorrectly()
    {
        // Arrange and Act
        _underTest.AddFeatureFilter<TestFeatureFilterMetadata>();

        // Assert
        _baseFeatureManagementBuilder.Received().AddFeatureFilter<TestFeatureFilterMetadata>();
    }
}