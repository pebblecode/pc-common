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
using System.Linq;
using System.Collections.Generic;

using IBatisNet.DataMapper;

using PebbleCode.Entities;
using PebbleCode.Entities.Tests.Integration.Repository;
using PebbleCode.Framework.Collections;
using PebbleCode.Framework.Dates;
using PebbleCode.Framework.IoC;
using PebbleCode.Repository;
using PebbleCode.Repository.Exceptions;

using PebbleCode.Entities.Tests.Integration.Entities;

namespace PC.Entities.Tests.Integration.Repository
{
	/// <summary>
	/// Add support for this repo to the global DB context accessor
	/// </summary>
    public partial class DbContext
    {
        [Inject]
        public WidgetRepository WidgetRepo { get; set; }
    }
	
	/// <summary>
	/// Provides access to the Widget Repository
	/// </summary>
	public partial class WidgetRepository : EditableEntityRepository<Widget, WidgetList, int>
	{
		/// <summary>
		/// Get all the instances of Widget from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override WidgetList GetAll()
		{
			return GetAll(EntityType.None);
		}
		
		/// <summary>
		/// Get all the instances of Widget from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override WidgetList GetAll(Flags toPopulate)
		{	
            Log("GetAll", toPopulate);
			WidgetList result = (WidgetList)this.Mapper.QueryWithRowDelegate<Widget>("SelectWidget", null, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get several instances of Widget from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override WidgetList Get(int[] ids)
		{
			return Get(ids, EntityType.None);
		}

		/// <summary>
		/// Get several instances of Widget from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override WidgetList Get(int[] ids, Flags toPopulate)
		{
			if (ids == null) throw new ArgumentNullException("ids");
			Log("Get(ids)", ids, toPopulate);
			if (ids.Length == 0) return new WidgetList();
			WidgetList result = (WidgetList)this.Mapper.QueryWithRowDelegate<Widget>("SelectWidgets", ids, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get an instance of Widget from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override Widget Get(int id)
		{
			return Get(id, EntityType.None);
		}

		/// <summary>
		/// Get an instance of Widget from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override Widget Get(int id, Flags toPopulate)
		{
            Log("Get(id)", id, toPopulate);
			Widget widget = this.Mapper.QueryForObject<Widget>("SelectWidget", id);
			if (widget != null)
			{
				OnAfterLoadEntity(widget);
				Populate(widget, toPopulate);
			}
			return widget;
		}

		/// <summary>
        /// Get Widget from the store, by thingId
        /// </summary>
        /// <param name="thingId">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public virtual WidgetList GetByThingId(int thingId)
		{
			return GetByThingId(thingId, EntityType.None);
		}

		/// <summary>
        /// Get Widget from the store, by thingId
        /// </summary>
        /// <param name="thingId">Search value</param>
        /// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with the given search value</returns>
        public virtual WidgetList GetByThingId(int thingId, Flags toPopulate)
		{
            Log("GetByThingId", toPopulate);
			WidgetList result = (WidgetList)this.Mapper.QueryWithRowDelegate<Widget>("SelectWidgetByThingId", thingId, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get entities from the database, by multiple thingIds
        /// </summary>
        /// <param name="thingIds"></param>
        /// <returns>All entities with one of the given search values</returns>
        public virtual WidgetList GetByThingId(int[] thingIds)
		{
			return GetByThingId(thingIds, EntityType.None);
		}

		/// <summary>
		/// Get entities from the database, by multiple thingIds
        /// </summary>
        /// <param name="thingIds">Search values</param>
		/// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with one of the given search values</returns>
        public virtual WidgetList GetByThingId(int[] thingIds, Flags toPopulate)
		{
            Log("GetByThingId", toPopulate);
			WidgetList result = (WidgetList)this.Mapper.QueryWithRowDelegate<Widget>("SelectWidgetByThingIds", thingIds, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
        /// Delete Widget from the store, by thingId.
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="thingId">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public virtual void DeleteByThingId(int thingId)
		{
            Log("DeleteByThingId");
			int deleted = this.Mapper.Delete("DeleteWidgetByThingId", thingId);
			if (deleted > 0) RaiseModelChanged();
		}

		/// <summary>
		/// Delete entities from the database, by multiple thingIds
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="thingIds">Search values</param>
        public virtual void DeleteByThingId(int[] thingIds)
		{
            Log("DeleteByThingId");
			int deleted = this.Mapper.Delete("DeleteWidgetByThingIds", thingIds);
			if (deleted > 0) RaiseModelChanged();
		}
  
		/// <summary>
		/// Delete a Widget from the store.
		/// Does NOT delete references. Make sure all sub entities deleted first.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
        public virtual void Delete(int id)
		{
            Log("Delete", id, EntityType.None);
			if (this.Mapper.Delete("DeleteWidget", id) != 1)
				ThrowWidgetEntityException(id);
			RaiseModelChanged();
		}
		
		/// <summary>
		/// Delete a Widget from the store
		/// </summary>
        /// <param name="widget">The entity to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, Widget widget)
		{
			Delete(new List<Entity<int>>(), toDelete, new List<Widget>{ widget });
		}

		/// <summary>
		/// Delete multiple Widget entities from the store.
		/// </summary>
        /// <param name="widgets">The entities to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, IEnumerable<Widget> widgets)
		{
			Delete(new List<Entity<int>>(), toDelete, widgets);
		}
		
		/// <summary>
		/// Delete Widget entities from the store, including sub entites marked for deletion
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being deleted further up the delete stack</param>
		/// <param name="widgets">The Widgets to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
		internal void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, IEnumerable<Widget> widgets)
		{
            if (widgets == null)
				throw new ArgumentNullException("widgets");
			Log("Delete", widgets.Select<Widget, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(widgets);

			// Loop over each entity and delete it.
			foreach (Widget widget in widgets)
			{
                // Already being deleted higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(widget))
                    continue;
					
                //Allow partial/subclasses to perform additional processing
                OnBeforeDeleteEntity(widget);

				// Delete child entities
				if ((toDelete & EntityType.FieldTest) == EntityType.FieldTest
					&& widget.FieldTestListUsingForeignKeyFieldPopulated)
				{
					foreach (FieldTest childFieldTest in widget.FieldTestListUsingForeignKeyField)
					{
						if (!entitiesBeingHandled.ContainsEntity(childFieldTest))
							FieldTestRepo.Delete(entitiesNowBeingHandled, toDelete, childFieldTest);
					}
				}

				// Delete child entities
				if ((toDelete & EntityType.FieldTest) == EntityType.FieldTest
					&& widget.FieldTestListUsingForeignKeyFieldNullablePopulated)
				{
					foreach (FieldTest childFieldTest in widget.FieldTestListUsingForeignKeyFieldNullable)
					{
						if (!entitiesBeingHandled.ContainsEntity(childFieldTest))
							FieldTestRepo.Delete(entitiesNowBeingHandled, toDelete, childFieldTest);
					}
				}

				// Now delete the entity
				if (this.Mapper.Delete("DeleteWidget", widget.Identity) != 1)
					ThrowWidgetEntityException(widget.Identity);
				widget.ResetChanged();                 
				//Allow partial/subclasses to perform additional processing
				OnAfterDeleteEntity(widget);
			}
			
			//Save to the repository updates table
			if (widgets.Count() > 0)
				RaiseModelChanged();
		}
		
		/// <summary>
		/// Delete all entries in this table - use with care!
		/// Does NOT delete references. Make sure all sub entities deleted first.
		/// Also note that events and OnAfter events will not be called for each valuation
		/// </summary>
		public override void DeleteAll()
		{
            Log("DeleteAll", EntityType.None);
			int deleted = this.Mapper.Delete("DeleteAllWidget", null);
			
			//Save to the repository updates table
			if (deleted > 0)
				RaiseModelChanged();
		}

		/// <summary>
		/// Save (insert/update) a Widget into the store
		/// </summary>
		/// <param name="widgets">The Widgets to save</param>
		public override void Save(params Widget[] widgets)
		{
            Save(EntityType.None, widgets);
		}

		/// <summary>
		/// Save (insert/update) a Widget into the store
		/// </summary>
		/// <param name="widgets">The Widgets to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, params Widget[] widgets)
		{
            Save(new List<Entity<int>>(), toSave, widgets);
		}

		/// <summary>
		/// Save (insert/update) a Widget into the store
		/// </summary>
		/// <param name="widgetList">The Widgets to save</param>
		public override void Save(WidgetList widgetList)
		{
            Save(EntityType.None, widgetList);
		}

		/// <summary>
		/// Save (insert/update) a Widget into the store
		/// </summary>
		/// <param name="widgetList">The Widgets to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, WidgetList widgetList)
		{
            Save(new List<Entity<int>>(), toSave, widgetList.ToArray());
		}

		/// <summary>
		/// Save (insert/update) a Widget into the store
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being saved further up the save stack</param>
		/// <param name="widgets">The Widgets to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		internal void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params Widget[] widgets)
		{
            if (widgets == null)
				throw new ArgumentNullException("widgets");
			Log("Save", widgets.Select<Widget, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(widgets);
			
			// Loop over each entity and save it.
			foreach (Widget widget in widgets)
			{
				
			
                // Already being saved higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(widget))
                    continue;
					
				// Allow derived/partial class to do extra work
				OnBeforeSaveEntity(widget);
				
				// Save parent entity... so we can update our key to it
				if ((toSave & EntityType.Thing) == EntityType.Thing
					&& widget.ThingPopulated)
				{
					if (!entitiesBeingHandled.ContainsEntity(widget.Thing))
					{
						ThingRepo.Save(entitiesNowBeingHandled, toSave, widget.Thing);
						widget.ThingId = widget.Thing.Identity;
					}
				}
				
				bool saved = false;
				
				try
				{
					// Save the entity
					if (widget.IsNew)
					{
						this.Mapper.Insert("InsertWidget", widget);
						saved = true;
					}
					else if (widget.IsChanged)
					{
						if (this.Mapper.Update("UpdateWidget", widget) != 1)
							ThrowWidgetEntityException(widget.Identity);
						saved = true;
					}
				}
				catch (Exception ex)
				{
					throw EntityLogger<int>.WriteUnexpectedException(
						ex, 
						"Failed to insert/update Entity",
						Category.EntityFramework,
						widget);
				}
				
				// Save child entities... update their key to us
				if ((toSave & EntityType.FieldTest) == EntityType.FieldTest
					&& widget.FieldTestListUsingForeignKeyFieldPopulated)
				{
					foreach (FieldTest childFieldTest in widget.FieldTestListUsingForeignKeyField)
					{
						childFieldTest.ForeignKeyField = widget.Identity;
						if (!entitiesBeingHandled.ContainsEntity(childFieldTest))
						{
                            if (childFieldTest.IsDeleted)
                                FieldTestRepo.Delete(entitiesNowBeingHandled, toSave, childFieldTest);
                            else
								FieldTestRepo.Save(entitiesNowBeingHandled, toSave, childFieldTest);
						}
					}
				}
				
				// Save child entities... update their key to us
				if ((toSave & EntityType.FieldTest) == EntityType.FieldTest
					&& widget.FieldTestListUsingForeignKeyFieldNullablePopulated)
				{
					foreach (FieldTest childFieldTest in widget.FieldTestListUsingForeignKeyFieldNullable)
					{
						childFieldTest.ForeignKeyFieldNullable = widget.Identity;
						if (!entitiesBeingHandled.ContainsEntity(childFieldTest))
						{
                            if (childFieldTest.IsDeleted)
                                FieldTestRepo.Delete(entitiesNowBeingHandled, toSave, childFieldTest);
                            else
								FieldTestRepo.Save(entitiesNowBeingHandled, toSave, childFieldTest);
						}
					}
				}
				
				// Post save protocol 
				if (saved)
				{
					// Allow derived/partial class to do extra work
					OnAfterSaveEntity(widget);
									
					widget.Reset();
					
					//The insert/update will have resulted in a new database_update row, inform interested parties
					RaiseModelChanged();
				}
			
			}
		}

		/// <summary>
		/// Throws an exception describing the entity we could not find
		/// </summary>
		/// <param name="id">The entity objects Id</param>
		private void ThrowWidgetEntityException(int id)
		{
			throw new EntityNotFoundException<int>(new EntityDescriptor<int>(id, typeof(Widget)));	
		}		
    		
        /// <summary>
        /// Populate the sub entities of widgetList
        /// </summary>
        /// <param name="widgets"></param>
        /// <param name="toPopulate"></param>
        public override void Populate(IEnumerable<Widget> widgets, Flags toPopulate)
        {
            Log("Populate", widgets.GetIdenties(), toPopulate);

            // Implement breadth first loading of related entities.
			// Any entity that has been requested to be loaded, should be loaded at this level where possible.
			// Remove all sub entity types that this entity relates to directly.
            Flags stillToPopulate = toPopulate;
			stillToPopulate = stillToPopulate.Remove(EntityType.FieldTest);
			stillToPopulate = stillToPopulate.Remove(EntityType.FieldTest);
			stillToPopulate = stillToPopulate.Remove(EntityType.Thing);

			// Get sub entities: FieldTest
			if ((toPopulate & EntityType.FieldTest) == EntityType.FieldTest)
            {
				// Grab the ones that actually need populating
				IEnumerable<Widget> toBePopulated = widgets.Where(entity => entity.FieldTestListUsingForeignKeyFieldPopulated == false);
				
				// And load the sub entities for those ones.
                FieldTestList childFieldTestList = toBePopulated.Count() > 0
                    ? FieldTestRepo.GetByForeignKeyField(
						toBePopulated.GetIdenties(), 
						stillToPopulate)
                    : new FieldTestList();
                Dictionary<int, List<FieldTest>> fieldTestListByForeignKeyField = childFieldTestList.MapByForeignKeyField;
				
				// Now go over all the entites. For ones that need popualting, populate collection
				// directly. For those already populated, make a check on sub entities to ensure
				// they are loaded to the required level
				FieldTestList toBeChecked = new FieldTestList();
                foreach (Widget widget in widgets)
                {
					if (!widget.FieldTestListUsingForeignKeyFieldPopulated)
					{
						var FieldTestListsForWidget = fieldTestListByForeignKeyField.ContainsKey(widget.Identity)
							? fieldTestListByForeignKeyField[widget.Identity]
							: null;
						if (FieldTestListsForWidget != null)
						{
							FieldTestListsForWidget.ForEach(entity => entity.ForeignKeyFieldWidget = widget);
						}
						widget.PopulateFieldTestListUsingForeignKeyField(FieldTestListsForWidget);
					}
					else
					{
						toBeChecked.AddRange(widget.FieldTestListUsingForeignKeyField);
					}
                }
				
				// If there's any "to be checked" (because they were already loaded) let the entities own
				// repo do whatever checks it needs to do
                if (toBeChecked.Count > 0)
					FieldTestRepo.Populate(toBeChecked, stillToPopulate);
            }

			// Get sub entities: FieldTest
			if ((toPopulate & EntityType.FieldTest) == EntityType.FieldTest)
            {
				// Grab the ones that actually need populating
				IEnumerable<Widget> toBePopulated = widgets.Where(entity => entity.FieldTestListUsingForeignKeyFieldNullablePopulated == false);
				
				// And load the sub entities for those ones.
                FieldTestList childFieldTestList = toBePopulated.Count() > 0
                    ? FieldTestRepo.GetByForeignKeyFieldNullable(
						toBePopulated.GetIdenties().Select(i => new Nullable<int>(i)).ToArray(), 
						stillToPopulate)
                    : new FieldTestList();
                Dictionary<int?, List<FieldTest>> fieldTestListByForeignKeyFieldNullable = childFieldTestList.MapByForeignKeyFieldNullable;
				
				// Now go over all the entites. For ones that need popualting, populate collection
				// directly. For those already populated, make a check on sub entities to ensure
				// they are loaded to the required level
				FieldTestList toBeChecked = new FieldTestList();
                foreach (Widget widget in widgets)
                {
					if (!widget.FieldTestListUsingForeignKeyFieldNullablePopulated)
					{
						var FieldTestListsForWidget = fieldTestListByForeignKeyFieldNullable.ContainsKey(widget.Identity)
							? fieldTestListByForeignKeyFieldNullable[widget.Identity]
							: null;
						if (FieldTestListsForWidget != null)
						{
							FieldTestListsForWidget.ForEach(entity => entity.ForeignKeyFieldNullableWidget = widget);
						}
						widget.PopulateFieldTestListUsingForeignKeyFieldNullable(FieldTestListsForWidget);
					}
					else
					{
						toBeChecked.AddRange(widget.FieldTestListUsingForeignKeyFieldNullable);
					}
                }
				
				// If there's any "to be checked" (because they were already loaded) let the entities own
				// repo do whatever checks it needs to do
                if (toBeChecked.Count > 0)
					FieldTestRepo.Populate(toBeChecked, stillToPopulate);
            }
			
			// Get parent entities: Thing
			if ((toPopulate & EntityType.Thing) == EntityType.Thing)
            {
				// Grab the ids for the ones that actually need populating
				IEnumerable<Widget> toBePopulated = widgets.Where(entity => entity.ThingRequiresPopulation);
				int[] idsToLoad = toBePopulated.Select(entity => entity.ThingId).ToList().ToArray();
				
				// And load the sub entities for those ones.
                ThingList parentThingList = toBePopulated.Count() > 0
                    ? ThingRepo.Get(idsToLoad, stillToPopulate)
                    : new ThingList();
                Dictionary<int, Thing> thingListById = parentThingList.MapById;
				
				// Now go over all the entites. For ones that need popualting, populate entities
				// directly. For those already populated, make a check on sub entities to ensure
				// they are loaded to the required level
				ThingList toBeChecked = new ThingList();
                foreach (Widget widget in widgets)
                {
					if (widget.ThingRequiresPopulation)
					{
						widget.Thing =
							thingListById.ContainsKey(widget.ThingId)
							? thingListById[widget.ThingId]
							: null;
					}
					else if (widget.ThingPopulated)
					{
						toBeChecked.Add(widget.Thing);
					}
                }
				
				// If there's any "to be checked" (because they were already loaded) let the entiies own
				// repo do whatever checks it needs to do
                if (toBeChecked.Count > 0)
					ThingRepo.Populate(toBeChecked, stillToPopulate);
            }
        }
		
		/// <summary>
		/// Notified listeners in the process, as well as via the database,
		/// that the indicated model has changed
		/// </summary>
		private void RaiseModelChanged()
		{		
			//Allow other clients to learn of changes
            DatabaseUpdateMonitor.IndicateChanged();
		}
	}

	/// <summary>
	/// Provides static access to the Widget store
	/// </summary>
	public static partial class WidgetRepo
	{
        private static WidgetRepository _instance = new WidgetRepository();
		
		/// <summary>
		/// Get all the instances of Widget from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static WidgetList GetAll()
		{
			return _instance.GetAll();
		}

		/// <summary>
		/// Get all the instances of Widget from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static WidgetList GetAll(Flags toPopulate)
		{
			return _instance.GetAll(toPopulate);
		}
		
		/// <summary>
		/// Get several instances of Widget from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static WidgetList Get(int[] ids)
		{
			return _instance.Get(ids);
		}
		
		/// <summary>
		/// Get several instances of Widget from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static WidgetList Get(int[] ids, Flags toPopulate)
		{
			return _instance.Get(ids, toPopulate);
		}

		/// <summary>
		/// Get an instance of Widget from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static Widget Get(int id)
		{
			return _instance.Get(id);
		}

		/// <summary>
		/// Get an instance of Widget from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static Widget Get(int id, Flags toPopulate)
		{
			return _instance.Get(id, toPopulate);
		}

		/// <summary>
        /// Get Widget from the store, by thingId
        /// </summary>
        /// <param name="thingId">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public static WidgetList GetByThingId(int thingId)
		{
			return GetByThingId(thingId, EntityType.None);
		}

		/// <summary>
        /// Get Widget from the store, by thingId
        /// </summary>
        /// <param name="thingId">Search value</param>
        /// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with the given search value</returns>
        public static WidgetList GetByThingId(int thingId, Flags toPopulate)
		{
			return _instance.GetByThingId(thingId, toPopulate);
		}

		/// <summary>
		/// Get entities from the database, by multiple thingIds
        /// </summary>
        /// <param name="thingIds"></param>
        /// <returns>All entities with one of the given search values</returns>
        public static WidgetList GetByThingId(int[] thingIds)
		{
			return GetByThingId(thingIds, EntityType.None);
		}

		/// <summary>
		/// Get entities from the database, by multiple thingIds
        /// </summary>
        /// <param name="thingIds">Search values</param>
		/// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with one of the given search values</returns>
        public static WidgetList GetByThingId(int[] thingIds, Flags toPopulate)
		{
			return _instance.GetByThingId(thingIds, toPopulate);
		}

		/// <summary>
        /// Delete Widget from the store, by thingId.
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="thingId">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public static void DeleteByThingId(int thingId)
		{
			_instance.DeleteByThingId(thingId);
		}

		/// <summary>
		/// Delete entities from the database, by multiple thingIds
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="thingIds">Search values</param>
        public static void DeleteByThingId(int[] thingIds)
		{
			_instance.DeleteByThingId(thingIds);
		}
  
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="widget">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(Widget widget, Flags toPopulate)
		{
			_instance.Populate(widget, toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="widget">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(Widget widget, Flags toPopulate)
		{
			_instance.Refresh(new WidgetList(widget), toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="widgetList">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(WidgetList widgetList, Flags toPopulate)
		{
			_instance.Refresh(widgetList, toPopulate);
		}
		
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="widgetList">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(WidgetList widgetList, Flags toPopulate)
		{
			_instance.Populate(widgetList, toPopulate);
		}
  
		/// <summary>
		/// Delete a Widget from the store
		/// Does NOT delete references.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(int id)
		{
			_instance.Delete(id);
		}

		/// <summary>
		/// Delete a Widget from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="widget">The Widget entitiy to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, Widget widget)
		{
			_instance.Delete(toDelete, widget);
		}
		
		/// <summary>
		/// Delete several Widget entities from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="widgets">The Widget entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, params Widget[] widgets)
		{
			_instance.Delete(toDelete, widgets);
		}
		
		/// <summary>
		/// Delete several Widget entities from the store
		/// </summary>
		/// <param name="entitiesBeingHandled"></param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="widgets">The Widget entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, params Widget[] widgets)
        {
            _instance.Delete(entitiesBeingHandled, toDelete, widgets);
        }
		
		/// <summary>
		/// Delete all Widget from the store - use with care!
		/// Does NOT delete references.
		/// </summary>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void DeleteAll()
		{
			_instance.DeleteAll();
		}

		/// <summary>
		/// Save (insert/update) one or more Widget into the store
		/// </summary>
		/// <param name="widgets">The Widgets to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(params Widget[] widgets)
		{
			_instance.Save(widgets);
		}

		/// <summary>
		/// Save (insert/update) one or more Widget into the store
		/// </summary>
		/// <param name="widgets">The Widgets to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, params Widget[] widgets)
		{
			_instance.Save(toSave, widgets);
		}

		/// <summary>
		/// Save (insert/update) one or more Widget into the store
		/// </summary>
		/// <param name="widgetList">The Widgets to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, WidgetList widgetList)
		{
			_instance.Save(toSave, widgetList);
		}

		/// <summary>
		/// Save (insert/update) one or more Widget into the store
		/// </summary>
		/// <param name="widgetList">The Widgets to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(WidgetList widgetList)
		{
			_instance.Save(widgetList);
		}
		
		/// <summary>
		/// Save (insert/update) one or more Widget into the store
		/// </summary>
		/// <param name="widgets">The Widgets to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params Widget[] widgets)
        {
            _instance.Save(entitiesBeingHandled, toSave, widgets);
        }
	}
}