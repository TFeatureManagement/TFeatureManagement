using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Extensions;

namespace TFeatureManagement.AspNetCore.Mvc.TagHelpers
{
    /// <summary>
    /// Provides a <![CDATA[<feature>]]> tag that can be used to conditionally render content based on the state of a
    /// feature(s).
    /// </summary>
    public class FeatureTagHelper<TFeature> : TagHelper
        where TFeature : Enum
    {
        private readonly IFeatureManager<TFeature> _featureManager;

        /// <summary>
        /// Creates a feature tag helper that requires the provided feature(s) to be enabled to render the content
        /// within the feature tag. The tag helper can be configured to require all or any of the provided feature(s) to
        /// be enabled.
        /// </summary>
        /// <param name="featureManager">The feature manager snapshot to use to evaluate feature state.</param>
        public FeatureTagHelper(IFeatureManagerSnapshot<TFeature> featureManager)
        {
            _featureManager = featureManager;
        }

        /// <summary>
        /// Gets or sets the features that should be enabled.
        /// </summary>
        [HtmlAttributeName("features")]
        public IEnumerable<TFeature> Features { get; set; }

        /// <summary>
        /// Gets or sets whether all or any features in <see cref="Features" /> should be enabled.
        /// </summary>
        public RequirementType Requirement { get; set; } = RequirementType.All;

        /// <summary>
        /// Gets or sets whether to negate the evaluation of the features.
        /// </summary>
        /// <remarks>
        /// This is used to display alternate content when the provided feature(s) are not enabled. If <see
        /// cref="RequirementType" /> is set to <see cref="RequirementType.All" /> then the content will display if not
        /// all of the features in <see cref="Features" /> are enabled. If <see cref="RequirementType" /> is set to <see
        /// cref="RequirementType.Any" /> then the content will display if not any of the features in <see
        /// cref="Features" /> are enabled.
        /// </remarks>
        public bool Negate { get; set; }

        /// <summary>
        /// Processes the tag helper context to evaluate if the content within the feature tag should be rendered.
        /// </summary>
        /// <param name="context">The tag helper context.</param>
        /// <param name="output">The tag helper output.</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null; // We don't want the feature tag to actually be a part of HTML, so we strip it.

            bool enabled = false;

            if (Features?.Any() == true)
            {
                enabled = Requirement == RequirementType.All ?
                    await Features.All(async feature => await _featureManager.IsEnabledAsync(feature).ConfigureAwait(false)).ConfigureAwait(false) :
                    await Features.Any(async feature => await _featureManager.IsEnabledAsync(feature).ConfigureAwait(false)).ConfigureAwait(false);
            }

            if (Negate)
            {
                enabled = !enabled;
            }

            if (!enabled)
            {
                output.SuppressOutput();
            }
        }
    }
}