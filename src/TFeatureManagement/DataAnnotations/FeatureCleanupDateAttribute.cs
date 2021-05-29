using System;

namespace TFeatureManagement.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FeatureCleanupDateAttribute : Attribute, IFeatureCleanupDate
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FeatureCleanupDateAttribute" /> with specified cleanup year, month
        /// and day.
        /// </summary>
        /// <param name="year">The cleanup year (1 through 9999).</param>
        /// <param name="month">The cleanup month (1 through 12)</param>
        /// <param name="day">The cleanup day (1 through the number of days in month)</param>
        public FeatureCleanupDateAttribute(int year, int month, int day)
        {
            CleanupDateValue = $"{year}-{month}-{day}";
            if (DateTime.TryParse(CleanupDateValue, out var cleanupDate))
            {
                CleanupDate = cleanupDate;
            }
        }

        /// <inheritdoc />
        public string CleanupDateValue { get; }

        /// <inheritdoc />
        public DateTime? CleanupDate { get; }
    }
}