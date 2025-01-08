using Microsoft.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Mvc.Filters;

public interface IFeatureActionFilterFactory<TFeature>
    where TFeature : struct, Enum
{
    IFilterMetadata CreateInstance(IFeatureActionFilterMetadata<TFeature> filterMetadata);
}