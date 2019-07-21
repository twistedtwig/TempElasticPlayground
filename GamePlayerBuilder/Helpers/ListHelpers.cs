using System;
using System.Collections.Generic;

namespace GamePlayerBuilder.Helpers
{
    public static class ListHelpers
    {
        public static IEnumerable<List<T>> SplitList<T>(this List<T> list, int nSize = 100)
        {
            for (int i = 0; i < list.Count; i += nSize)
            {
                yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
            }
        }
    }
}
