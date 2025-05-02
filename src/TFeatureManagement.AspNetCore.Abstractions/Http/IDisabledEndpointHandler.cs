using Microsoft.AspNetCore.Http;

namespace TFeatureManagement.AspNetCore.Http;

/// <summary>
/// A handler that is invoked when an endpoint is disabled because it requires a set of features to be enabled but the
/// features are not enabled.
/// </summary>
public interface IDisabledEndpointHandler<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Handle requests to an endpoint that is disabled because it requires a set of features to be enabled but the
    /// features are not enabled.
    /// </summary>
    /// <param name="features">The set of features that should be enabled for the endpoint to be enabled.</param>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="context">The <see cref="EndpointFilterInvocationContext"/> associated with the current request/response.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the handler has executed.</returns>
    [Obsolete("Use HandleDisabledEndpointAsync instead. This will be removed in an upcoming major release.", false)]
    Task HandleDisabledEndpoint(IEnumerable<TFeature> features, RequirementType requirementType, EndpointFilterInvocationContext context);

    /// <summary>
    /// Handle requests to an endpoint that is disabled because it requires a set of features to be enabled but the
    /// features are not enabled.
    /// </summary>
    /// <param name="features">The set of features that should be enabled for the endpoint to be enabled.</param>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="context">The <see cref="EndpointFilterInvocationContext"/> associated with the current request/response.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the handler has executed.</returns>
    async Task HandleDisabledEndpointAsync(IEnumerable<TFeature> features, RequirementType requirementType, EndpointFilterInvocationContext context)
#pragma warning disable CS0618 // Type or member is obsolete
        => await HandleDisabledEndpoint(features, requirementType, context);
#pragma warning restore CS0618 // Type or member is obsolete
}