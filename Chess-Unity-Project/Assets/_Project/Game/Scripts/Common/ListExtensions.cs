using System;
using System.Collections.Generic;
using System.Linq;

namespace SteampunkChess
{
    public static class ListExtensions
    {
        public static List<int> FindAllIndexes<T>(this List<T> source, Func<T, bool> condition)
        {
            return source
                .Where(v => condition.Invoke(v))
                .Select((item, index) => new { Item = item, Index = index })
                .Select(v => v.Index)
                .ToList();
        }
    }
}