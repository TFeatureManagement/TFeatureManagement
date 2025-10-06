using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;

namespace TFeatureManagement.Tests
{
    [TestClass]
    public class FeatureDefaultProviderTests
    {
        public const string TestsEnvironmentName = "Tests";

        private FeatureDefaultProvider<FeatureDefaultProviderFeature> _underTest;

        private IHostEnvironment _environment;

        [TestInitialize]
        public void Setup()
        {
            _environment = Substitute.For<IHostEnvironment>();
            _environment.EnvironmentName = TestsEnvironmentName;

            _underTest = new FeatureDefaultProvider<FeatureDefaultProviderFeature>(
                _environment);
        }

        [TestMethod]
        public void Constructor_FeatureEnumHasValidIsEnabledDefaultAttributes_InitialisesIsEnabledDefaultsCorrectly()
        {
            // Arrange and Act
            var underTest = new FeatureDefaultProvider<FeatureDefaultProviderFeature>(
                _environment);

            // Assert
            underTest.IsEnabledDefaults
                .Should()
                .HaveCount(4)
                .And
                .Contain(
                    new KeyValuePair<FeatureDefaultProviderFeature, bool>(FeatureDefaultProviderFeature.Default, true),
                    new KeyValuePair<FeatureDefaultProviderFeature, bool>(FeatureDefaultProviderFeature.DefaultForCurrentEnvironment, true),
                    new KeyValuePair<FeatureDefaultProviderFeature, bool>(FeatureDefaultProviderFeature.DefaultForCurrentEnvironmentOnly, true),
                    new KeyValuePair<FeatureDefaultProviderFeature, bool>(FeatureDefaultProviderFeature.DefaultForCurrentAndOtherEnvironment, true)
                )
                .And
                .NotContain(x => x.Key == FeatureDefaultProviderFeature.NoDefault, "because the value has no is enabled default attributes.")
                .And
                .NotContain(x => x.Key == FeatureDefaultProviderFeature.DefaultForOtherEnvironmentOnly, "because the value has no is enabled default attributes for the current environment.");
        }

        [TestMethod]
        public void Constructor_FeatureEnumHasValueWithMultipleIsEnabledEnvironmentDefaultAttributes_ThrowsExpectedException()
        {
            // Arrange and Act
            var action = () => new FeatureDefaultProvider<FeatureDefaultProviderFeatureInvalidAttributes>(
                _environment);

            // Assert
            action.Should().Throw<FeatureManagementException>()
                .Where(ex => ex.ErrorType == FeatureManagementErrorType.MultipleIsEnabledEnvironmentDefaultAttributes);
        }

        [TestMethod]
        public void TryGetIsEnabledDefault_FeatureHasIsEnabledDefault_ReturnsTrueAndSetsOutParameterCorrectly()
        {
            // Arrange
            var feature = FeatureDefaultProviderFeature.Default;

            // Act
            var hasDefault = _underTest.TryGetIsEnabledDefault(feature, out bool isEnabledDefault);

            // Assert
            hasDefault.Should().BeTrue();
            isEnabledDefault.Should().BeTrue();
        }

        [TestMethod]
        public void TryGetIsEnabledDefault_FeatureDoesNotHaveIsEnabledDefault_ReturnsFalse()
        {
            // Arrange
            var feature = FeatureDefaultProviderFeature.NoDefault;

            // Act
            var hasDefault = _underTest.TryGetIsEnabledDefault(feature, out bool _);

            // Assert
            hasDefault.Should().BeFalse();
        }
    }
}
