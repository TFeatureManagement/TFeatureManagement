using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;

namespace TFeatureManagement.AspNetCore.Routing.Matching;

public class FeatureMatcherPolicy<TFeature> : MatcherPolicy, IEndpointSelectorPolicy
    where TFeature : struct, Enum
{
    public override int Order => 0;

    public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
    {
        var appliesToEndpoints = false;

        foreach (var endpoint in endpoints)
        {
            if (endpoint.Metadata.GetOrderedMetadata<IFeatureMetadata<TFeature>>().Any())
            {
                appliesToEndpoints = true;
                break;
            }
        }

        return appliesToEndpoints;
    }

    public async Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
    {
        await ApplyAsyncInternal(httpContext, candidates);
        return;
    }

    internal static async Task ApplyAsyncInternal(HttpContext httpContext, CandidateSet candidates)
    {
        var featureManager = httpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

        for (var i = 0; i < candidates.Count; i++)
        {
            if (candidates.IsValidCandidate(i))
            {
                var candidate = candidates[i];

                var enabled = true;

                foreach (var metadata in candidate.Endpoint.Metadata
                    .GetOrderedMetadata<IFeatureMetadata<TFeature>>()
                    .Where(m => m.Features?.Any() == true))
                {
                    enabled = enabled && await featureManager.IsEnabledAsync(metadata.RequirementType, metadata.Features);

                    if (!enabled)
                    {
                        // If the endpoint has multiple feature metadata we don't want to evaluate any more
                        // if we have already determined that the endpoint is not enabled.
                        break;
                    }
                }

                if (!enabled)
                {
                    // The endpoint is not be enabled so set the endpoint validity to false.
                    candidates.SetValidity(i, false);
                }
            }
        }

        return;
    }
}