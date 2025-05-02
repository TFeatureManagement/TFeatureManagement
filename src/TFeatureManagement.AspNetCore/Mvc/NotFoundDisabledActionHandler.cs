using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Mvc;

/// <summary>
/// A disabled action handler that returns a not found status code result for disabled actions.
/// </summary>
public class NotFoundDisabledActionHandler<TFeature> : IDisabledActionHandler<TFeature>
    where TFeature : struct, Enum
{
    /// <inheritdoc />
    [Obsolete("Use HandleDisabledActionAsync instead. This will be removed in an upcoming major release.", false)]
    public async Task HandleDisabledAction(IEnumerable<TFeature> features, ActionExecutingContext context)
    {
        await HandleDisabledActionAsync(features, RequirementType.Any, context);
    }

    /// <inheritdoc />
    public Task HandleDisabledActionAsync(IEnumerable<TFeature> features, RequirementType requirementType, ActionExecutingContext context)
    {
        context.Result = new StatusCodeResult(StatusCodes.Status404NotFound);

        return Task.CompletedTask;
    }
}