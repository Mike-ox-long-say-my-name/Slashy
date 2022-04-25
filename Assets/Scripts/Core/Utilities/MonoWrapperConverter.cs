using System.Collections.Generic;
using System.Linq;
using Core.DependencyInjection;

namespace Core.Utilities
{
    public static class MonoWrapperConverter
    {
        public static List<T> ToNativeList<T>(this IEnumerable<IMonoWrapper<T>> source) where T : class
        {
            return source.Select(wrapper => wrapper.Resolve()).ToList();
        }
    }
}