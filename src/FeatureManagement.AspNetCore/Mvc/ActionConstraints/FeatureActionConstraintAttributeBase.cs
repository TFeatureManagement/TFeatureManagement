using System;

namespace FeatureManagement.AspNetCore.Mvc.ActionConstraints
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class FeatureActionConstraintAttributeBase : Attribute
    {
    }
}