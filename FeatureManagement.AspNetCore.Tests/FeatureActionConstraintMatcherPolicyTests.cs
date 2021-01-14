using FeatureManagement.AspNetCore.Mvc.ActionConstraints;
using FeatureManagement.AspNetCore.Mvc.Routing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

#if !NETCOREAPP2_1
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if !NETCOREAPP2_1

namespace FeatureManagement.AspNetCore.Tests
{
    [TestClass]
    public class FeatureActionConstraintMatcherPolicyTests
    {
        private FeatureActionConstraintMatcherPolicy<Feature> _underTest;

        private Mock<IFeatureManagerSnapshot<Feature>> _featureManager;
        private Mock<HttpContext> _httpContext;

        [TestInitialize]
        public void Initialize()
        {
            _underTest = new FeatureActionConstraintMatcherPolicy<Feature>();

            _featureManager = new Mock<IFeatureManagerSnapshot<Feature>>();

            _httpContext = new Mock<HttpContext>();
            _httpContext.Setup(x => x.RequestServices.GetRequiredService<IFeatureManagerSnapshot<Feature>>()).Returns(_featureManager.Object);
        }

        [TestMethod]
        public void AppliesToEndpoints_NoEndpoints_ReturnsFalse()
        {
            // Arrange
            var endpoints = new List<Endpoint>();

            // Act
            var result = _underTest.AppliesToEndpoints(endpoints);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void AppliesToEndpoints_NoEndpointsWithFeatureActionConstraintMetadata_ReturnsFalse()
        {
            // Arrange
            var endpoints = new List<Endpoint>
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>>())
            };

            // Act
            var result = _underTest.AppliesToEndpoints(endpoints);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void AppliesToEndpoints_EndpointsWithFeatureActionConstraintMetadata_ReturnsTrue()
        {
            // Arrange
            var endpoints = new List<Endpoint>
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { new Mock<IFeatureActionConstraintMetadata<Feature>>().Object })
            };

            // Act
            var result = _underTest.AppliesToEndpoints(endpoints);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void ApplyAsyncInternal_EndpointsWithFeatureActionConstraintMetadata_ReturnsTrue()
        {
            // Arrange
            var endpoints = new List<Endpoint>
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { new Mock<IFeatureActionConstraintMetadata<Feature>>().Object })
            };

            // Act
            var result = _underTest.AppliesToEndpoints(endpoints);

            // Assert
            result.Should().BeTrue();
        }

        internal static readonly RequestDelegate _emptyRequestDelegate = (_) => Task.CompletedTask;

        private static RouteEndpoint CreateEndpoint<TFeature>(string template, IList<IFeatureActionConstraintMetadata<Feature>> metadataItems)
            where TFeature : Enum
        {
            return new RouteEndpoint(
                _emptyRequestDelegate,
                RoutePatternFactory.Parse(template),
                0,
                new EndpointMetadataCollection(metadataItems),
                $"test: {template}");
        }
    }
}

#endif