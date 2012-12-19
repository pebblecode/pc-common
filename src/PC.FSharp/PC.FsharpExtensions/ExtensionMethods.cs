using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.FSharp.Collections;
using System.Reflection;
using System.IO;

namespace PebbleCode.Framework
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method to make Microsoft.FSharp.Core.FSharpOption<T>.get_IsSome pretty!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool HasValue<T>(this Microsoft.FSharp.Core.FSharpOption<T> option)
        {
            return Microsoft.FSharp.Core.FSharpOption<T>.get_IsSome(option);
        }


        /// <summary>
        /// Convert to an FSharp list
        /// </summary>
        /// <returns></returns>
        public static FSharpList<Type> ToFSharpList<Type>(this IEnumerable<Type> source)
        {
            FSharpList<Type> result = FSharpList<Type>.Empty;
            foreach (var item in source.Reverse())
            {
                result = new FSharpList<Type>(item, result);
            }
            return result;
        }

        /// <summary>
        /// Convert to an FSharp list
        /// </summary>
        /// <returns></returns>
        public static FSharpList<KeyType> ToFSharpList<KeyType, ValueType>(this Dictionary<KeyType, ValueType>.KeyCollection source)
        {
            FSharpList<KeyType> result = FSharpList<KeyType>.Empty;
            foreach (KeyType key in source)
                result = new FSharpList<KeyType>(key, result);
            return result;
        }

        /// <summary>
        /// Convert to an FSharp list
        /// </summary>
        /// <returns></returns>
        public static FSharpList<ValueType> ToFSharpList<KeyType, ValueType>(this Dictionary<KeyType, ValueType>.ValueCollection source)
        {
            FSharpList<ValueType> result = FSharpList<ValueType>.Empty;
            foreach (ValueType key in source)
                result = new FSharpList<ValueType>(key, result);
            return result;
        }
    }
}