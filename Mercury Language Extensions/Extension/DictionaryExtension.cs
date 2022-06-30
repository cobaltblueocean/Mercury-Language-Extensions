using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cern.Colt.Matrix.Implementation;

namespace System.Collections.Generic
{
    public static class DictionaryExtension
    {
        #region Extension for IDictionary<TKey, TValue>
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(originalDictionary);
        }

        public static TValue PutIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, TValue value)
        {
            if (!originalDictionary.ContainsKey(key))
            {
                originalDictionary.Add(key, value);
            }

            return originalDictionary[key];
        }

        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            return Sort(originalDictionary, SortOrder.Asending);
        }

        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, SortOrder order)
        {
            var data = (order == SortOrder.Asending) ? originalDictionary.OrderBy(pair => pair.Key) : originalDictionary.OrderByDescending(pair => pair.Key);
            var buf = new List<KeyValuePair<TKey, TValue>>();

            foreach (var item in data)
            {
                buf.Add(new KeyValuePair<TKey, TValue>(item.Key, item.Value));
            }

            originalDictionary.RemoveAll();
            originalDictionary.Clear();

            originalDictionary.AddOrUpdateAll(buf);

            return originalDictionary;
        }

        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary)
        {
            var keys = originalDictionary.Keys.ToList();
            foreach (var key in keys)
            {
                originalDictionary.Remove(key);
            }
        }

        #endregion
    }
}
