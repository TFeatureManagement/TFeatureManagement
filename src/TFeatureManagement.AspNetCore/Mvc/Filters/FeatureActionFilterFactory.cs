using Microsoft.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Mvc.Filters;

public class FeatureActionFilterFactory<TFeature> : IFeatureActionFilterFactory<TFeature>
    where TFeature : struct, Enum
{
    public IFilterMetadata CreateInstance(IFeatureActionFilterMetadata<TFeature> filterMetadata)
    {
        var featureActionFilter = new FeatureActionFilter<TFeature>(filterMetadata.Features, filterMetadata.RequirementType);
        if (filterMetadata is IOrderedFilter orderedFilter)
        {
            featureActionFilter.Order = orderedFilter.Order;
        }

        return featureActionFilter;
    }
}