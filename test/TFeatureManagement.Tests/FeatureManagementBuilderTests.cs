using FluentAssertions;
using Microsoft.FeatureManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
        public void AddFeatureFilter_FeatureFilterImplementsMultipleFeatureFilterInterfaces_ThrowsArgumentException()
        {
            // Arrange and Act
            Action action = () => _underTest.AddFeatureFilter<FeatureFilterWithMultipleFeatureFilterInterfaces>();

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .Where(ex => ex.Message.StartsWith("A single feature filter cannot implement more than one feature filter interface.") &&
                             ex.ParamName.Equals("T"));
        }

        [TestMethod]
        public void AddFeatureFilter_FeatureFilterWithFeatureEnumTypeThatDoesNotMatchBuilderFeatureEnumType_ThrowsArgumentException()
        {
            // Arrange and Act
            Action action = () => _underTest.AddFeatureFilter<FeatureFilterForOtherFeature>();

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .Where(ex => ex.Message.StartsWith("The feature enum type specified by the feature filter must match the feature enum type of the feature management builder.") &&
                             ex.ParamName.Equals("T"));
        }

        [TestMethod]
        public void AddFeatureFilter_FeatureFilterWithFeatureEnumTypeThatDoesMatchBuilderFeatureEnumType_CallsBaseAddFeatureFilter()
        {
            // Arrange and Act
            _underTest.AddFeatureFilter<FeatureFilter>();

            // Assert
            _baseFeatureManagementBuilder.Verify(x => x.AddFeatureFilter<FeatureFilter>(), Times.Once);
        }

        [TestMethod]
        public void AddFeatureFilter_FeatureFilterWithoutFeatureEnumType_CallsBaseAddFeatureFilter()
        {
            // Arrange and Act
            _underTest.AddFeatureFilter<FeatureFilterWithoutFeatureEnumType>();

            // Assert
            _baseFeatureManagementBuilder.Verify(x => x.AddFeatureFilter<FeatureFilterWithoutFeatureEnumType>(), Times.Once);
        }
    }
}