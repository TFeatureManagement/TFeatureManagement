﻿namespace TFeatureManagement
{
    /// <summary>
    /// Describes whether any or all features in a given set should be required to be considered enabled.
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
        All
    }
}