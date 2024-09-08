using TFeatureManagement.AspNetCore.Example.Models;
using TFeatureManagement.AspNetCore.Mvc.TagHelpers;

namespace TFeatureManagement.AspNetCore.Example.TagHelpers;

public class FeatureTagHelper : FeatureTagHelper<Feature>
{
    public FeatureTagHelper(IFeatureManagerSnapshot<Feature> featureManager)
        : base(featureManager)
    {
    }
}