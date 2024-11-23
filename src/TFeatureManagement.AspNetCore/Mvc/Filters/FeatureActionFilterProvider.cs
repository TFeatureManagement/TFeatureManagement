using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace TFeatureManagement.AspNetCore.Mvc.Filters;

public class FeatureActionFilterProvider<TFeature> : IFilterProvider
    where TFeature : struct, Enum
{
    public int Order => -1000;

    public void OnProvidersExecuting(FilterProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.ActionContext.ActionDescriptor.FilterDescriptors != null)
        {
            var factory = context.ActionContext.HttpContext.RequestServices.GetRequiredService<IFeatureActionFilterFactory<TFeature>>();

            var results = context.Results;
            // Perf: Avoid allocating enumerator and read interface .Count once rather than per iteration
            var resultsCount = results.Count;
            for (var i = 0; i < resultsCount; i++)
            {
                ProvideFilter(results[i], factory);
            }
        }
    }

    public void OnProvidersExecuted(FilterProviderContext context)
    {
    }

    public void ProvideFilter(FilterItem filterItem, IFeatureActionFilterFactory<TFeature> factory)
    {
        if (filterItem.Descriptor.Filter is IFeatureActionFilterMetadata<TFeature> filterMetadata)
        {
            filterItem.Filter = factory.CreateInstance(filterMetadata);
            filterItem.IsReusable = true;
        }
    }
}