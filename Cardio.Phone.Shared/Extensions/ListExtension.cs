using System.Collections.Generic;

namespace Cardio.Phone.Shared.Extensions
{
    public static class ListExtension
    {
        public static void AddRange<TBase, TSubtype>(this IList<TBase> source, IList<TSubtype> elementsToAdd) where TSubtype : TBase
        {
            for (int i = 0; i < source.Count; i++)
            {
                source.Add(elementsToAdd[i]);
            }
        }
    }
}
