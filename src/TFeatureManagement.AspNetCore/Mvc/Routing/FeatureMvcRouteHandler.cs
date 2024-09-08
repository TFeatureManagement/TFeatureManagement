using Microsoft.AspNetCore.Routing;

namespace TFeatureManagement.AspNetCore.Mvc.Routing;

public class FeatureMvcRouteHandler : IRouter
{
    public FeatureMvcRouteHandler(IRouter defaultHandler)
    {
        DefaultHandler = defaultHandler;
    }

    /// <inheritdoc />
    public IRouter DefaultHandler { get; set; }

    public VirtualPathData GetVirtualPath(VirtualPathContext context)
    {
        return DefaultHandler.GetVirtualPath(context);
    }

    public async Task RouteAsync(RouteContext context)
    {
        await DefaultHandler.RouteAsync(context);
    }
}