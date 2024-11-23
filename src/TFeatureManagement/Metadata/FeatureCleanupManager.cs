using System.Reflection;
using System.Runtime.CompilerServices;

namespace TFeatureManagement.Metadata;

/// <inheritdoc cref="IFeatureCleanupManager{TFeature}" />
public class FeatureCleanupManager<TFeature> : IFeatureCleanupManager<TFeature>
    where TFeature : struct, Enum
{
    private readonly IFeatureManager<TFeature> _featureManager;
    private readonly IFeatureNameProvider<TFeature> _featureNameProvider;

    /// <summary>
    /// Creates a feature cleanup manager.
    /// </summary>
    /// <param name="featureManager">The feature manager.</param>
    /// <param name="featureNameProvider">The feature enum provider.</param>
    public FeatureCleanupManager(
        IFeatureManager<TFeature> featureManager,
        IFeatureNameProvider<TFeature> featureNameProvider)
    {
        _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        _featureNameProvider = featureNameProvider ?? throw new ArgumentNullException(nameof(featureNameProvider));
    }

    /// <inheritdoc />
    public IDictionary<TFeature, TFeatureCleanupDate> GetFeatureCleanupDates<TFeatureCleanupDate>()
        where TFeatureCleanupDate : Attribute, IFeatureCleanupDate
    {
        var featureCleanupDates = new Dictionary<TFeature, TFeatureCleanupDate>();

#if NET8_0_OR_GREATER
        var features = Enum.GetValues<TFeature>();
#else
        var features = Enum
            .GetValues(typeof(TFeature))
            .Cast<TFeature>();
#endif

        foreach (var feature in features)
        {
            var featureFieldInfo = typeof(TFeature).GetField(_featureNameProvider.GetFeatureName(feature));
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
#if NET8_0_OR_GREATER
        var featureKeys = Enum
            .GetValues<TFeature>()
            .Select(_featureNameProvider.GetFeatureName)
            .ToList();
#else
        var featureKeys = Enum
            .GetValues(typeof(TFeature))
            .Cast<TFeature>()
            .Select(_featureNameProvider.GetFeatureName)
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