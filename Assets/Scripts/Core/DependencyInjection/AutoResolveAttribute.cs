using System;

namespace Core.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoResolveAttribute : Attribute
    {
    }
}