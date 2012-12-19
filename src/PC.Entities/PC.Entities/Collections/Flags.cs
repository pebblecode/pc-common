using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace PebbleCode.Framework.Collections
{
    /// <summary>
    /// An immutable type that acts like a flags enum
    /// </summary>
    [Serializable]
    public sealed class Flags : ICloneable
    {
        /// <summary>
        /// The Flags in this flag set
        /// </summary>
        private int[] _flags;

        /// <summary>
        /// Create a new flag set from one or more flags
        /// </summary>
        /// <param name="flags"></param>
        public Flags(params int[] flags)
        {
            if (flags == null)
                _flags = new int[] { };
            else if (ContainsDuplicates(flags))
                _flags = CopyAndRemoveDuplicates(flags);
            else
                _flags = (int[])flags.Clone();
        }

        /// <summary>
        /// Create a new flag set from a list of flags
        /// </summary>
        /// <param name="flags"></param>
        public Flags(IEnumerable<int> flags)
        {
            if (flags == null)
                _flags = new int[] { };
            else if (ContainsDuplicates(flags))
                _flags = CopyAndRemoveDuplicates(flags);
            else
                _flags = flags.ToArray();
        }

        /// <summary>
        /// Check to see if the list contains duplicates and remove them if it does
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags"></param>
        /// <returns></returns>
        private static bool ContainsDuplicates(IEnumerable<int> flags)
        {
            int count = flags.Count();
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (flags.ElementAt(i) == flags.ElementAt(j))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Copy the source and remove any duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags"></param>
        /// <returns></returns>
        private static int[] CopyAndRemoveDuplicates(IEnumerable<int> flags)
        {
            List<int> copy = new List<int>(flags);
            for (int i = 0; i < copy.Count(); i++)
            {
                for (int j = i + 1; j < copy.Count(); j++)
                {
                    if (copy[i] == copy[j])
                    {
                        copy.RemoveAt(j);
                        j--;
                    }
                }
            }
            return copy.ToArray();
        }

        /// <summary>
        /// Clone the flags
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Flags(_flags);
        }

        /// <summary>
        /// Get the empty flag set
        /// </summary>
        public static Flags None = new Flags();

        /// <summary>
        /// Is this flags enum empty?
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty
        {
            get { return _flags.Length == 0; }
        }

        /// <summary>
        /// How many flags?
        /// </summary>
        /// <returns></returns>
        public int Length
        {
            get { return _flags.Length; }
        }

        /// <summary>
        /// Split the flag into separate flags
        /// </summary>
        /// <returns></returns>
        public List<Flags> Split()
        {
            List<Flags> result = new List<Flags>();
            foreach (int flag in _flags)
            {
                result.Add(new Flags(flag));
            }
            return result;
        }

        /// <summary>
        /// Get all the values
        /// </summary>
        public int[] Values
        {
            get { return (int[])_flags.Clone(); }
        }

        public int SingleValue
        {
            get
            {
                if (this._flags.Length != 1)
                    throw new InvalidOperationException("Cannot return a single flag value if there is not exactly 1 flag set");
                return this._flags[0];
            }
        }

        /// <summary>
        /// Does this flags array contain all of the specified flags?
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Contains(params int[] flags)
        {
            if (flags == null) throw new NullReferenceException("flags is null");
            bool contains = true;
            foreach (int flag in flags)
            {
                contains &= _flags.Contains(flag);
            }
            return contains;
        }

        /// <summary>
        /// Remove all supplied flags from the list of flags
        /// </summary>
        /// <param name="flagsToRemove"></param>
        /// <returns></returns>
        public Flags Remove(Flags flagsToRemove)
        {
            if (flagsToRemove == null) throw new NullReferenceException("flagsToRemove is null");

            // Simple scenario
            if (flagsToRemove._flags.Length == 0) return new Flags(_flags);

            // Otheriwse we have to check for each one
            List<int> result = new List<int>(_flags);
            foreach (int flag in flagsToRemove._flags)
                result.Remove(flag);

            return new Flags(result);
        }

        /// <summary>
        /// Equivalent of a flags enum bitwise OR
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Flags operator |(Flags left, Flags right) 
        {
            if (left == null) throw new NullReferenceException("left is null");
            if (right == null) throw new NullReferenceException("right is null");

            // Simple scenario
            if (left._flags.Length == 0) return new Flags(right._flags);
            if (right._flags.Length == 0) return new Flags(left._flags);

            // Neither is empty, have to compare values
            List<int> result = new List<int>(left._flags);
            foreach (int flag2 in right._flags)
            {
                if (!result.Contains(flag2))
                    result.Add(flag2);
            }

            return new Flags(result);
        }

        /// <summary>
        /// Equivalent of a flags enum bitwise XOR
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Flags operator ^(Flags left, Flags right)
        {
            if (left == null) throw new NullReferenceException("left is null");
            if (right == null) throw new NullReferenceException("right is null");

            // Simple scenario
            if (left._flags.Length == 0) return new Flags(right._flags);
            if (right._flags.Length == 0) return new Flags(left._flags);

            // Neither is empty, have to compare values
            // Start with left, and remove any that also occur in right.
            // Add any that don't occur in right
            List<int> result = new List<int>(left._flags);
            foreach (int flag2 in right._flags)
            {
                if (result.Contains(flag2))
                    result.Remove(flag2);
                else
                    result.Add(flag2);
            }

            return new Flags(result);
        }

        /// <summary>
        /// Equivalent of a flags enum bitwise AND
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Flags operator &(Flags left, Flags right)
        {
            if (left == null) throw new NullReferenceException("left is null");
            if (right == null) throw new NullReferenceException("right is null");

            // Simple scenario
            if (left._flags.Length == 0) return new Flags();
            if (right._flags.Length == 0) return new Flags();

            // Neither is empty, have to compare values
            List<int> result = new List<int>();
            foreach (int flag1 in left._flags)
            {
                if (right._flags.Contains(flag1))
                    result.Add(flag1);
            }

            return new Flags(result);
        }

        /// <summary>
        /// Return a comma separated string of values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(",", _flags);
        }

        #region Equality

        /// <summary>
        /// Check for value equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
                return false;

            // If parameter cannot be cast to Flags return false.
            Flags ef = obj as Flags;
            if ((System.Object)ef == null)
            {
                return false;
            }

            // Default to equality operator (see below)
            return (ef == this);
        }

        /// <summary>
        /// Get a unique hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // Sort the ints into ascending order, convert to a , sep'd
            // string and then get the hash code of the string
            return string.Join(",", _flags.OrderBy(i => i).ToArray()).GetHashCode();
        }

        /// <summary>
        /// Best practice to implement this method when implementing Equals
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Equals(Flags ef)
        {
            // If parameter is null return false.
            if ((System.Object)ef == null)
                return false;

            // Default to equality operator (see below)
            return (ef == this);
        }

        /// <summary>
        /// Overload of == operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Flags a, Flags b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            // N.B. Cast to object or we'll stack overflow
            if (((System.Object)a == null) || ((System.Object)b == null))
                return false;

            // Compare the arrays
            if (a._flags.Length != b._flags.Length)
                return false;
            
            // Same length, so just check each thing from A is in B
            foreach (int value in a._flags)
            {
                if (!b._flags.Contains(value))
                    return false;
            }

            // Must all have existed
            return true;
        }

        /// <summary>
        /// Overload of != operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Flags a, Flags b)
        {
            return !(a == b);
        }

        #endregion

        public static Dictionary<int, string> ReadFlags(Type flagsType)
        {
            var names = new Dictionary<int, string>();

            // Grab all public EntityFlag types
            FieldInfo[] fields = flagsType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                // Is it a Flags field?
                if (field.FieldType == typeof(Flags))
                {
                    // Get its value
                    Flags flags = (Flags)field.GetValue(null);
                    if (flags.Length == 1)
                    {
                        names.Add(flags.Values[0], field.Name);
                    }
                }
            }
            return names;
        }

        public static Flags FromName(string name, Dictionary<int, string> allFlags)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Flags.None;

            if (!name.Contains(','))
            {
                foreach (KeyValuePair<int, string> pair in allFlags)
                {
                    if (pair.Value.ToLower() == name.ToLower())
                        return new Flags(pair.Key);
                }
                throw new KeyNotFoundException("The flag with name {0} was not found.".fmt(name));
            }
            else
            {
                var names = name.Split(',').Select(n => n.Trim().ToLower()).ToList();
                var flags = allFlags.Where(n => names.Contains(n.Value.ToLower())).Select(n => n.Key);
                if (flags.Count() != names.Count)
                    throw new KeyNotFoundException("One of the specified flag names was not found.");

                if (flags.Any())
                    return new Flags(flags);
                else
                    return Flags.None;
            }
        }

        public static string ToName(Flags flags, Dictionary<int, string> allFlags)
        {
            if (flags.IsEmpty)
                return null;

            else if (flags.Length == 1)
                return allFlags[flags.Values[0]];

            else
                return String.Join(", ", flags.Values.Select(val => allFlags[val]));
        }
    }
}

