using Microsoft.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Mvc;

/// <summary>
/// A handler that is invoked when an MVC action is disabled because it requires a set of features to be enabled but
/// the features are not enabled.
/// </summary>
public interface IDisabledActionHandler<TFeature>
    where TFeature : struct, Enum
{
    /// <summary>
    /// Handle requests to an MVC action that is disabled because it requires a set of features to be enabled but the
    /// features are not enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled for the action to be enabled.</param>
    /// <param name="context">The action executing context provided by MVC.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the handler has executed.</returns>
    [Obsolete("Use HandleDisabledActionAsync instead. This will be removed in an upcoming major release.", false)]
    Task HandleDisabledAction(IEnumerable<TFeature> features, ActionExecutingContext context);

    /// <summary>
    /// Handle requests to an MVC action that is disabled because it requires a set of features to be enabled but the
    /// features are not enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled for the action to be enabled.</param>
    /// <param name="requirementType">The requirement type.</param>
    /// <param name="context">The action executing context provided by MVC.</param>
    /// <returns>A <see cref="Task" /> that on completion indicates the handler has executed.</returns>
    async Task HandleDisabledActionAsync(IEnumerable<TFeature> features, RequirementType requirementType, ActionExecutingContext context)
#pragma warning disable CS0618 // Type or member is obsolete
        => await HandleDisabledAction(features, context);
#pragma warning restore CS0618 // Type or member is obsolete
}