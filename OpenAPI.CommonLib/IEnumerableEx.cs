using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public static class IEnumerableEx
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> ie, Action<TSource> body)
        {
            System.Threading.Tasks.Parallel.ForEach(ie, body);
        }
    }
}
