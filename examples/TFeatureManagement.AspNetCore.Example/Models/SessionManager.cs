namespace TFeatureManagement.AspNetCore.Example.Models;

public class SessionManager : ISessionManager<Feature>
{
    public Task<bool?> GetAsync(Feature feature, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<bool?>(null);
    }

    public Task SetAsync(Feature feature, bool enabled, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}