#if NET7_0_OR_GREATER

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Http;

/// <summary>
/// A disabled endpoint handler that returns a not found status code response for disabled endpoints.
/// </summary>
public class NotFoundDisabledEndpointHandler<TFeature> : IDisabledEndpointHandler<TFeature>
    where TFeature : struct, Enum
{
    /// <inheritdoc />
    public Task HandleDisabledEndpoint(IEnumerable<TFeature> features, RequirementType requirementType, EndpointFilterInvocationContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        return Task.CompletedTask;
    }
}

#endif