using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace TFeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    public class FeatureActionConstraintProvider<TFeature> : IActionConstraintProvider
        where TFeature : Enum
    {
        private readonly MvcOptions _mvcOptions;

        public FeatureActionConstraintProvider(IOptions<MvcOptions> options)
        {
            _mvcOptions = options.Value;
        }

        /// <inheritdoc />
        public int Order => -1000;

        /// <inheritdoc />
        public void OnProvidersExecuting(ActionConstraintProviderContext context)
        {
#if !NETCOREAPP2_1
            if (_mvcOptions.EnableEndpointRouting)
            {
                // If using endpoint routing then an endpoint selector policy is used to check feature action
                // constraints and this provider does not need to execute.
                return;
            }
#endif

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var factory = context.HttpContext.RequestServices.GetRequiredService<IFeatureActionConstraintFactory<TFeature>>();

            foreach (var item in context.Results)
            {
                ProvideConstraint(item, factory);
            }
        }

        /// <inheritdoc />
        public void OnProvidersExecuted(ActionConstraintProviderContext context)
        {
        }

        private void ProvideConstraint(ActionConstraintItem item, IFeatureActionConstraintFactory<TFeature> factory)
        {
            // Don't overwrite anything that was done by a previous provider.
            if (item.Constraint != null)
            {
                return;
            }

            if (item.Metadata is IFeatureActionConstraintMetadata<TFeature> constraintMetadata)
            {
                item.Constraint = factory.CreateInstance(constraintMetadata);
                item.IsReusable = true;
            }
        }
    }
}