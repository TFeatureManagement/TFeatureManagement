using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    public class FeatureManager<TFeature> : IFeatureManager<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IFeatureManager _baseFeatureManager;

        public FeatureManager(IFeatureManager baseFeatureManager)
        {
            _baseFeatureManager = baseFeatureManager;
        }

        public IAsyncEnumerable<string> GetFeatureNamesAsync()
        {
            return _baseFeatureManager.GetFeatureNamesAsync();
        }

        public Task<bool> IsEnabledAsync(TFeature feature)
        {
            return _baseFeatureManager.IsEnabledAsync(feature.ToString());
        }

        public Task<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context)
        {
            return _baseFeatureManager.IsEnabledAsync(feature.ToString(), context);
        }
    }
}