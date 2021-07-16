using System;
using System.Linq;

namespace TFeatureManagement.Metadata
{
    public class FeatureTeamProvider<TFeature, TFeatureMetadata, TFeatureTeam> : IFeatureMetadataProvider<TFeature, TFeatureMetadata>
        where TFeature : struct, Enum
        where TFeatureMetadata : IFeatureMetadata<TFeature>, new()
        where TFeatureTeam : struct, Enum
    {
        public void CreateFeatureMetadata(FeatureMetadataProviderContext<TFeature, TFeatureMetadata> context)
        {
            if (context.FeatureMetadata is IFeatureTeam<TFeatureTeam> featureTeam)
            {
                var featureTeamAttribute = context.Attributes.OfType<IFeatureTeam<TFeatureTeam>>().SingleOrDefault();
                if (featureTeamAttribute != null)
                {
                    featureTeam.Team = featureTeamAttribute.Team;
                }
            }
        }
    }
}