using FeatureManagement.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

#if !NETCOREAPP2_1

using Microsoft.AspNetCore.Routing.Matching;

#endif

using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if !NETCOREAPP2_1

namespace FeatureManagement.AspNetCore.Mvc.Routing
{
    public class FeatureActionConstraintMatcherPolicy<TFeature> : MatcherPolicy, IEndpointSelectorPolicy
        where TFeature : Enum
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

#if NETCOREAPP3_1

        public async Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            await ApplyAsyncInternal(httpContext, candidates).ConfigureAwait(false);
            return;
        }

#else

        public async Task ApplyAsync(HttpContext httpContext, EndpointSelectorContext context, CandidateSet candidates)
        {
            await ApplyAsyncInternal(httpContext, candidates).ConfigureAwait(false);
            return;
        }

#endif

        internal async Task ApplyAsyncInternal(HttpContext httpContext, CandidateSet candidates)
        {
            var featureManager = httpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

            // Perf: Avoid allocations
            for (var i = 0; i < candidates.Count; i++)
            {
                if (candidates.IsValidCandidate(i))
                {
                    var candidate = candidates[i];

                    //foreach (var metadata in candidate.Endpoint.Metadata.GetOrdered7Metadata<IFeatureActionConstraintMetadata<TFeature>>())
                    var actionDescriptor = candidate.Endpoint.Metadata.GetMetadata<ActionDescriptor>();
                    if (actionDescriptor != null)
                    {
                        var enabled = true;

                        foreach (var constraintMetadata in actionDescriptor?.ActionConstraints?
                            .Where(c => c is IFeatureActionConstraintMetadata<TFeature>)
                            .Select(c => c as IFeatureActionConstraintMetadata<TFeature>)
                            .ToList())
                        {
                            var isEnabledTasks = new List<Task<bool>>();
                            foreach (var feature in constraintMetadata.Features)
                            {
                                isEnabledTasks.Add(featureManager.IsEnabledAsync(feature));
                            }

                            await Task.WhenAll(isEnabledTasks).ConfigureAwait(false);

                            if (constraintMetadata.RequirementType == RequirementType.All)
                            {
                                enabled = enabled && isEnabledTasks.Select(t => t.Result).All(isEnabled => isEnabled);
                            }
                            else
                            {
                                enabled = enabled && isEnabledTasks.Select(t => t.Result).Any(isEnabled => isEnabled);
                            }
                        }

                        if (!enabled)
                        {
                            candidates.SetValidity(i, false);
                        }
                    }
                }
            }

            return;
        }
    }
}

#endif