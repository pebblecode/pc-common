using System;
using System.Collections.Generic;
using System.Text;

namespace PebbleCode.Framework.Utilities
{
    /// <summary>
    /// Array "extension" methods.
    /// </summary>
    public static class ArrayExt
    {
        /// <summary>
        /// Does the haystack contain the needle?
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="needle"></param>
        /// <returns></returns>
        public static bool Contains<Type>(Type[] haystack, Type needle)
            where Type : IComparable
        {
            if (haystack == null)
                return false;

            for (int i = 0; i < haystack.Length; i++)
            {
                if (haystack[i].CompareTo(needle) == 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Find an element at an index
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T ElementAt<T>(LinkedList<T> source, int index)
        {
            LinkedListNode<T> node = source.First;
            for (int i = 0; i < index; i++)
            {
                if (node == null)
                    throw new IndexOutOfRangeException();
                node = node.Next;
            }

            return node.Value;
        }

        /// <summary>
        /// Convert a collection to an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T[] ToArray<T>(ICollection<T> collection)
        {
            T[] result = new T[collection.Count];
            int index = 0;
            foreach (T item in collection)
                result[index++] = item;
            return result;
        }

        /// <summary>
        /// Get first item in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T First<T>(ICollection<T> collection)
        {
            foreach (T item in collection)
                return item;
            return default(T);
        }
    }
}
