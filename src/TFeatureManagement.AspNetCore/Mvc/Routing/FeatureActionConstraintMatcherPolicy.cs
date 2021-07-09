#if !NETCOREAPP2_1

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Extensions;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;

namespace TFeatureManagement.AspNetCore.Mvc.Routing
{
    public class FeatureActionConstraintMatcherPolicy<TFeature> : MatcherPolicy, IEndpointSelectorPolicy
        where TFeature : struct, Enum
    {
        public override int Order => 0;

        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            var appliesToEndpoints = false;

            foreach (var endpoint in endpoints)
            {
                if (endpoint.Metadata.GetOrderedMetadata<IFeatureActionConstraintMetadata<TFeature>>().Any())
                {
                    appliesToEndpoints = true;
                    break;
                }
            }

            return appliesToEndpoints;
        }

#if NETSTANDARD2_0

        public async Task ApplyAsync(HttpContext httpContext, EndpointSelectorContext context, CandidateSet candidates)
        {
            await ApplyAsyncInternal(httpContext, candidates).ConfigureAwait(false);
            return;
        }

#else

        public async Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            await ApplyAsyncInternal(httpContext, candidates).ConfigureAwait(false);
            return;
        }

#endif

        internal async Task ApplyAsyncInternal(HttpContext httpContext, CandidateSet candidates)
        {
            var featureManager = httpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

            for (var i = 0; i < candidates.Count; i++)
            {
                if (candidates.IsValidCandidate(i))
                {
                    var candidate = candidates[i];

                    var enabled = true;

                    foreach (var metadata in candidate.Endpoint.Metadata
                        .GetOrderedMetadata<IFeatureActionConstraintMetadata<TFeature>>()
                        .Where(m => m.Features?.Any() == true))
                    {
                        enabled = enabled && (metadata.RequirementType == RequirementType.All ?
                            await metadata.Features.All(async feature => await featureManager.IsEnabledAsync(feature).ConfigureAwait(false)).ConfigureAwait(false) :
                            await metadata.Features.Any(async feature => await featureManager.IsEnabledAsync(feature).ConfigureAwait(false)).ConfigureAwait(false));

                        if (!enabled)
                        {
                            // If the endpoint has multiple feature action constraints we don't want to evaluate any
                            // more constraints if we have already determined that the endpoint is not enabled.
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
}

#endif