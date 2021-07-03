namespace TFeatureManagement.Metadata
{
    public interface IFeatureCategory<TFeatureCategory>
    {
        TFeatureCategory Category { get; }
    }
}