namespace TFeatureManagement.Metadata
{
    public interface IFeatureLifetime<TFeatureLifetime>
    {
        TFeatureLifetime Lifetime { get; set; }
    }
}