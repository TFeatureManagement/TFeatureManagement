using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.Tests
{
    [TestClass]
    public class FeatureManagementBuilderTests
    {
        private FeatureManagementBuilder<Feature> _underTest;

        private Mock<IFeatureManagementBuilder> _baseFeatureManagementBuilder;

        [TestInitialize]
        public void Setup()
        {
            _baseFeatureManagementBuilder = new Mock<IFeatureManagementBuilder>();

            _underTest = new FeatureManagementBuilder<Feature>(_baseFeatureManagementBuilder.Object);
        }

        [TestMethod]
        public void AddFeatureFilter_CallsBaseAddFeatureFilterCorrectly()
        {
            // Arrange and Act
            _underTest.AddFeatureFilter<TestFeatureFilterMetadata>();

            // Assert
            _baseFeatureManagementBuilder.Verify(x => x.AddFeatureFilter<TestFeatureFilterMetadata>(), Times.Once);
        }
    }
}