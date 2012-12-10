using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework
{
    /// <summary>
    /// Helper class for building param sets
    /// </summary>
    public class Param
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public Param(string name, object value)
        {
            Name = name;
            if (value != null && value.GetType().IsEnum)
            {
                Value = value.ToString();
            }
            else if (value != null && value.GetType().IsArray && value.GetType().GetElementType().IsEnum)
            {
                //Converting array of Enums to array of object (enum string value)
                Value = (value as Array).Cast<object>().Select(e => e.ToString()).ToArray();
            }
            else
            {
                Value = value;
            }
        }
    }
}
