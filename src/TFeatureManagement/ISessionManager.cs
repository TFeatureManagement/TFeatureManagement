using Microsoft.FeatureManagement;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    /// <summary>
    /// Used to store feature state across a session. The implementor is free to decide what constitutes a session.
    /// </summary>
    public interface ISessionManager<TFeature> : ISessionManager
    {
        /// <summary>
        /// Set the state of a feature to be used for a session.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="enabled">The state of the feature.</param>
        Task SetAsync(TFeature feature, bool enabled);

        /// <summary>
        /// Queries the session manager for the session's feature state, if any, for the given feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>The state of the feature if it is present in the session, otherwise null.</returns>
        Task<bool?> GetAsync(TFeature feature);
    }
}