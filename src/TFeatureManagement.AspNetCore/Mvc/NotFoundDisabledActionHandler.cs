using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Mvc
{
    /// <summary>
    /// A disabled action handler that returns a not found status code result for disabled actions.
    /// </summary>
    public class NotFoundDisabledActionHandler<TFeature> : IDisabledActionHandler<TFeature>
        where TFeature : struct, Enum
    {
        /// <inheritdoc />
        public Task HandleDisabledAction(IEnumerable<TFeature> features, ActionExecutingContext context)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status404NotFound);

            return Task.CompletedTask;
        }
    }
}