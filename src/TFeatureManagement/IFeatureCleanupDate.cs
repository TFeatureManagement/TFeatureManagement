using System;

namespace TFeatureManagement
{
    public interface IFeatureCleanupDate
    {
        /// <summary>
        /// Gets the cleanup year.
        /// </summary>
        public int CleanupYear { get; }

        /// <summary>
        /// Gets the cleanup month.
        /// </summary>
        public int CleanupMonth { get; }

        /// <summary>
        /// Gets the cleanup day.
        /// </summary>
        public int CleanupDay { get; }

        /// <summary>
        /// Gets the cleanup date.
        /// </summary>
        /// <remarks>Returns null if the cleanup date is not a valid date.</remarks>
        public DateTime? CleanupDate { get; }
    }
}