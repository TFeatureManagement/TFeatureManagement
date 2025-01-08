namespace TFeatureManagement;

public interface IFeatureEnumParser<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Converts the string representation of the name of a feature to the equivalent <typeparamref name="TFeature"
    /// /> value.
    /// </summary>
    /// <param name="featureName">The feature name.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case; otherwise, <c>false</c>.</param>
    /// <param name="feature">
    /// When this method returns, if the conversion succeeded, contains an object of type <typeparamref
    /// name="TFeature" /> whose value is represented by <paramref name="featureName" />. If the conversion failed,
    /// contains the default value of the underlying type of <typeparamref name="TFeature" />.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="featureName" /> parameter was converted successfully; otherwise, false.
    /// </returns>
    public bool TryParse(string featureName, bool ignoreCase, out TFeature feature);
}