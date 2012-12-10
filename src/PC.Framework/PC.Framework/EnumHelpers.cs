using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PebbleCode.Framework
{
    public static class EnumHelpers
    {
        public static string ReadableName(this Enum value)
        {
            string readableName = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            ReadableNameAttribute[] attrs = fi.GetCustomAttributes(typeof(ReadableNameAttribute), false) as ReadableNameAttribute[];
            if (attrs.Length > 0)
            {
                readableName = attrs[0].Name;
            }

            return readableName ?? ToReadableName(value.ToString());
        }

        internal static string ToReadableName(string pascalCasedString)
        {
            return Regex.Replace(
                Regex.Replace(
                    pascalCasedString,
                    @"[a-z][A-Z]+",
                    m => string.Concat(m.Value[0], ' ', m.Value.Substring(1))),
                @"[A-Z]{2,}[a-z]",
                m => string.Concat(m.Value.Substring(0, m.Length - 2), ' ', m.Value.Substring(m.Length - 2)));
        }
    }

    [AttributeUsage(AttributeTargets.Enum|AttributeTargets.Field, AllowMultiple = false)]
    public class ReadableNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public ReadableNameAttribute(string readableName)
        {
            Name = readableName;
        }
    }
}
