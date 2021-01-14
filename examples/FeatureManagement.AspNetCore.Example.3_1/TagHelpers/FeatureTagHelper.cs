using FeatureManagement.AspNetCore.Example.Models;
using FeatureManagement.AspNetCore.Mvc.TagHelpers;

namespace FeatureManagement.AspNetCore.Example.TagHelpers
{
    public class FeatureTagHelper : FeatureTagHelper<Feature>
    {
        public FeatureTagHelper(IFeatureManagerSnapshot<Feature> featureManager)
            : base(featureManager)
        {
        }
    }
}