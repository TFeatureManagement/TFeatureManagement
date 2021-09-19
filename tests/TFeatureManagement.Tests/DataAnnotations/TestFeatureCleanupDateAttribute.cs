using System;
using TFeatureManagement.Metadata;

namespace TFeatureManagement.Tests.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TestFeatureCleanupDateAttribute : Attribute, IFeatureCleanupDate
    {
        public TestFeatureCleanupDateAttribute(int year, int month, int day)
        {
            CleanupYear = year;
            CleanupMonth = month;
            CleanupDay = day;
        }

        public int CleanupYear { get; }

        public int CleanupMonth { get; }

        public int CleanupDay { get; }

        public DateTime? CleanupDate => DateTime.TryParse($"{CleanupYear}-{CleanupMonth}-{CleanupDay}", out var cleanupDate) ? (DateTime?)cleanupDate : null;
    }
}