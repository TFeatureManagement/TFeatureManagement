using System;

namespace TFeatureManagement
{
    public class DefaultEnumParser<T> : IEnumParser<T>
        where T : Enum
    {
        public T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}