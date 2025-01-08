namespace TFeatureManagement.Metadata;

/// <summary>
/// Used to manage feature cleanup.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public interface IFeatureCleanupManager<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Gets feature cleanup dates.
    /// </summary>
    /// <typeparam name="TFeatureCleanupDate">The feature cleanup date attribute type.</typeparam>
    /// <returns>The feature cleanup dates.</returns>
    public IDictionary<TFeature, TFeatureCleanupDate?> GetFeatureCleanupDates<TFeatureCleanupDate>()
        where TFeatureCleanupDate : Attribute, IFeatureCleanupDate;

    /// <summary>
    /// Gets a list of feature names registered in the feature manager that do not have a matching value in the
    /// specified feature enum.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// An enumerator which provides asynchronous iteration over the feature names registered in the feature manager
    /// that do not have a matching value in the specified feature enum.
    /// </returns>
    IAsyncEnumerable<string> GetFeatureNamesNotInFeatureEnumAsync(CancellationToken cancellationToken = default);
}