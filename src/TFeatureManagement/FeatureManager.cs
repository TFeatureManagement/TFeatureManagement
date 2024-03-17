using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Threading;
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

        /// <inheritdoc />
        public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
        {
            return _baseFeatureManager.GetFeatureNamesAsync();
        }

        /// <inheritdoc />
        public Task<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
        {
            return _baseFeatureManager.IsEnabledAsync(feature.ToString());
        }

        /// <inheritdoc />
        public Task<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
        {
            return _baseFeatureManager.IsEnabledAsync(feature.ToString(), context);
        }
    }
}