using System;
using System.Collections.Generic;

namespace Util
{
    public static class CollectionExtensions
    {
        public static T FirstFast<T>(this IEnumerable<T> col, Func<T, bool> func)
        {
            foreach(var item in col)
            {
                if(func(item))
                    return item;
            }

            return default;
        }

        public static T FirstFast<T>(this IList<T> list, Func<T, bool> func)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(func(list[i]))
                    return list[i];
            }

            return default;
        }

        public static T FirstFast<T>(this T[] arr, Func<T, bool> func)
        {
            for(int i = 0; i < arr.Length; i++)
            {
                if(func(arr[i]))
                    return arr[i];
            }

            return default;
        }
    }
}