using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Example.Routing
{
    public class FeatureActionConstraintMatcherPolicy : MatcherPolicy, IEndpointSelectorPolicy
    {
        public override int Order => 0;

        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            throw new System.NotImplementedException();
        }

        public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            throw new System.NotImplementedException();
        }
    }
}