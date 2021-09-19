using System;

namespace TFeatureManagement.Metadata
{
    public interface IFeatureTeam<TFeatureTeam>
        where TFeatureTeam : struct, Enum
    {
        TFeatureTeam Team { get; set; }
    }
}