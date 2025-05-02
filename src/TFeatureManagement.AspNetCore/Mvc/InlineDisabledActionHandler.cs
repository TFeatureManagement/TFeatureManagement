using Microsoft.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Mvc;

/// <summary>
/// A disabled action handler that executes an inline handler.
/// </summary>
internal class InlineDisabledActionHandler<TFeature> : IDisabledActionHandler<TFeature>
    where TFeature : struct, Enum
{
    private readonly Action<IEnumerable<TFeature>, RequirementType, ActionExecutingContext> _handler;

    [Obsolete("Use constructor overload that accepts a handler of type Action<IEnumerable<TFeature>, RequirementType, ActionExecutingContext> instead. This will be removed in an upcoming major release.")]
    public InlineDisabledActionHandler(Action<IEnumerable<TFeature>, ActionExecutingContext> handler)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));

        _handler = (features, requirementType, context) => handler(features, context);
    }

    public InlineDisabledActionHandler(Action<IEnumerable<TFeature>, RequirementType, ActionExecutingContext> handler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    /// <inheritdoc />
    [Obsolete("Use HandleDisabledActionAsync instead. This will be removed in an upcoming major release.", false)]
    public Task HandleDisabledAction(IEnumerable<TFeature> features, ActionExecutingContext context)
    {
        _handler(features, RequirementType.Any, context);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task HandleDisabledActionAsync(IEnumerable<TFeature> features, RequirementType requirementType, ActionExecutingContext context)
    {
        _handler(features, requirementType, context);

        return Task.CompletedTask;
    }
}