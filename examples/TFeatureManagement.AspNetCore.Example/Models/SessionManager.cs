using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Example.Models
{
    public class SessionManager : ISessionManager<Feature>
    {
        public Task<bool?> GetAsync(Feature feature)
        {
            return Task.FromResult<bool?>(null);
        }

        public Task SetAsync(Feature feature, bool enabled)
        {
            return Task.CompletedTask;
        }
    }
}