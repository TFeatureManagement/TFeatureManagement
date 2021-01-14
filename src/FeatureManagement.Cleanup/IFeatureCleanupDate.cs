using System;

namespace FeatureManagement
{
    public interface IFeatureCleanupDate
    {
        /// <summary>
        /// Gets the string representation of the clean-up date.
        /// </summary>
        public string CleanupDateValue { get; }

        /// <summary>
        /// Gets the clean-up date.
        /// </summary>
        /// <remarks>Returns null if the clean-up date is not a valid date.</remarks>
        public DateTime? CleanupDate { get; }
    }
}