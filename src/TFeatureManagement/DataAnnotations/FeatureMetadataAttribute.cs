using System;

namespace TFeatureManagement.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FeatureMetadataAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the description of the feature.
        /// </summary>
        public string Description { get; set; }
    }
}