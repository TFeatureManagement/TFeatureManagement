using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TFeatureManagement
{
    /// <summary>
    /// Used to evaluate whether a feature is enabled or disabled.
    /// </summary>
    /// <typeparam name="TFeature">The feature enum type.</typeparam>
    public interface IFeatureManager<TFeature>
        where TFeature : struct, Enum
    {
        /// <summary>
        /// Retrieves a list of feature names registered in the feature manager.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// An enumerator which provides asynchronous iteration over the feature names registered in the feature
        /// manager.
        /// </returns>
        IAsyncEnumerable<string> GetFeatureNamesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether a given feature is enabled.
        /// </summary>
        /// <param name="feature">The feature to check.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns><see langword="true"/> if the feature is enabled; otherwise, <see langword="false"/>.</returns>
        ValueTask<bool> IsEnabledAsync(TFeature feature, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether a given feature is enabled.
        /// </summary>
        /// <param name="feature">The feature to check.</param>
        /// <param name="context">
        /// A context providing information that can be used to evaluate whether a feature should be on or off.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns><see langword="true"/> if the feature is enabled; otherwise, <see langword="false"/>.</returns>
        ValueTask<bool> IsEnabledAsync<TContext>(TFeature feature, TContext context, CancellationToken cancellationToken = default);
    }
}