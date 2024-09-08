using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFeatureManagement.AspNetCore.Mvc.TagHelpers
{
    /// <summary>
    /// Provides a <![CDATA[<feature>]]> tag that can be used to conditionally render content based on the state of a
    /// feature(s).
    /// </summary>
    public class FeatureTagHelper<TFeature> : TagHelper
        where TFeature : struct, Enum
    {
        private readonly IFeatureManager<TFeature> _featureManager;

        /// <summary>
        /// Creates a feature tag helper that requires the provided feature(s) to be enabled to render the content
        /// within the feature tag. The tag helper can be configured to require all, any, not all or not any of the
        /// provided feature(s) to be enabled.
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
        [HtmlAttributeName("requirement-type")]
        public RequirementType RequirementType { get; set; } = RequirementType.All;

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
                enabled = await _featureManager.IsEnabledAsync(RequirementType, Features);
            }

            if (!enabled)
            {
                output.SuppressOutput();
            }
        }
    }
}