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

using PebbleCode.Framework;
using PebbleCode.Entities;

namespace PebbleCode.Entities.Tests.Integration.Entities
{
	/// <summary>
	/// Generated by MyGeneration using the IBatis Boardbooks BusinessEntityCollection template
	/// </summary>
	[Serializable]
	public partial class WidgetList : EntityList<Widget, int>
	{
		/// <summary>
		/// Construct a WidgetList from an list of type Widget
		/// </summary>
		/// <param name="entries">list of type Widget</param>
		public WidgetList(IEnumerable<Widget> entries) : base(entries) { }

		/// <summary>
		/// Construct a WidgetList from several Widgets
		/// </summary>
		/// <param name="entries">several initial Widgets for the list</param>
		public WidgetList(params Widget[] entries) : base(entries) { }

		/// <summary>
		/// Construct a new WidgetList 
		/// </summary>
		public WidgetList() { }
		
        /// <summary>
		/// Implicit cast to array type 
		/// </summary>
		public static implicit operator Widget[](WidgetList entities)
        {
            return entities.ToArray();
        }
		
        /// <summary>
		/// Implicit cast from array type 
		/// </summary>
		public static implicit operator WidgetList(Widget[] entities)
        {
            return new WidgetList(entities);
        }
		
		/// <summary>
		///	Returns a copy of the instance of Widget
		/// </summary>
		/// <remarks>
		/// Overrides the base version, but calls OnClone to allow base classes chance
		/// to clone their information
		/// </remarks>
		public override object Clone()
        {
			// Create a new list.
			WidgetList clone = new WidgetList();

			// Let the base class clone all of the items in our collection.
			OnClone(clone);
			// N.B. OnClone can be overridden in the handmade part of this
			// partial class to add extra code during clone (but make sure
			// you call the base version to copy the collection)
			
			// Return our pristine clone!
            return clone;			
		}

        /// <summary>
        /// Map out the collection by ThingId
        /// </summary>
        public Dictionary<int, List<Widget>> MapByThingId
        {
            get { return MapByField<int>((entity) => entity.ThingId); }
        }

		/// <summary>
        /// Get Widget from the collection, by thingId
        /// </summary>
        /// <param name="thingId">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public WidgetList FindByThingId(int thingId)
		{
            return new WidgetList(this.FindAll((widget) => widget.ThingId == thingId));
		}
	}
}