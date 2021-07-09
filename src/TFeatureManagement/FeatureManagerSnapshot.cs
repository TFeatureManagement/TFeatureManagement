using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    public class FeatureManagerSnapshot<TFeature> : IFeatureManagerSnapshot<TFeature>
        where TFeature : struct, Enum
    {
        private readonly IFeatureManagerSnapshot _baseFeatureManagerSnapshot;

        public FeatureManagerSnapshot(IFeatureManagerSnapshot baseFeatureManagerSnapshot)
        {
            _baseFeatureManagerSnapshot = baseFeatureManagerSnapshot;
        }

        public IAsyncEnumerable<string> GetFeatureNamesAsync()
        {
            return _baseFeatureManagerSnapshot.GetFeatureNamesAsync();
        }

        public Task<bool> IsEnabledAsync(TFeature feature)
        {
            return _baseFeatureManagerSnapshot.IsEnabledAsync(feature.ToString());
        }

        public Task<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context)
        {
            return _baseFeatureManagerSnapshot.IsEnabledAsync(feature.ToString(), context);
        }
    }
}