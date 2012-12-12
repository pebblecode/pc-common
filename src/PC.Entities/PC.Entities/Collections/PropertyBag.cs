using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PebbleCode.Framework;
using PebbleCode.Framework.Dates;

namespace PebbleCode.Framework.Collections
{
    /// <summary>
    /// A very simple adapter for a string dictionary, providing some strongly typed
    /// utility accessors
    /// </summary>
    [Serializable]
    public class PropertyBag : Dictionary<string, string>, ICloneable, ISerializable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public PropertyBag()
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PropertyBag(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Shallow clone, works as strings are immutable
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            PropertyBag newPb = new PropertyBag();
            foreach (string key in this.Keys)
            {
                newPb.Add(key, this[key]);
            }
            return newPb;
        }

        /// <summary>
        /// DateTime setter support
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDateTime(string key, DateTime value)
        {
            this.Remove(key);
            this.Add(key, DateUtils.ToYYYYMMDD(value));
        }

        /// <summary>
        /// DateTime access
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string key) { return DateUtils.FromYYYYMMDD(this[key]).Value; }

        /// <summary>
        /// String Setter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetString(string key, string value)
        {
            this.Remove(key);
            this.Add(key, value);
        }

        /// <summary>
        /// Decimal setter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDecimal(string key, decimal value)
        {
            this.Remove(key);
            this.Add(key, value.ToString());
        }

        /// <summary>
        /// Boolean setter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetBoolean(string key, bool value)
        {
            this.Remove(key);
            this.Add(key, value.ToString());
        }

        /// <summary>
        /// String getter
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key) { return this[key]; }

        /// <summary>
        /// Decimal getter
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public decimal GetDecimal(string key) { return decimal.Parse(this[key]); }

        /// <summary>
        /// Boolean getter
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetBoolean(string key) { return bool.Parse(this[key]); }

        /// <summary>
        /// Delegate to base hashcode implementation
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Compare keys and values for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PropertyBag other = obj as PropertyBag;
            if (obj == null || this.Count != other.Count) return false;

            //trye to find a missing key or unequal value
            foreach (string key in this.Keys)
            {
                if (!other.ContainsKey(key)) return false;
                if (!this[key].Equals(other[key])) return false;
            }
            return true;
        }

    }
}
