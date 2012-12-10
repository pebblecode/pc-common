using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace PebbleCode.Framework.Utilities
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Camelises a column_name to standard PropertyName notation
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static string ColumnNameToPropertyName(string colName)
        {
            StringBuilder propName = new StringBuilder();
            string[] tokens = colName.Split('_');
            foreach (string token in tokens)
            {
                propName.Append(char.ToUpper(token[0]));
                propName.Append(token.Substring(1).ToLower());
            }
            return propName.ToString();
        }

        /// <summary>
        /// A utility function for extracting object value of a property - the object
        /// must be non-null and the property must exist.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object  obj, string propName)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propName);
            if (prop == null)
            {
                throw new ArgumentException(string.Format(
                    "The type {0} does not have a property named {1}",
                    obj.GetType(), propName));
            }
            return prop.GetValue(obj, null);
        }

        /// <summary>
        /// A utility function for extracting the decimal value of a property - the object
        /// must be non-null and the property must exist.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static decimal? GetDecimalPropertyValue(object obj, string propName)
        {
            return (decimal?)GetPropertyValue(obj,propName);
        }

        /// <summary>
        /// A utility function for checking the value of a property is empty or not.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static bool PropertyHasValue(object obj, string propName)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propName);
            if (prop == null)
            {
                throw new ArgumentException(string.Format(
                    "The type {0} does not have a property nameed {1}",
                    obj.GetType(), propName));
            }

            return prop.GetValue(obj, null) != null ? true : false;
        }
        
        public static void SetDateTimePropertyValue(object obj, string propertyName, DateTime? value)
        {
            PropertyInfo prop = GetPublicInstanceProperty(obj, propertyName);

            //Be careful to only set the value to null if it is supported
            if (!value.HasValue && !prop.PropertyType.Equals(typeof(DateTime?)))
            {
                //Null value not supported, skip.
                return;
            }

            prop.SetValue(obj, value, null);
        }
        public static void SetBooleanPropertyValue(object obj, string propertyName, bool? value)
        {
            PropertyInfo prop = GetPublicInstanceProperty(obj, propertyName);

            //Be careful to only set the value to null if it is supported
            if (!value.HasValue && !prop.PropertyType.Equals(typeof(DateTime?)))
            {
                //Null value not supported, skip.
                return;
            }

            prop.SetValue(obj, value, null);
        }
        
        /// <summary>
        /// Sets the value of a possibly nullable decimal property.
        /// </summary>
        /// <param name="obj">The object on which the property exists.</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">The value</param>
        public static void SetDecimalPropertyValue(object obj, string propertyName, decimal? value)
        {
            PropertyInfo prop = GetPublicInstanceProperty(obj, propertyName);

            //Be careful to only set the value to null if it is supported
            if (!value.HasValue && ! prop.PropertyType.Equals(typeof(decimal?)))
            {
                //Null value not supported, skip.
                return;
            }

            prop.SetValue(obj, value, null);
        }

        /// <summary>
        /// Sets the value of a public instance property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            PropertyInfo prop = GetPublicInstanceProperty(obj, propertyName);
            prop.SetValue(obj, value, null);
        }

        /// <summary>
        /// Returns the public instance property matching this name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetPublicInstanceProperty(object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo prop = type.GetProperty(propertyName, 
                BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
            {
                throw new ArgumentException(string.Format(
                    "The type {0} does not have a property named {1}",
                    obj.GetType(), propertyName));
            }
            return prop;
        }

        /// <summary>
        /// Returns the public instance method matching this name.  Doesn't work so well when 
        /// there are multiple matches
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodInfo GetPublicInstanceMethod(object obj, string methodName)
        {
            MethodInfo method = obj.GetType().GetMethod(methodName, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
            {
                throw new ArgumentException(string.Format(
                    "The type {0} does not have a method nameed {1}",
                    obj.GetType(), methodName));
            }
            return method;
        }

        /// <summary>
        /// Returns the static method identified by methodName
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodInfo GetPublicStaticMethod(Type type, string methodName)
        {
            MethodInfo method = type.GetMethod(methodName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            if (method == null)
            {
                throw new ArgumentException(string.Format(
                    "The type {0} does not have a method nameed {1}",
                    type.Name, methodName));
            }
            return method;
        }

        /// <summary>
        /// Useful when constructing an object using the Activator, when you already
        /// have the object[] of parameters, but need array of Type[] to select
        /// a constructor
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Type[] ParametersToTypeArray(object[] parameters)
        {
            return parameters.Select(o => o.GetType()).ToArray();
        }

        /// <summary>
        /// Look up the property type on a specific object.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        private static Type GetMemberType(Type t, string memberName)
        {
            memberName = memberName.ToLower();
            foreach (PropertyInfo property in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.Name.ToLower().Equals(memberName))
                {
                    return property.PropertyType;
                }
            }
            throw new Exception(string.Format("Member {0} not found.", memberName));
        }

        /// <summary>
        /// Returns true if the calling code originates in a visual studio test container
        /// </summary>
        /// <returns></returns>
        public static bool InUnitTest
        {
            get
            {
                StackTrace st = new StackTrace();
                foreach (StackFrame frame in st.GetFrames())
                {
                    MethodBase caller = frame.GetMethod();
                    if (caller.DeclaringType.FullName.Contains("Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestExecuter"))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
