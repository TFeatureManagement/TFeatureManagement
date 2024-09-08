using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace TFeatureManagement.AspNetCore.Mvc.Filters;

public class FeatureGatedAsyncActionFilter<TFilter, TFeature> : IAsyncActionFilter
    where TFilter : IAsyncActionFilter
    where TFeature : struct, Enum
{
    public FeatureGatedAsyncActionFilter(TFeature feature)
    {
        Feature = feature;
    }

    public TFeature Feature { get; }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var featureManager = context.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

        if (await featureManager.IsEnabledAsync(Feature))
        {
            var serviceProvider = context.HttpContext.RequestServices.GetRequiredService<IServiceProvider>();
            var filter = ActivatorUtilities.CreateInstance<TFilter>(serviceProvider);

            await filter.OnActionExecutionAsync(context, next);
        }
        else
        {
            await next();
        }
    }
}