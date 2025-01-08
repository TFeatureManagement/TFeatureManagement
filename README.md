[![Status](https://github.com/TFeatureManagement/TFeatureManagement/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/TFeatureManagement/TFeatureManagement/actions/workflows/dotnet.yml?query=branch%3Amaster)
[![Nuget](https://img.shields.io/nuget/v/TFeatureManagement?label=TFeatureManagement)](https://www.nuget.org/packages/TFeatureManagement)
[![Nuget](https://img.shields.io/nuget/v/TFeatureManagement.AspNetCore?label=TFeatureManagement.AspNetCore)](https://www.nuget.org/packages/TFeatureManagement.AspNetCore)

# TFeatureManagement

TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries to add support for using enums to define and reference feature flags. It does so by implementing generic classes, methods and interfaces (hence the name TFeatureManagement) that wrap the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore classes, methods and interfaces. These generic classes, methods and interfaces ensure the use of an enum to define and consume feature flags.

As TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries, it is recommended that you familiarise yourself with those libraries in addition to reading this readme as a lot of the concepts and functionality in those libraries apply to TFeatureManagement as well. The project website and source repository for those libraries can be found in the [Microsoft.FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet) GitHub repo.

## Feature Flag Configuration

As TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries, the .NET Core configuration system is used to determine the state of feature flags. See [Feature Flags](https://learn.microsoft.com/en-au/azure/azure-app-configuration/feature-management-dotnet-reference#feature-flags) for more information on declaring feature flags using the .NET Core configuration system.

## Registration

### Referencing

To make it possible to reference the configured feature flags in code, define feature flag variables like below.

``` C#
// Define feature flags in an enum
public enum Feature
{
    FeatureX,
    FeatureY,
    FeatureZ
}
```
    
### Service Registration

Feature flags rely on .NET Core dependency injection. We can register the feature management services using standard conventions.

For a .NET Core Application:

``` C#
using TFeatureManagement.DependencyInjection;
using Microsoft.FeatureManagement.FeatureFilters;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddFeatureManagement<Feature>()
              .AddFeatureFilter<PercentageFilter>()
              .AddFeatureFilter<TimeWindowFilter>();
  }
}
```

For an ASP.NET Core Application:

``` C#
using TFeatureManagement.AspNetCore.DependencyInjection;
using Microsoft.FeatureManagement.FeatureFilters;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddFeatureManagement<Feature>()
              .AddFeatureFilter<PercentageFilter>()
              .AddFeatureFilter<TimeWindowFilter>();
  }
}
```

**Note:** TFeatureManagement supports all built-in Microsoft.FeatureManagement feature filters.

## Consumption

The simplest use case for feature flags is to do a conditional check for whether a feature is enabled to take different paths in code. The use cases grow from there as the feature flag API begins to offer extensions into ASP.NET Core.

### Feature Check

The basic form of feature management is checking if a feature is enabled and then performing actions based on the result. This is done through the `IFeatureManager<TFeature>`'s `IsEnabledAsync` method.

``` C#
…
IFeatureManager<Feature> featureManager;
…
if (await featureManager.IsEnabledAsync(Feature.FeatureX))
{
    // Do something
}
```

The `IFeatureManager<TFeature>`'s `IsEnabledAsync` method can also be used to check if a set of features are enabled.

``` C#
…
IFeatureManager<Feature> featureManager;
…
if (await featureManager.IsEnabledAsync(Feature.FeatureX, Feature.FeatureY))
{
    // Do something
}
```

The above example checks whether all the features are enabled, but the method can also be used to checked whether any of features are enabled.

``` C#
…
IFeatureManager<Feature> featureManager;
…
if (await featureManager.IsEnabledAsync(RequirementType.Any, Feature.FeatureX, Feature.FeatureY))
{
    // Do something
}
```

### Dependency Injection

When using the feature management library with MVC, the `IFeatureManager<TFeature>` can be obtained through dependency injection.

``` C#
public class HomeController : Controller
{
    private readonly IFeatureManager<Feature> _featureManager;
    
    public HomeController(IFeatureManager<Feature> featureManager)
    {
        _featureManager = featureManager;
    }
}
```

### Enabling / Disabling Controllers and Actions

MVC controllers and actions can require a feature to be enabled for the controller or action to be enabled. This can be done by using a `FeatureActionFilterAttribute`, which can be found in the `TFeatureManagement.AspNetCore.Mvc.Filters` namespace.

``` C#
[FeatureActionFilter<Feature>(Feature.FeatureX)]
public class HomeController : Controller
{
    …
}
```

MVC controllers and actions can also require a set of features to be enabled for the controller or action to be enabled.

``` C#
[FeatureActionFilter<Feature>(Feature.FeatureX, Feature.FeatureY)]
public class HomeController : Controller
{
    …
}
```

The above example requires all the features to be enabled for the controller or action to be enabled but the controller or action can also only require any of the features to be enabled.

``` C#
[FeatureActionFilter<Feature>(RequirementType.Any, Feature.FeatureX, Feature.FeatureY)]
public class HomeController : Controller
{
    …
}
```

When a controller or action is disabled because the required feature(s) are not enabled, a registered `IDisabledActionHandler<TFeature>` will be invoked. By default, a minimalistic handler is used which returns HTTP 404. This can be overridden using the `UseDisabledActionHandler<TFeature>` extensions for `IFeatureManagementBuilder<TFeature>` when adding feature management.

``` C#
public interface IDisabledActionHandler<TFeature>
    where TFeature : struct, Enum
{
    Task HandleDisabledAction(IEnumerable<TFeature> features, ActionExecutingContext context);
}
```

### Enabling / Disabling Controller and Action route matching

MVC controllers and actions can require a feature to be enabled for the controller or action to be matched during routing. This allows for multiple actions to have the same route but only have one of them matching during routing. This can be done by using a `FeatureActionConstraintAttribute`, which can be found in the `TFeatureManagement.AspNetCore.Mvc.ActionConstraints` namespace.

``` C#
[HttpGet("featureconstrained", Order = -1)]
[FeatureActionConstraint<Feature>(Feature.FeatureX)]
public IActionResult FeatureConstrained()
{
    return new OkObjectResult($"Visible if the feature is enabled.");
}

[HttpGet("featureconstrained")]
public IActionResult FeatureConstrainedFallback()
{
    return new OkObjectResult($"Visible if the feature for the {nameof(FeatureConstrained)} action is not enabled.");
}
```

**Note:** It is important that the routes have an order defined otherwise multiple routes will be returned from matching if they are all enabled. In conventional routing (including legacy routing) the routes will have an order based on the order they are defined in, but for attribute routing it is important to define an order for the routes. Generally, the order value for the route with the `FeatureActionConstraintAttribute` on it should be lower than the order value for the other route(s).

MVC controllers and actions can also require a set of features to be enabled for the controller or action to be matched during routing.

``` C#
[HttpGet("featureconstrained", Order = -1)]
[FeatureActionConstraint<Feature>(Feature.FeatureX, Feature.FeatureY)]
public IActionResult FeatureConstrained()
{
    return new OkObjectResult($"Visible if all the features are enabled.");
}

[HttpGet("featureconstrained")]
public IActionResult FeatureConstrainedFallback()
{
    return new OkObjectResult($"Visible if any of the features for the {nameof(FeatureConstrained)} action are not enabled.");
}
```

The above example requires all the features to be enabled for the controller or action to be matched during routing but the controller or action can also only require any of the features to be enabled.

``` C#
[HttpGet("featureconstrained", Order = -1)]
[FeatureActionConstraint<Feature>(RequirementType.Any, Feature.FeatureX, Feature.FeatureY)]
public IActionResult FeatureConstrained()
{
    return new OkObjectResult($"Visible if any of the features are enabled.");
}

[HttpGet("featureconstrained")]
public IActionResult FeatureConstrainedFallback()
{
    return new OkObjectResult($"Visible if all of the features for the {nameof(FeatureConstrained)} action are not enabled.");
}
```

### View

In MVC views `<feature>` tags can be used to conditionally render content based on whether a feature is enabled or not.

``` HTML+Razor
<feature features="new[] { Feature.FeatureX }">
  <p>This can only be seen if 'FeatureX' is enabled.</p>
</feature>
```

The `<feature>` tag requires you to create a `FeatureTagHelper` that inherits from `FeatureTagHelper<TFeature>` to work (as ASP.NET Core does not support using a generic tag helper directly - see https://github.com/aspnet/Mvc/issues/6513).

``` C#
using TFeatureManagement;
using TFeatureManagement.AspNetCore.TagHelpers;

namespace TFeatureManagement.AspNetCore.Example.TagHelpers
{
    public class FeatureTagHelper : FeatureTagHelper<Feature>
    {
        public FeatureTagHelper(IFeatureManagerSnapshot<Feature> featureManager)
            : base(featureManager)
        {
        }
    }
}
```

The tag helper should then be added to the ViewImports.cshtml file.

``` HTML+Razor
@addTagHelper *, TFeatureManagement.AspNetCore.Example
```

The `<feature>` tag can also be used to render content based on whether a set of features are enabled or not.

``` HTML+Razor
<feature features="new[] { Feature.FeatureX,Feature.FeatureY }">
  <p>This can only be seen if 'FeatureX' and 'FeatureY' are enabled.</p>
</feature>
```

The above example requires all the features to be enabled to render the content but the `<feature>` tag can also be configured to only require any of the features to be enabled.

``` HTML+Razor
<feature features="new[] { Feature.FeatureX,Feature.FeatureY }" requirement-type="Any">
  <p>This can be seen if either 'FeatureX' or 'FeatureY' are enabled.</p>
</feature>
```

The `<feature>` tag can also be configured to require not all or not any of the features to be enabled. If configured to require not all of the features to be enabled then the content will be rendered if not all of the features are enabled. If configured to require not any of the features to be enabled then the content will be rendered if none of the features are enabled.

``` HTML+Razor
<feature features="new[] { Feature.FeatureX,Feature.FeatureY }" requirement-type="NotAll">
  <p>This can be seen if not both 'FeatureX' and 'FeatureY' are enabled.</p>
</feature>
```
``` HTML+Razor
<feature features="new[] { Feature.FeatureX,Feature.FeatureY }" requirement-type="NotAny">
  <p>This can only be seen if neither 'FeatureX' or 'FeatureY' are enabled.</p>
</feature>
```

### MVC Filters

MVC action filters can be set up to conditionally execute based on the state of a feature. This is done by registering MVC filters in a feature aware manner.
The feature management pipeline supports async MVC Action filters, which implement `IAsyncActionFilter`.

``` C#
services.AddMvc(o => 
{
    o.Filters.AddForFeature<SomeMvcFilter>(Feature.FeatureX);
});
```

The code above adds an MVC filter named `SomeMvcFilter`. This filter is only triggered within the MVC pipeline if the feature it specifies, "FeatureX", is enabled.

### Application building

The feature management library can be used to add application branches and middleware that execute conditionally based on feature state.

``` C#
app.UseMiddlewareWhenFeatureEnabled<ThirdPartyMiddleware>(Feature.FeatureX);
```

With the above call, the application adds a middleware component that only appears in the request pipeline if the feature "FeatureX" is enabled. If the feature is enabled/disabled during runtime, the middleware pipeline can be changed dynamically.

This builds off the more generic capability to branch the entire application based on feature state.

``` C#
app.UseWhenFeatureEnabled(Feature.FeatureX, appBuilder => 
{
    appBuilder.UseMiddleware<T>();
});
```
