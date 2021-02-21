namespace TFeatureManagement
{
    public interface IEnumParser<T>
    {
        T Parse(string value);
    }
}