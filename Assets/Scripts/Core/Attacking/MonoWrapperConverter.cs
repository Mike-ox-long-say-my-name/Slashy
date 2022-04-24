using System.Collections.Generic;
using System.Linq;
using Core.Characters;

namespace Core.Attacking
{
    public static class MonoWrapperConverter
    {
        public static List<T> ToNativeList<T>(this IEnumerable<IMonoWrapper<T>> source) where T : class
        {
            return source.Select(wrapper => wrapper.Native).ToList();
        }
    }
}