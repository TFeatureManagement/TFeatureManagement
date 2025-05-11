[![Status](https://github.com/TFeatureManagement/TFeatureManagement/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/TFeatureManagement/TFeatureManagement/actions/workflows/dotnet.yml?query=branch%3Amaster)
[![Nuget](https://img.shields.io/nuget/v/TFeatureManagement?label=TFeatureManagement)](https://www.nuget.org/packages/TFeatureManagement)
[![Nuget](https://img.shields.io/nuget/v/TFeatureManagement?label=TFeatureManagement.Abstractions)](https://www.nuget.org/packages/TFeatureManagement.Abstractions)
[![Nuget](https://img.shields.io/nuget/v/TFeatureManagement.AspNetCore?label=TFeatureManagement.AspNetCore)](https://www.nuget.org/packages/TFeatureManagement.AspNetCore)
[![Nuget](https://img.shields.io/nuget/v/TFeatureManagement.AspNetCore?label=TFeatureManagement.AspNetCore.Abstractions)](https://www.nuget.org/packages/TFeatureManagement.AspNetCore.Abstractions)

# TFeatureManagement

TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries to add support for using enums to define and reference feature flags. It does so by implementing generic classes, methods and interfaces (hence the name TFeatureManagement) that wrap the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore classes, methods and interfaces. These generic classes, methods and interfaces ensure the use of an enum to define and consume feature flags.

As TFeatureManagement extends the Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore libraries, it is recommended that you familiarise yourself with those libraries in addition to reading the [TFeatureManagement Wiki](https://github.com/TFeatureManagement/TFeatureManagement/wiki) as a lot of the concepts and functionality in those libraries apply to TFeatureManagement as well. The project website and source repository for those libraries can be found in the [Microsoft.FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet) GitHub repo.
