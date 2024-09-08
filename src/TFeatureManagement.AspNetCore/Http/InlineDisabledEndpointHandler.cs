#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Http;

/// <summary>
/// A disabled endpoint handler that executes an inline handler.
/// </summary>
internal class InlineDisabledEndpointHandler<TFeature> : IDisabledEndpointHandler<TFeature>
    where TFeature : struct, Enum
{
    private readonly Action<IEnumerable<TFeature>, RequirementType, EndpointFilterInvocationContext> _handler;

    public InlineDisabledEndpointHandler(Action<IEnumerable<TFeature>, RequirementType, EndpointFilterInvocationContext> handler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    /// <inheritdoc />
    public Task HandleDisabledEndpoint(IEnumerable<TFeature> features, RequirementType requirementType, EndpointFilterInvocationContext context)
    {
        _handler(features, requirementType, context);

        return Task.CompletedTask;
    }
}

#endif