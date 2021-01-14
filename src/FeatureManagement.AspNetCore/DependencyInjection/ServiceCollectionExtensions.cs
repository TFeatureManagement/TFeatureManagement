using FeatureManagement.AspNetCore.Mvc.ActionConstraints;
using FeatureManagement.AspNetCore.Mvc.Routing;
using FeatureManagement.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace FeatureManagement.AspNetCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IFeatureManagementBuilder<TFeature> AddFeatureManagement<TFeature>(this IServiceCollection services)
            where TFeature : Enum
        {
            var builder = FeatureManagement.DependencyInjection.ServiceCollectionExtensions.AddFeatureManagement<TFeature>(services);
            builder.Services.AddSingleton<IFeatureActionConstraintFactory<TFeature>, FeatureActionConstraintFactory<TFeature>>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IActionConstraintProvider, FeatureActionConstraintProvider<TFeature>>());
#if !NETCOREAPP2_1
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, FeatureActionConstraintMatcherPolicy<TFeature>>());
#endif

            return builder;
        }
    }
}