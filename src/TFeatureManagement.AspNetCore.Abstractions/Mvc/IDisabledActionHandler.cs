using Microsoft.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Mvc;

/// <summary>
/// A handler that is invoked when an MVC action is disabled because it requires all or any of a set of features to
/// be enabled but the features are not enabled.
/// </summary>
public interface IDisabledActionHandler<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Handle requests to an MVC action that is disabled because it requires all or any of a set of features to be
    /// enabled but the features are not enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled for the action to be enabled.</param>
    /// <param name="context">The action executing context provided by MVC.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the handler has executed.</returns>
    Task HandleDisabledAction(IEnumerable<TFeature> features, ActionExecutingContext context);
}