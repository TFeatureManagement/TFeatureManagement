using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Mvc.Filters;
using TFeatureManagement.AspNetCore.Mvc.Routing;
using TFeatureManagement.DependencyInjection;

namespace TFeatureManagement.AspNetCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IFeatureManagementBuilder<TFeature> AddFeatureManagement<TFeature>(this IServiceCollection services)
            where TFeature : struct, Enum
        {
            var builder = TFeatureManagement.DependencyInjection.ServiceCollectionExtensions.AddFeatureManagement<TFeature>(services);
            builder.Services.AddSingleton<IFeatureActionFilterFactory<TFeature>, FeatureActionFilterFactory<TFeature>>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IFilterProvider, FeatureActionFilterProvider<TFeature>>());
            builder.Services.AddSingleton<IFeatureActionConstraintFactory<TFeature>, FeatureActionConstraintFactory<TFeature>>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IActionConstraintProvider, FeatureActionConstraintProvider<TFeature>>());
#if !NETCOREAPP2_1
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, FeatureActionConstraintMatcherPolicy<TFeature>>());
#endif

            return builder;
        }
    }
}