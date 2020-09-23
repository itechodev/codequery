using System.Collections.Generic;

namespace CodeQuery
{
    public static class ListExtensions
    {
        public static T IndexOrDefault<T>(this IList<T> list, int index)
        {
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }

            return default;
        }
    }
}