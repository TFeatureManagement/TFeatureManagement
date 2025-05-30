﻿namespace TFeatureManagement;

/// <summary>
/// Used to store feature state across a session. The implementor is free to decide what constitutes a session.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public interface ISessionManager<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Queries the session manager for the session's feature state, if any, for the given feature.
    /// </summary>
    /// <param name="feature">The feature.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The state of the feature if it is present in the session, otherwise null.</returns>
    Task<bool?> GetAsync(TFeature feature, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the state of a feature to be used for a session.
    /// </summary>
    /// <param name="feature">The feature.</param>
    /// <param name="enabled">The state of the feature.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    Task SetAsync(TFeature feature, bool enabled, CancellationToken cancellationToken = default);
}