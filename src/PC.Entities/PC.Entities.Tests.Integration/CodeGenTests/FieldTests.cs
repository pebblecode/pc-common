using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using PebbleCode.Entities.Tests.Integration.Entities;
using PebbleCode.Framework.Collections;

namespace PebbleCode.Tests.Unit.CodeGenTests
{
    [TestFixture]
    public class FieldTests
    {
        [Test]
        public void CheckFieldTypes()
        {
            Type fieldTestType = typeof(FieldTests);

            // Check the types of the gen'd code
            Assert.AreEqual(typeof(DateTime), GetPropertyType((FieldTest f) => f.DatetimeField));
            Assert.AreEqual(typeof(DateTime?), GetPropertyType((FieldTest f) => f.DatetimeFieldNullable));
            Assert.AreEqual(typeof(decimal), GetPropertyType((FieldTest f) => f.DecimalField));
            Assert.AreEqual(typeof(decimal?), GetPropertyType((FieldTest f) => f.DecimalFieldNullable));
            Assert.AreEqual(typeof(Flags), GetPropertyType((FieldTest f) => f.EntityFlag));
            Assert.AreEqual(typeof(TestEnum), GetPropertyType((FieldTest f) => f.EnumField));
            Assert.AreEqual(typeof(TestEnum?), GetPropertyType((FieldTest f) => f.EnumFieldNullable));
            Assert.AreEqual(typeof(int), GetPropertyType((FieldTest f) => f.ForeignKeyField));
            Assert.AreEqual(typeof(int?), GetPropertyType((FieldTest f) => f.ForeignKeyFieldNullable));
            Assert.AreEqual(typeof(Widget), GetPropertyType((FieldTest f) => f.ForeignKeyFieldNullableWidget));
            Assert.AreEqual(typeof(Widget), GetPropertyType((FieldTest f) => f.ForeignKeyFieldWidget));
            Assert.AreEqual(typeof(DateTime), GetPropertyType((FieldTest f) => f.IntDateField));
            Assert.AreEqual(typeof(DateTime?), GetPropertyType((FieldTest f) => f.IntDateFieldNullable));
            Assert.AreEqual(typeof(int), GetPropertyType((FieldTest f) => f.IntField));
            Assert.AreEqual(typeof(int?), GetPropertyType((FieldTest f) => f.IntFieldNullable));
            Assert.AreEqual(typeof(string), GetPropertyType((FieldTest f) => f.NodeIdField));
            Assert.AreEqual(typeof(string), GetPropertyType((FieldTest f) => f.NodeIdFieldNullable));
            Assert.AreEqual(typeof(PropertyBag), GetPropertyType((FieldTest f) => f.ObjectField));
            Assert.AreEqual(typeof(PropertyBag), GetPropertyType((FieldTest f) => f.ObjectFieldNullable));
            Assert.AreEqual(typeof(string), GetPropertyType((FieldTest f) => f.StringField));
            Assert.AreEqual(typeof(string), GetPropertyType((FieldTest f) => f.StringFieldNullable));
            Assert.AreEqual(typeof(string), GetPropertyType((FieldTest f) => f.TextField));
            Assert.AreEqual(typeof(string), GetPropertyType((FieldTest f) => f.TextFieldNullable));
            Assert.AreEqual(typeof(byte[]), GetPropertyType((FieldTest f) => f.TimestampField));
            Assert.AreEqual(typeof(int), GetPropertyType((FieldTest f) => f.TinyintField));
            Assert.AreEqual(typeof(int?), GetPropertyType((FieldTest f) => f.TinyintFieldNullable));
        }

        private Type GetPropertyType<T, P>(Expression<Func<T, P>> action)
        {
            return GetProperty(action).PropertyType;
        }

        /// <summary>
        /// Get the property for a lamdba
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        private PropertyInfo GetProperty<T, P>(Expression<Func<T, P>> action)
        {
            PropertyInfo propertyInfo = (action.Body as MemberExpression).Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            return propertyInfo;
        }
    }
}
