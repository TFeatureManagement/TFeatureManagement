using System.Reflection;
using System.Runtime.CompilerServices;

namespace TFeatureManagement.Metadata;

/// <inheritdoc cref="IFeatureCleanupManager{TFeature}" />
public class FeatureCleanupManager<TFeature> : IFeatureCleanupManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IFeatureManager<TFeature> _featureManager;
    private readonly IFeatureEnumConverter<TFeature> _featureEnumConverter;

    /// <summary>
    /// Creates a feature cleanup manager.
    /// </summary>
    /// <param name="featureManager">The feature manager.</param>
    /// <param name="featureEnumConverter">The feature enum converter.</param>
    public FeatureCleanupManager(
        IFeatureManager<TFeature> featureManager,
        IFeatureEnumConverter<TFeature> featureEnumConverter)
    {
        _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        _featureEnumConverter = featureEnumConverter ?? throw new ArgumentNullException(nameof(featureEnumConverter));
    }

    /// <inheritdoc />
    public IDictionary<TFeature, TFeatureCleanupDate> GetFeatureCleanupDates<TFeatureCleanupDate>()
        where TFeatureCleanupDate : Attribute, IFeatureCleanupDate
    {
        var featureCleanupDates = new Dictionary<TFeature, TFeatureCleanupDate>();

#if NET6_0_OR_GREATER
        var features = Enum.GetValues<TFeature>();
#else
        var features = Enum
            .GetValues(typeof(TFeature))
            .Cast<TFeature>();
#endif

        foreach (var feature in features)
        {
            var featureFieldInfo = typeof(TFeature).GetField(_featureEnumConverter.GetFeatureName(feature));
            if (featureFieldInfo != null)
            {
                var featureCleanupDateAttribute = featureFieldInfo.GetCustomAttribute<TFeatureCleanupDate>(false);
                featureCleanupDates.Add(feature, featureCleanupDateAttribute);
            }
        }

        return featureCleanupDates;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<string> GetFeatureNamesNotInFeatureEnumAsync([EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
#if NET6_0_OR_GREATER
        var featureKeys = Enum
            .GetValues<TFeature>()
            .Select(_featureEnumConverter.GetFeatureName)
            .ToList();
#else
        var featureKeys = Enum
            .GetValues(typeof(TFeature))
            .Cast<TFeature>()
            .Select(_featureEnumConverter.GetFeatureName)
            .ToList();
#endif

        await foreach (var featureName in _featureManager.GetFeatureNamesAsync(cancellationToken))
        {
            if (!featureKeys.Contains(featureName))
            {
                yield return featureName;
            }
        }
    }
}