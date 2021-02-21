using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    internal class TypedSessionManagerExecutor<TFeature, TSessionManager> : ISessionManager
        where TFeature : Enum
        where TSessionManager : ISessionManager<TFeature>
    {
        private readonly TSessionManager _sessionManager;
        private readonly IEnumParser<TFeature> _enumParser;

        internal TypedSessionManagerExecutor(TSessionManager sessionManager, IEnumParser<TFeature> enumParser)
        {
            _sessionManager = sessionManager;
            _enumParser = enumParser;
        }

        public Task<bool?> GetAsync(string featureName)
        {
            var feature = _enumParser.Parse(featureName);

            return _sessionManager.GetAsync(feature);
        }

        public Task SetAsync(string featureName, bool enabled)
        {
            var feature = _enumParser.Parse(featureName);

            return _sessionManager.SetAsync(feature, enabled);
        }
    }
}