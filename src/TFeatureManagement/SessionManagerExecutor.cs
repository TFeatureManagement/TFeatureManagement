using Microsoft.FeatureManagement;

namespace TFeatureManagement;

/// <summary>
/// An <see cref="ISessionManager" /> implementation that converts the feature name to <typeparamref name="TFeature"
/// /> and executes an <see cref="ISessionManager{TFeature}" />.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
/// <typeparam name="TSessionManager">The session manager type.</typeparam>
internal class SessionManagerExecutor<TFeature, TSessionManager> : ISessionManager
    where TFeature : struct, Enum
    where TSessionManager : class, ISessionManager<TFeature>
{
    private readonly TSessionManager _sessionManager;
    private readonly IFeatureEnumParser<TFeature> _featureEnumParser;

    /// <summary>
    /// Creates a session manager executor for the provided <typeparamref name="TSessionManager" />.
    /// </summary>
    /// <param name="sessionManager">The session manager to execute.</param>
    /// <param name="featureEnumParser">The feature enum parser for <typeparamref name="TFeature" />.</param>
    public SessionManagerExecutor(TSessionManager sessionManager, IFeatureEnumParser<TFeature> featureEnumParser)
    {
        _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
        _featureEnumParser = featureEnumParser ?? throw new ArgumentNullException(nameof(featureEnumParser));
    }

    /// <inheritdoc />
    public async Task<bool?> GetAsync(string featureName)
    {
        if (_featureEnumParser.TryParse(featureName, true, out TFeature feature))
        {
            return await _sessionManager.GetAsync(feature).ConfigureAwait(false);
        }

        return null;
    }

    /// <inheritdoc />
    public async Task SetAsync(string featureName, bool isEnabled)
    {
        if (_featureEnumParser.TryParse(featureName, true, out TFeature feature))
        {
            await _sessionManager.SetAsync(feature, isEnabled).ConfigureAwait(false);
        }
    }
}