using System;

namespace TFeatureManagement.Metadata
{
    public interface IFeatureTeam<TFeatureTeam>
        where TFeatureTeam : Enum
    {
        TFeatureTeam Team { get; set; }
    }
}