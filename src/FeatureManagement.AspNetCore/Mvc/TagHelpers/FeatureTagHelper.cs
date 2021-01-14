using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureManagement.AspNetCore.Mvc.TagHelpers
{
    public class FeatureTagHelper<TFeature> : TagHelper
        where TFeature : Enum
    {
        private readonly IFeatureManager<TFeature> _featureManager;

        /// <summary>
        /// Creates a feature tag helper.
        /// </summary>
        /// <param name="featureManager">
        /// The feature manager snapshot to use to evaluate feature state.
        /// </param>
        public FeatureTagHelper(IFeatureManagerSnapshot<TFeature> featureManager)
        {
            _featureManager = featureManager;
        }

        [HtmlAttributeName("features")]
        public IEnumerable<TFeature> Features { get; set; }

        /// <summary>
        /// Controls whether 'All' or 'Any' feature in a list of features should be enabled to
        /// render the content within the feature tag.
        /// </summary>
        public RequirementType Requirement { get; set; } = RequirementType.All;

        /// <summary>
        /// Negates the evaluation for whether or not a feature tag should display content. This is
        /// used to display alternate content when a feature or set of features are disabled.
        /// </summary>
        public bool Negate { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null; // We don't want the feature tag to actually be a part of HTML, so we strip it

            var features = Features.ToDictionary(feature => feature, _ => false);
            foreach (var feature in Features)
            {
                features[feature] = await _featureManager.IsEnabledAsync(feature).ConfigureAwait(false);
            }

            bool enabled = false;
            if (Requirement == RequirementType.All)
            {
                enabled = features.Values.All(isEnabled => isEnabled);
            }
            else
            {
                enabled = features.Values.Any(isEnabled => isEnabled);
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