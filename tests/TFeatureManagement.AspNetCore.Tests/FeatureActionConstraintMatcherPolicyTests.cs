using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Mvc.Routing;

namespace TFeatureManagement.AspNetCore.Tests
{
    [TestClass]
    public class FeatureActionConstraintMatcherPolicyTests
    {
        private FeatureActionConstraintMatcherPolicy<Feature> _underTest;

        private Mock<IFeatureManagerSnapshot<Feature>> _featureManager;
        private Mock<HttpContext> _httpContext;
        private Mock<HttpRequest> _httpRequest;
        private Mock<IFeatureCollection> _httpFeatures;

        [TestInitialize]
        public void Initialize()
        {
            _underTest = new FeatureActionConstraintMatcherPolicy<Feature>();

            _featureManager = new Mock<IFeatureManagerSnapshot<Feature>>();

            _httpContext = new Mock<HttpContext>();
            _httpContext.Setup(x => x.RequestServices.GetService(typeof(IFeatureManagerSnapshot<Feature>))).Returns(_featureManager.Object);

            _httpRequest = new Mock<HttpRequest>();
            _httpContext.Setup(x => x.Request).Returns(_httpRequest.Object);

            _httpFeatures = new Mock<IFeatureCollection>();
            _httpContext.Setup(x => x.Features).Returns(_httpFeatures.Object);
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
        public async Task ApplyAsyncInternal_GetsFeatureManagerCorrectly()
        {
            // Arrange
            var endpoints = new Endpoint[]
            {
            };
            var candidates = CreateCandidateSet(endpoints);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            _httpContext.Verify(x => x.RequestServices.GetService(typeof(IFeatureManagerSnapshot<Feature>)), Times.Once);
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointIsNotValid_DoesNotCallFeatureManagerIsEnabled()
        {
            // Arrange
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { new Mock<IFeatureActionConstraintMetadata<Feature>>().Object })
            };
            var candidates = CreateCandidateSet(endpoints);
            candidates.SetValidity(0, false);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            _featureManager.Verify(x => x.IsEnabledAsync(It.IsAny<Feature>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointIsNotValid_DoesNotSetValidityOfEndpoint()
        {
            // Arrange
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { new Mock<IFeatureActionConstraintMetadata<Feature>>().Object })
            };
            var candidates = CreateCandidateSet(endpoints);
            candidates.SetValidity(0, false);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            candidates.IsValidCandidate(0).Should().BeFalse();
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsNull_DoesNotCallFeatureManagerIsEnabled()
        {
            // Arrange
            var features = default(List<Feature>);
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            _featureManager.Verify(x => x.IsEnabledAsync(It.IsAny<Feature>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsNull_DoesNotSetValidityOfEndpoint()
        {
            // Arrange
            var features = default(List<Feature>);
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            candidates.IsValidCandidate(0).Should().BeTrue();
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsEmpty_DoesNotCallFeatureManagerIsEnabled()
        {
            // Arrange
            var features = new List<Feature>();
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            _featureManager.Verify(x => x.IsEnabledAsync(It.IsAny<Feature>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsEmpty_DoesNotSetValidityOfEndpoint()
        {
            // Arrange
            var features = new List<Feature>();
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            candidates.IsValidCandidate(0).Should().BeTrue();
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsNotEmpty_CallsFeatureManagerIsEnabledForEachFeature()
        {
            // Arrange
            var features = new List<Feature>
            {
                Feature.Test1,
                Feature.Test2
            };
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            foreach (var feature in features)
            {
                _featureManager.Verify(x => x.IsEnabledAsync(feature, It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataRequirementTypeIsAll_DoesNotSetValidityIfAllFeaturesAreEnabled()
        {
            // Arrange
            var features = new List<Feature>
            {
                Feature.Test1,
                Feature.Test2
            };
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            endpointMetadata.Setup(x => x.RequirementType).Returns(RequirementType.All);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            _featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<Feature>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            candidates.IsValidCandidate(0).Should().BeTrue();
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataRequirementTypeIsAll_SetsValidityToFalseIfAnyFeatureIsNotEnabled()
        {
            // Arrange
            var features = new List<Feature>
            {
                Feature.Test1,
                Feature.Test2
            };
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            endpointMetadata.Setup(x => x.RequirementType).Returns(RequirementType.All);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            _featureManager.Setup(x => x.IsEnabledAsync(Feature.Test1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _featureManager.Setup(x => x.IsEnabledAsync(Feature.Test2, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            candidates.IsValidCandidate(0).Should().BeFalse();
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataRequirementTypeIsAny_DoesNotSetValidityIfAnyFeaturesIsEnabled()
        {
            // Arrange
            var features = new List<Feature>
            {
                Feature.Test1,
                Feature.Test2
            };
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            endpointMetadata.Setup(x => x.RequirementType).Returns(RequirementType.Any);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            _featureManager.Setup(x => x.IsEnabledAsync(Feature.Test1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _featureManager.Setup(x => x.IsEnabledAsync(Feature.Test2, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            candidates.IsValidCandidate(0).Should().BeTrue();
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointMetadataRequirementTypeIsAny_SetsValidityToFalseIfAllFeaturesAreDisabled()
        {
            // Arrange
            var features = new List<Feature>
            {
                Feature.Test1,
                Feature.Test2
            };
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            endpointMetadata.Setup(x => x.RequirementType).Returns(RequirementType.Any);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            _featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<Feature>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            candidates.IsValidCandidate(0).Should().BeFalse();
        }

        [TestMethod]
        public async Task ApplyAsyncInternal_EndpointHasAdditionalMetadata_DoesNotProcessAnyMoreMetadataIfAlreadyDeterminedEndpointIsNotEnabled()
        {
            // Arrange
            var features = new List<Feature>
            {
                Feature.Test1
            };
            var endpointMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            endpointMetadata.Setup(x => x.Features).Returns(features);
            var additionalFeatures = new List<Feature>
            {
                Feature.Test2
            };
            var additionalMetadata = new Mock<IFeatureActionConstraintMetadata<Feature>>();
            additionalMetadata.Setup(x => x.Features).Returns(additionalFeatures);
            var endpoints = new[]
            {
                CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata.Object, additionalMetadata.Object })
            };
            var candidates = CreateCandidateSet(endpoints);

            _featureManager.Setup(x => x.IsEnabledAsync(It.IsAny<Feature>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            await _underTest.ApplyAsyncInternal(_httpContext.Object, candidates);

            // Assert
            foreach (var feature in features)
            {
                _featureManager.Verify(x => x.IsEnabledAsync(feature, It.IsAny<CancellationToken>()), Times.Once);
            }
            foreach (var feature in additionalFeatures)
            {
                _featureManager.Verify(x => x.IsEnabledAsync(feature, It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        internal static readonly RequestDelegate _emptyRequestDelegate = (_) => Task.CompletedTask;

        private static RouteEndpoint CreateEndpoint<TFeature>(string template, IList<IFeatureActionConstraintMetadata<Feature>> metadataItems)
            where TFeature : struct, Enum
        {
            return new RouteEndpoint(
                _emptyRequestDelegate,
                RoutePatternFactory.Parse(template),
                0,
                new EndpointMetadataCollection(metadataItems),
                $"test: {template}");
        }

        private static CandidateSet CreateCandidateSet(Endpoint[] endpoints)
        {
            return new CandidateSet(endpoints, new RouteValueDictionary[endpoints.Length], new int[endpoints.Length]);
        }
    }
}