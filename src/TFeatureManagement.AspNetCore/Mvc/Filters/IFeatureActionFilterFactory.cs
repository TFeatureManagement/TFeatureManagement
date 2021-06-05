using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TFeatureManagement.AspNetCore.Mvc.Filters
{
    public interface IFeatureActionFilterFactory<TFeature>
        where TFeature : Enum
    {
        IFilterMetadata CreateInstance(IFeatureActionFilterMetadata<TFeature> filterMetadata);
    }
}