# TFeatureManagement

TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries to add better support for using enums to define and reference feature flags, which is Microsoft's recommended approach for defining and referencing feature flags. It does so by implementing generic classes, methods and interfaces (hence the name TFeatureManagement) that wrap the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore classes, methods and interfaces. These generic classes, methods and interfaces ensure the use of an enum to define and consume feature flags.

As TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries, it is recommended that you familiarise yourself with those libraries in addition to reading this readme as a lot of the concepts and functionality in those libraries apply to TFeatureManagement as well. The project site for those libraries can be found at https://github.com/microsoft/FeatureManagement-Dotnet.

## Registration

As TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries, the .NET Core configuration system is used to determine the state of feature flags. See https://github.com/microsoft/FeatureManagement-Dotnet#registration for more information on declaring feature flags using the .NET Core configuration system.

### Referencing

To make it possible to reference the configured feature flags in code, define feature flag variables like below.

``` C#
// Define feature flags in an enum
public enum MyFeatureFlags
{
    FeatureT,
    FeatureU,
    FeatureV
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
      services.AddFeatureManagement<MyFeatureFlags>()
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
      services.AddFeatureManagement<MyFeatureFlags>()
              .AddFeatureFilter<PercentageFilter>()
              .AddFeatureFilter<TimeWindowFilter>();
  }
}
```

**Note:** TFeatureManagement supports all built-in Microsoft.FeatureManagement feature filters.

## Consumption

The simplest use case for feature flags is to do a conditional check for whether a feature is enabled to take different paths in code. The uses cases grow from there as the feature flag API begins to offer extensions into ASP.NET Core.

### Feature Check

The basic form of feature management is checking if a feature is enabled and then performing actions based on the result. This is done through the `IFeatureManager<TFeature>`'s `IsEnabledAsync` method.

``` C#
…
IFeatureManager<MyFeatureFlags> featureManager;
…
if (await featureManager.IsEnabledAsync(MyFeatureFlags.FeatureU))
{
    // Do something
}
```

### Dependency Injection

When using the feature management library with MVC, the `IFeatureManager<TFeature>` can be obtained through dependency injection.

``` C#
public class HomeController : Controller
{
    private readonly IFeatureManager<MyFeatureFlags> _featureManager;
    
    public HomeController(IFeatureManager<MyFeatureFlags> featureManager)
    {
        _featureManager = featureManager;
    }
}
```

### View

In MVC views `<feature>` tags can be used to conditionally render content based on whether a feature is enabled or not.

``` HTML+Razor
<feature features="new[] { MyFeatureFlags.FeatureX }">
  <p>This can only be seen if 'FeatureX' is enabled.</p>
</feature>
```

The `<feature>` tag can also be used to render content based on whether a set of features are enabled or not.

``` HTML+Razor
<feature features="new[] { MyFeatureFlags.FeatureX,MyFeatureFlags.FeatureY }">
  <p>This can only be seen if 'FeatureX' and 'FeatureY' are enabled.</p>
</feature>
```

The above example requires all the features to be enabled to render the content but the `<feature>` tag can also be configured to only require any of the features to be enabled.

``` HTML+Razor
<feature features="new[] { MyFeatureFlags.FeatureX,MyFeatureFlags.FeatureY }" requirement="Any">
  <p>This can be seen if either 'FeatureX' or 'FeatureY' are enabled.</p>
</feature>
```

The `<feature>` tag can also be configured to negate the evaluation of the features. If configured to require all of the features to be enabled then the content will be rendered if not all of the features are enabled. If configured to require any of the features to be enabled then the content will be rendered if none of the features are enabled.

``` HTML+Razor
<feature features="new[] { MyFeatureFlags.FeatureX,MyFeatureFlags.FeatureY }" requirement="All" negate="true">
  <p>This can be seen if not both 'FeatureX' and 'FeatureY' are enabled.</p>
</feature>
```
``` HTML+Razor
<feature features="new[] { MyFeatureFlags.FeatureX,MyFeatureFlags.FeatureY }" requirement="Any" negate="true">
  <p>This can only be seen if neither 'FeatureX' or 'FeatureY' are enabled.</p>
</feature>
```

The `<feature>` tag requires you to create a `FeatureTagHelper` that inherits from `FeatureTagHelper<TFeature>` to work (as ASP.NET Core does not support using a generic tag helper directly - see https://github.com/aspnet/Mvc/issues/6513).

``` C#
using TFeatureManagement;
using TFeatureManagement.AspNetCore.TagHelpers;

namespace TFeatureManagement.AspNetCore.Example.TagHelpers
{
    public class FeatureTagHelper : FeatureTagHelper<MyFeatureFlags>
    {
        public FeatureTagHelper(IFeatureManagerSnapshot<MyFeatureFlags> featureManager)
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

### MVC Filters

MVC action filters can be set up to conditionally execute based on the state of a feature. This is done by registering MVC filters in a feature aware manner.
The feature management pipeline supports async MVC Action filters, which implement `IAsyncActionFilter`.

``` C#
services.AddMvc(o => 
{
    o.Filters.AddForFeature<SomeMvcFilter>(MyFeatureFlags.FeatureV);
});
```

The code above adds an MVC filter named `SomeMvcFilter`. This filter is only triggered within the MVC pipeline if the feature it specifies, "FeatureV", is enabled.

### Application building

The feature management library can be used to add application branches and middleware that execute conditionally based on feature state.

``` C#
app.UseMiddlewareForFeature<ThirdPartyMiddleware>(MyFeatureFlags.FeatureU);
```

With the above call, the application adds a middleware component that only appears in the request pipeline if the feature "FeatureU" is enabled. If the feature is enabled/disabled during runtime, the middleware pipeline can be changed dynamically.

This builds off the more generic capability to branch the entire application based on a feature.

``` C#
app.UseForFeature(MyFeatureFlags.FeatureU, appBuilder => 
{
    appBuilder.UseMiddleware<T>();
});
```
