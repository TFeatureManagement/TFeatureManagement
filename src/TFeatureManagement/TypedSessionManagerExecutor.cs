using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    internal class TypedSessionManagerExecutor<TFeature, TSessionManager> : ISessionManager
        where TFeature : struct, Enum
        where TSessionManager : ISessionManager<TFeature>
    {
        private readonly TSessionManager _sessionManager;
        private readonly IFeatureEnumParser<TFeature> _featureEnumParser;

        public TypedSessionManagerExecutor(TSessionManager sessionManager, IFeatureEnumParser<TFeature> featureEnumParser)
        {
            _sessionManager = sessionManager;
            _featureEnumParser = featureEnumParser;
        }

        public async Task<bool?> GetAsync(string featureName)
        {
            if (_featureEnumParser.TryParse(featureName, true, out TFeature feature))
            {
                return await _sessionManager.GetAsync(feature).ConfigureAwait(false);
            }

            return null;
        }

        public async Task SetAsync(string featureName, bool enabled)
        {
            if (_featureEnumParser.TryParse(featureName, true, out TFeature feature))
            {
                await _sessionManager.SetAsync(feature, enabled).ConfigureAwait(false);
            }
        }
    }
}