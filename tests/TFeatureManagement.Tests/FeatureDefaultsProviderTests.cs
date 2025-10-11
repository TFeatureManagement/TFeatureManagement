using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace TFeatureManagement.Tests
{
    [TestClass]
    public class FeatureDefaultsProviderTests
    {
        public const string TestsEnvironmentName = "Tests";

        private IFeatureNameProvider<FeatureDefaultProviderFeature> _featureNameProvider;
        private IHostEnvironment _environment;

        private readonly Func<FeatureDefaultProviderFeature, string> _getFeatureName = (FeatureDefaultProviderFeature feature) => feature.ToString();

        [TestInitialize]
        public void Setup()
        {
            _featureNameProvider = Substitute.For<IFeatureNameProvider<FeatureDefaultProviderFeature>>();
            _featureNameProvider.GetFeatureName(Arg.Any<FeatureDefaultProviderFeature>()).Returns(x => _getFeatureName((FeatureDefaultProviderFeature)x[0]));
            _environment = Substitute.For<IHostEnvironment>();
            _environment.EnvironmentName = TestsEnvironmentName;
        }

        [TestMethod]
        public void Constructor_FeatureEnumHasValidIsEnabledDefaultAttributes_InitialisesIsEnabledDefaultsCorrectly()
        {
            // Arrange and Act
            var underTest = new FeatureDefaultsProvider<FeatureDefaultProviderFeature>(
                _featureNameProvider,
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
        public void Constructor_FeatureEnumHasValidIsEnabledDefaultAttributes_InitialisesIsEnabledDefaultsByFeatureNameCorrectly()
        {
            // Arrange and Act
            var underTest = new FeatureDefaultsProvider<FeatureDefaultProviderFeature>(
                _featureNameProvider,
                _environment);

            // Assert
            underTest.IsEnabledDefaultsByFeatureName
                .Should()
                .HaveCount(4)
                .And
                .Contain(
                    new KeyValuePair<string, bool>(_getFeatureName(FeatureDefaultProviderFeature.Default), true),
                    new KeyValuePair<string, bool>(_getFeatureName(FeatureDefaultProviderFeature.DefaultForCurrentEnvironment), true),
                    new KeyValuePair<string, bool>(_getFeatureName(FeatureDefaultProviderFeature.DefaultForCurrentEnvironmentOnly), true),
                    new KeyValuePair<string, bool>(_getFeatureName(FeatureDefaultProviderFeature.DefaultForCurrentAndOtherEnvironment), true)
                )
                .And
                .NotContain(x => x.Key.Equals(_getFeatureName(FeatureDefaultProviderFeature.NoDefault)), "because the value has no is enabled default attributes.")
                .And
                .NotContain(x => x.Key.Equals(_getFeatureName(FeatureDefaultProviderFeature.DefaultForOtherEnvironmentOnly)), "because the value has no is enabled default attributes for the current environment.");
        }

        [TestMethod]
        public void Constructor_FeatureEnumHasValueWithMultipleIsEnabledEnvironmentDefaultAttributes_ThrowsExpectedException()
        {
            // Arrange
            var featureNameProvider = Substitute.For<IFeatureNameProvider<FeatureDefaultsProviderFeatureInvalidAttributes>>();

            // Act
            var action = () => new FeatureDefaultsProvider<FeatureDefaultsProviderFeatureInvalidAttributes>(
                featureNameProvider,
                _environment);

            // Assert
            action.Should().Throw<FeatureManagementException>()
                .Where(ex => ex.ErrorType == FeatureManagementErrorType.MultipleIsEnabledDefaultAttributes);
        }
    }
}
