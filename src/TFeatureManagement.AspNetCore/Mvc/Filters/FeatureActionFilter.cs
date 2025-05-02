using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace TFeatureManagement.AspNetCore.Mvc.Filters;

/// <summary>
/// An action filter that can be used to require a set of features to be enabled for an action to be enabled. If the
/// required features are not enabled the registered <see cref="IDisabledActionHandler{TFeature}" /> will be invoked.
/// </summary>
/// <typeparam name="TFeature">The feature enum type.</typeparam>
public class FeatureActionFilter<TFeature> : IAsyncActionFilter, IOrderedFilter
    where TFeature : struct, Enum
{
    /// <summary>
    /// Creates an action filter that requires a set of features to be enabled for the actions to be enabled.
    /// </summary>
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    public FeatureActionFilter(IEnumerable<TFeature> features)
        : this(features, RequirementType.All)
    {
    }

    /// <summary>
    /// Creates an action filter that requires a set of features to be enabled for the actions to be enabled.
    /// </summary>
    /// <param name="features">The features that should be enabled.</param>
    /// <param name="requirementType">The requirement type.</param>
    public FeatureActionFilter(IEnumerable<TFeature> features, RequirementType requirementType)
    {
        if (features?.Any() != true)
        {
            throw new ArgumentNullException(nameof(features));
        }

        Features = features;
        RequirementType = requirementType;
    }

    /// <summary>
    /// Gets the features that should be enabled.
    /// </summary>
    public IEnumerable<TFeature> Features { get; }

    /// <summary>
    /// Gets which features in <see cref="Features" /> should be enabled.
    /// </summary>
    public RequirementType RequirementType { get; }

    /// <inheritdoc />
    public int Order { get; set; }

    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var featureManager = context.HttpContext.RequestServices.GetRequiredService<IFeatureManagerSnapshot<TFeature>>();

        if (await featureManager.IsEnabledAsync(RequirementType, Features))
        {
            await next();
        }
        else
        {
            var disabledActionHandler = context.HttpContext.RequestServices.GetService<IDisabledActionHandler<TFeature>>() ?? new NotFoundDisabledActionHandler<TFeature>();

            await disabledActionHandler.HandleDisabledActionAsync(Features, RequirementType, context);
        }
    }
}