using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureManagement
{
    /// <summary>
    /// Used to evaluate whether a feature is enabled or disabled.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureManager<TFeature>
        where TFeature : Enum
    {
        /// <summary>
        /// Retrieves a list of feature names registered in the feature manager.
        /// </summary>
        /// <returns>
        /// An enumerator which provides asynchronous iteration over the feature names registered in
        /// the feature manager.
        /// </returns>
        IAsyncEnumerable<string> GetFeatureNamesAsync();

        /// <summary>
        /// Checks whether a given feature is enabled.
        /// </summary>
        /// <param name="feature">The feature to check.</param>
        /// <returns>True if the feature is enabled, otherwise false.</returns>
        Task<bool> IsEnabledAsync(TFeature feature);

        /// <summary>
        /// Checks whether a given feature is enabled.
        /// </summary>
        /// <param name="feature">The feature to check.</param>
        /// <param name="context">
        /// A context providing information that can be used to evaluate whether a feature should be
        /// on or off.
        /// </param>
        /// <returns>True if the feature is enabled, otherwise false.</returns>
        Task<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context);
    }
}