namespace TFeatureManagement;

public class TargetingContext : ITargetingContext
{
    public string? UserId { get; set; }

    public IEnumerable<string>? Groups { get; set; }
}
