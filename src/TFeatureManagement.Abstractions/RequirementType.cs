namespace TFeatureManagement;

/// <summary>
/// Describes which features in a given set should be required to be enabled for the set to be considered enabled.
/// </summary>
public enum RequirementType
{
    /// <summary>
    /// The enabled state will be attained if any feature in the set is enabled.
    /// </summary>
    Any,

    /// <summary>
    /// The enabled state will be attained if all features in the set are enabled.
    /// </summary>
    All,

    /// <summary>
    /// The enabled state will be attained if not any features in the set are enabled (none are enabled).
    /// </summary>
    NotAny,

    /// <summary>
    /// The enabled state will be attained if not all features in the set are enabled (any are disabled).
    /// </summary>
    NotAll
}