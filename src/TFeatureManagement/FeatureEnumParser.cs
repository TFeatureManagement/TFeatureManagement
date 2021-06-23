using System;

namespace TFeatureManagement
{
    public class FeatureEnumParser<T> : IFeatureEnumParser<T>
        where T : struct, Enum
    {
        /// <inheritdoc />
        public bool TryParse(string featureName, bool ignoreCase, out T feature)
        {
            if (Enum.TryParse(featureName, ignoreCase, out feature)
                && Enum.IsDefined(typeof(T), feature))
            {
                return true;
            }
            else
            {
                feature = default;
                return false;
            }
        }
    }
}