using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommander.Helpers
{
    internal static class CollectionExtension
    {
        public static int IndexOf<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int index = 0; index != list.Count; ++index)
            {
                if (predicate(list[index]))
                {
                    return index;
                }
            }

            return -1;
        }
    }
}
