using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Utilities
{
    public static class ComparisonUtils
    {
        /// <summary>
        /// Check if a value has changed, including null checking
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool HasValueChanged(object oldValue, object newValue)
        {
            // If they're both null, they're the same
            if (oldValue == null && newValue == null)
                return false;

            // If just one is null, or they're not equal, then the value has changed
            else if (oldValue == null || newValue == null || !oldValue.Equals(newValue))
                return true;

            // So must be equal values
            else
                return false;
        }

        /// <summary>
        /// Compare two lists and their elements
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static int CompareLists<ListType>(List<ListType> list1, List<ListType> list2)
            where ListType : IComparable
        {
            if (list1 == null && list2 == null)
                return 0;
            else if (list1 == null && list2 != null)
                return -1;
            else if (list1 != null && list2 == null)
                return 1;
            else if (list1.Count != list2.Count)
                return list1.Count.CompareTo(list2.Count);
            else
            {
                // Compare item by item
                for (int index = 0; index < list1.Count; index++)
                {
                    int compareResult = list1[index].CompareTo(list2[index]);
                    if (compareResult != 0)
                        return compareResult;
                }

                // Its a match!
                return 0;
            }
        }

        /// <summary>
        /// Compare two lists and their elements
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static int CompareNullables<NullableType>(Nullable<NullableType> nullable1, Nullable<NullableType> nullable2)
            where NullableType : struct, IComparable
        {
            if (nullable1 == null && nullable2 == null)
                return 0;
            else if (nullable1 == null && nullable2 != null)
                return -1;
            else if (nullable1 != null && nullable2 == null)
                return 1;
            else
                return nullable1.Value.CompareTo(nullable2.Value);
        }

        /// <summary>
        /// Compare two lists and their elements
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool EquateLists(IList list1, IList list2)
        {
            if (list1 == null && list2 == null)
                return true;
            else if (list1 == null && list2 != null)
                return false;
            else if (list1 != null && list2 == null)
                return false;
            else if (list1.Count != list2.Count)
                return false;
            else
            {
                // Compare item by item
                for (int index = 0; index < list1.Count; index++)
                {
                    if (!list1[index].Equals(list2[index]))
                        return false;
                }

                // Its a match!
                return true;
            }
        }

        /// <summary>
        /// Compare two given comparables (either or both can be null)
        /// </summary>
        /// <param name="comparable1"></param>
        /// <param name="comparable2"></param>
        /// <returns></returns>
        public static int CompareComparables(IComparable comparable1, IComparable comparable2)
        {
            if (comparable1 == null && comparable2 == null)
                return 0;
            else if (comparable1 == null && comparable2 != null)
                return -1;
            else if (comparable1 != null && comparable2 == null)
                return 1;
            else
                return comparable1.CompareTo(comparable2);
        }

        /// <summary>
        /// Compare 2D byte arrays to make sure they are equal
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static int CompareData(byte[,] array1, byte[,] array2)
        {
            if (array1 == null && array2 == null)
                return 0;
            else if (array1 == null && array2 != null)
                return -1;
            else if (array1 != null && array2 == null)
                return 1;
            else
            {
                if (array1.GetLength(0) != array2.GetLength(0))
                    return array1.GetLength(0).CompareTo(array2.GetLength(0));

                if (array1.GetLength(1) != array2.GetLength(1))
                    return array1.GetLength(1).CompareTo(array2.GetLength(1));

                for (int rowIndex = 0; rowIndex < array1.GetLength(0); rowIndex++)
                {
                    for (int colIndex = 0; colIndex < array1.GetLength(1); colIndex++)
                    {
                        if (array1[rowIndex, colIndex] != array2[rowIndex, colIndex])
                            return array1[rowIndex, colIndex].CompareTo(array2[rowIndex, colIndex]);
                    }
                }

                // Its a match!
                return 0;
            }
        }

        /// <summary>
        /// Compare 2 bitmaps
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static int CompareBitmaps(bool[] array1, bool[] array2)
        {
            if (array1 == null && array2 == null)
                return 0;
            else if (array1 == null && array2 != null)
                return -1;
            else if (array1 != null && array2 == null)
                return 1;
            else
            {
                if (array1.Length != array2.Length)
                    return array1.Length.CompareTo(array2.Length);

                for (int index = 0; index < array1.Length; index++)
                {
                    if (array1[index] != array2[index])
                        return array1[index].CompareTo(array2[index]);
                }

                // Its a match!
                return 0;
            }
        }

        /// <summary>
        /// Compare two byte arrays to make sure they are equal
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static int CompareData(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
                return 0;
            else if (array1 == null && array2 != null)
                return -1;
            else if (array1 != null && array2 == null)
                return 1;
            else
            {
                if (array1.Length != array2.Length)
                    return array1.Length.CompareTo(array2.Length);

                for (int index = 0; index < array1.Length; index++)
                {
                    if (array1[index] != array2[index])
                        return array1[index].CompareTo(array2[index]);
                }

                // Its a match!
                return 0;
            }
        }
    }
}
