using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Mvc
{
    /// <summary>
    /// A disabled action handler that executes an inline handler.
    /// </summary>
    internal class InlineDisabledActionHandler<TFeature> : IDisabledActionHandler<TFeature>
        where TFeature : Enum
    {
        private readonly Action<IEnumerable<TFeature>, ActionExecutingContext> _handler;

        public InlineDisabledActionHandler(Action<IEnumerable<TFeature>, ActionExecutingContext> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc />
        public Task HandleDisabledAction(IEnumerable<TFeature> features, ActionExecutingContext context)
        {
            _handler(features, context);

            return Task.CompletedTask;
        }
    }
}