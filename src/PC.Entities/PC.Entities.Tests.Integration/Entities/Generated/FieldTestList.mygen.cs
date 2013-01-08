/*****************************************************************************/
/***                                                                       ***/
/***    This is an automatically generated file. It is generated using     ***/
/***    MyGeneration in conjunction with IBatisBusinessObject template.    ***/
/***                                                                       ***/
/***    DO NOT MODIFY THIS FILE DIRECTLY!                                  ***/
/***                                                                       ***/
/***    If you need to make changes either modify the template and         ***/
/***    regenerate or derive a class from this class and override.         ***/
/***                                                                       ***/
/*****************************************************************************/

using System;
using System.Collections.Generic;
using PebbleCode.Entities;

namespace PC.Entities.Tests.Integration.Entities
{
	/// <summary>
	/// Generated by MyGeneration using the IBatis Boardbooks BusinessEntityCollection template
	/// </summary>
	[Serializable]
	public partial class FieldTestList : EntityList<FieldTest>
	{
		/// <summary>
		/// Construct a FieldTestList from an list of type FieldTest
		/// </summary>
		/// <param name="entries">list of type FieldTest</param>
		public FieldTestList(IEnumerable<FieldTest> entries) : base(entries) { }

		/// <summary>
		/// Construct a FieldTestList from several FieldTests
		/// </summary>
		/// <param name="entries">several initial FieldTests for the list</param>
		public FieldTestList(params FieldTest[] entries) : base(entries) { }

		/// <summary>
		/// Construct a new FieldTestList 
		/// </summary>
		public FieldTestList() { }
		
        /// <summary>
		/// Implicit cast to array type 
		/// </summary>
		public static implicit operator FieldTest[](FieldTestList entities)
        {
            return entities.ToArray();
        }
		
        /// <summary>
		/// Implicit cast from array type 
		/// </summary>
		public static implicit operator FieldTestList(FieldTest[] entities)
        {
            return new FieldTestList(entities);
        }
		
		/// <summary>
		///	Returns a copy of the instance of FieldTest
		/// </summary>
		/// <remarks>
		/// Overrides the base version, but calls OnClone to allow base classes chance
		/// to clone their information
		/// </remarks>
		public override object Clone()
        {
			// Create a new list.
			FieldTestList clone = new FieldTestList();

			// Let the base class clone all of the items in our collection.
			OnClone(clone);
			// N.B. OnClone can be overridden in the handmade part of this
			// partial class to add extra code during clone (but make sure
			// you call the base version to copy the collection)
			
			// Return our pristine clone!
            return clone;			
		}

        /// <summary>
        /// Map out the collection by ForeignKeyField
        /// </summary>
        public Dictionary<int, List<FieldTest>> MapByForeignKeyField
        {
            get { return MapByField<int>((entity) => entity.ForeignKeyField); }
        }

        /// <summary>
        /// Map out the collection by ForeignKeyFieldNullable
        /// </summary>
        public Dictionary<int?, List<FieldTest>> MapByForeignKeyFieldNullable
        {
            get { return MapByField<int?>((entity) => entity.ForeignKeyFieldNullable); }
        }

		/// <summary>
        /// Get FieldTest from the collection, by foreignKeyField
        /// </summary>
        /// <param name="foreignKeyField">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public FieldTestList FindByForeignKeyField(int foreignKeyField)
		{
            return new FieldTestList(this.FindAll((fieldTest) => fieldTest.ForeignKeyField == foreignKeyField));
		}

		/// <summary>
        /// Get FieldTest from the collection, by foreignKeyFieldNullable
        /// </summary>
        /// <param name="foreignKeyFieldNullable">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public FieldTestList FindByForeignKeyFieldNullable(int? foreignKeyFieldNullable)
		{
            return new FieldTestList(this.FindAll((fieldTest) => fieldTest.ForeignKeyFieldNullable == foreignKeyFieldNullable));
		}
	}
}