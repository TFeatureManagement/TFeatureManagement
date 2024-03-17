using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Threading;
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

        /// <inheritdoc />
        public IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default)
        {
            return _baseFeatureManagerSnapshot.GetFeatureNamesAsync();
        }

        /// <inheritdoc />
        public ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default)
        {
            return new ValueTask<bool>(_baseFeatureManagerSnapshot.IsEnabledAsync(feature.ToString()));
        }

        /// <inheritdoc />
        public ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default)
        {
            return new ValueTask<bool>(_baseFeatureManagerSnapshot.IsEnabledAsync(feature.ToString(), context));
        }
    }
}