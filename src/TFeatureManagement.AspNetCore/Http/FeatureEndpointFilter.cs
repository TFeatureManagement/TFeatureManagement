#if NET8_0_OR_GREATER

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Http;

/// <summary>
/// An endpoint filter that can be used to require a set of features to be enabled for an endpoint to be enabled. If
/// the required features are not enabled the registered <see cref="IDisabledEndpointHandler{TFeature}{TFeature}" />
/// will be invoked.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public class FeatureEndpointFilter<TFeature> : IEndpointFilter
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an endpoint filter that requires all the provided feature(s) to be enabled for the actions to be
    /// enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureEndpointFilter(IEnumerable<TFeature> features)
        : this(features, RequirementType.All)
    {
    }

    /// <summary>
    /// Creates an endpoint filter that requires the provided feature(s) to be enabled for the endpoint to be enabled.
    /// The filter can be configured to require all or any of the provided feature(s) to be enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    /// <param name="requirementType">
    /// Specifies whether all or any of the provided features should be enabled.
    /// </param>
    public FeatureEndpointFilter(IEnumerable<TFeature> features, RequirementType requirementType)
    {
        if (features?.Any() != true)
        {
            throw new ArgumentNullException(nameof(features));
        }

        Features = features;
        RequirementType = requirementType;
    }

    /// <summary>
    /// Gets the features that should be enabled.
    /// </summary>
    public IEnumerable<TFeature> Features { get; }

    /// <summary>
    /// Gets which features in <see cref="Features" /> should be enabled.
    /// </summary>
    public RequirementType RequirementType { get; }

    /// <inheritdoc />
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var featureManager = context.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

        if (await featureManager.IsEnabledAsync(RequirementType, Features))
        {
            return await next(context);
        }
        else
        {
            var disabledEndpointHandler = context.HttpContext.RequestServices.GetService<IDisabledEndpointHandler<TFeature>>() ?? new NotFoundDisabledEndpointHandler<TFeature>();

            await disabledEndpointHandler.HandleDisabledEndpoint(Features, RequirementType, context);
            return new ValueTask<object>(Task.FromResult<object>(null));
        }
    }
}

#endif