using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Mvc.Routing;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.AspNetCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IFeatureManagementBuilder<TFeature> AddFeatureManagement<TFeature>(this IServiceCollection services)
            where TFeature : Enum
        {
            var builder = TFeatureManagement.DependencyInjection.ServiceCollectionExtensions.AddFeatureManagement<TFeature>(services);
            builder.Services.AddSingleton<IFeatureActionConstraintFactory<TFeature>, FeatureActionConstraintFactory<TFeature>>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IActionConstraintProvider, FeatureActionConstraintProvider<TFeature>>());
#if !NETCOREAPP2_1
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, FeatureActionConstraintMatcherPolicy<TFeature>>());
#endif

            return builder;
        }
    }
}