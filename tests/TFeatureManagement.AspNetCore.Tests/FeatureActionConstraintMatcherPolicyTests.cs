using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Mvc.Routing;

namespace TFeatureManagement.AspNetCore.Tests;

[TestClass]
public class FeatureActionConstraintMatcherPolicyTests
{
    private FeatureActionConstraintMatcherPolicy<Feature> _underTest;

    private IFeatureManagerSnapshot<Feature> _featureManager;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private IFeatureCollection _httpFeatures;

    [TestInitialize]
    public void Initialize()
    {
        _underTest = new FeatureActionConstraintMatcherPolicy<Feature>();

        _featureManager = Substitute.For<IFeatureManagerSnapshot<Feature>>();

        _httpContext = Substitute.For<HttpContext>();
        _httpContext.RequestServices.GetService(typeof(IFeatureManagerSnapshot<Feature>)).Returns(_featureManager);

        _httpRequest = Substitute.For<HttpRequest>();
        _httpContext.Request.Returns(_httpRequest);

        _httpFeatures = Substitute.For<IFeatureCollection>();
        _httpContext.Features.Returns(_httpFeatures);
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
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { Substitute.For<IFeatureActionConstraintMetadata<Feature>>() })
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
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
        _httpContext.Received().RequestServices.GetService(typeof(IFeatureManagerSnapshot<Feature>));
    }

    [TestMethod]
    public async Task ApplyAsyncInternal_EndpointIsNotValid_DoesNotCallFeatureManagerIsEnabled()
    {
        // Arrange
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { Substitute.For<IFeatureActionConstraintMetadata<Feature>>() })
        };
        var candidates = CreateCandidateSet(endpoints);
        candidates.SetValidity(0, false);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
#pragma warning disable CA2012 // Use ValueTasks correctly
        _ = _featureManager.DidNotReceive().IsEnabledAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>());
#pragma warning restore CA2012 // Use ValueTasks correctly
    }

    [TestMethod]
    public async Task ApplyAsyncInternal_EndpointIsNotValid_DoesNotSetValidityOfEndpoint()
    {
        // Arrange
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { Substitute.For<IFeatureActionConstraintMetadata<Feature>>() })
        };
        var candidates = CreateCandidateSet(endpoints);
        candidates.SetValidity(0, false);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
        candidates.IsValidCandidate(0).Should().BeFalse();
    }

    [TestMethod]
    public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsNull_DoesNotCallFeatureManagerIsEnabled()
    {
        // Arrange
        var features = default(List<Feature>);
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
#pragma warning disable CA2012 // Use ValueTasks correctly
        _ = _featureManager.DidNotReceive().IsEnabledAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>());
#pragma warning restore CA2012 // Use ValueTasks correctly
    }

    [TestMethod]
    public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsNull_DoesNotSetValidityOfEndpoint()
    {
        // Arrange
        var features = default(List<Feature>);
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
        candidates.IsValidCandidate(0).Should().BeTrue();
    }

    [TestMethod]
    public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsEmpty_DoesNotCallFeatureManagerIsEnabled()
    {
        // Arrange
        var features = new List<Feature>();
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
#pragma warning disable CA2012 // Use ValueTasks correctly
        _ = _featureManager.DidNotReceive().IsEnabledAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>());
#pragma warning restore CA2012 // Use ValueTasks correctly
    }

    [TestMethod]
    public async Task ApplyAsyncInternal_EndpointMetadataFeaturesIsEmpty_DoesNotSetValidityOfEndpoint()
    {
        // Arrange
        var features = new List<Feature>();
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

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
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
        foreach (var feature in features)
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
            _ = _featureManager.Received().IsEnabledAsync(feature, Arg.Any<CancellationToken>());
#pragma warning restore CA2012 // Use ValueTasks correctly
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
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        endpointMetadata.RequirementType.Returns(RequirementType.All);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        _featureManager.IsEnabledAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

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
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        endpointMetadata.RequirementType.Returns(RequirementType.All);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        _featureManager.IsEnabledAsync(Feature.Test1, Arg.Any<CancellationToken>()).Returns(true);
        _featureManager.IsEnabledAsync(Feature.Test2, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

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
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        endpointMetadata.RequirementType.Returns(RequirementType.Any);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        _featureManager.IsEnabledAsync(Feature.Test1, Arg.Any<CancellationToken>()).Returns(true);
        _featureManager.IsEnabledAsync(Feature.Test2, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

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
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        endpointMetadata.RequirementType.Returns(RequirementType.Any);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        _featureManager.IsEnabledAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

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
        var endpointMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        endpointMetadata.Features.Returns(features);
        var additionalFeatures = new List<Feature>
        {
            Feature.Test2
        };
        var additionalMetadata = Substitute.For<IFeatureActionConstraintMetadata<Feature>>();
        additionalMetadata.Features.Returns(additionalFeatures);
        var endpoints = new[]
        {
            CreateEndpoint<Feature>("/", new List<IFeatureActionConstraintMetadata<Feature>> { endpointMetadata, additionalMetadata })
        };
        var candidates = CreateCandidateSet(endpoints);

        _featureManager.IsEnabledAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _underTest.ApplyAsyncInternal(_httpContext, candidates);

        // Assert
        foreach (var feature in features)
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
            _ = _featureManager.Received().IsEnabledAsync(feature, Arg.Any<CancellationToken>());
#pragma warning restore CA2012 // Use ValueTasks correctly
        }
        foreach (var feature in additionalFeatures)
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
            _ = _featureManager.DidNotReceive().IsEnabledAsync(feature, Arg.Any<CancellationToken>());
#pragma warning restore CA2012 // Use ValueTasks correctly
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