using TFeatureManagement.Tests.DataAnnotations;

namespace TFeatureManagement.Tests
{
    public enum Feature
    {
        [FeatureCleanupDate(2000, 1, 1)]
        Test1,

        Test2
    }
}