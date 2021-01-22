using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    public abstract class SessionManagerBase<TFeature> : ISessionManager<TFeature>
        where TFeature : struct, Enum
    {
        public abstract Task SetAsync(TFeature feature, bool enabled);

        public Task SetAsync(string featureName, bool enabled)
        {
            if (Enum.TryParse<TFeature>(featureName, out var feature))
            {
                return SetAsync(feature, enabled);
            }

            throw new ArgumentOutOfRangeException(nameof(featureName));
        }

        public abstract Task<bool?> GetAsync(TFeature feature);

        public Task<bool?> GetAsync(string featureName)
        {
            if (Enum.TryParse<TFeature>(featureName, out var feature))
            {
                return GetAsync(feature);
            }

            throw new ArgumentOutOfRangeException(nameof(featureName));
        }
    }
}