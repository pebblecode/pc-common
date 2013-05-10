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
        public ThingRepository ThingRepo { get; set; }
    }
	
	/// <summary>
	/// Provides access to the Thing Repository
	/// </summary>
	public partial class ThingRepository : EditableEntityRepository<Thing, ThingList, int>
	{
		/// <summary>
		/// Get all the instances of Thing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override ThingList GetAll()
		{
			return GetAll(EntityType.None);
		}
		
		/// <summary>
		/// Get all the instances of Thing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override ThingList GetAll(Flags toPopulate)
		{	
            Log("GetAll", toPopulate);
			ThingList result = (ThingList)this.Mapper.QueryWithRowDelegate<Thing>("SelectThing", null, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get several instances of Thing from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override ThingList Get(int[] ids)
		{
			return Get(ids, EntityType.None);
		}

		/// <summary>
		/// Get several instances of Thing from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override ThingList Get(int[] ids, Flags toPopulate)
		{
			if (ids == null) throw new ArgumentNullException("ids");
			Log("Get(ids)", ids, toPopulate);
			if (ids.Length == 0) return new ThingList();
			ThingList result = (ThingList)this.Mapper.QueryWithRowDelegate<Thing>("SelectThings", ids, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get an instance of Thing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override Thing Get(int id)
		{
			return Get(id, EntityType.None);
		}

		/// <summary>
		/// Get an instance of Thing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override Thing Get(int id, Flags toPopulate)
		{
            Log("Get(id)", id, toPopulate);
			Thing thing = this.Mapper.QueryForObject<Thing>("SelectThing", id);
			if (thing != null)
			{
				OnAfterLoadEntity(thing);
				Populate(thing, toPopulate);
			}
			return thing;
		}
  
		/// <summary>
		/// Delete a Thing from the store.
		/// Does NOT delete references. Make sure all sub entities deleted first.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
        public virtual void Delete(int id)
		{
            Log("Delete", id, EntityType.None);
			if (this.Mapper.Delete("DeleteThing", id) != 1)
				ThrowThingEntityException(id);
			RaiseModelChanged();
		}
		
		/// <summary>
		/// Delete a Thing from the store
		/// </summary>
        /// <param name="thing">The entity to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, Thing thing)
		{
			Delete(new List<Entity<int>>(), toDelete, new List<Thing>{ thing });
		}

		/// <summary>
		/// Delete multiple Thing entities from the store.
		/// </summary>
        /// <param name="things">The entities to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, IEnumerable<Thing> things)
		{
			Delete(new List<Entity<int>>(), toDelete, things);
		}
		
		/// <summary>
		/// Delete Thing entities from the store, including sub entites marked for deletion
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being deleted further up the delete stack</param>
		/// <param name="things">The Things to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
		internal void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, IEnumerable<Thing> things)
		{
            if (things == null)
				throw new ArgumentNullException("things");
			Log("Delete", things.Select<Thing, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(things);

			// Loop over each entity and delete it.
			foreach (Thing thing in things)
			{
                // Already being deleted higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(thing))
                    continue;
					
                //Allow partial/subclasses to perform additional processing
                OnBeforeDeleteEntity(thing);

				// Delete child entities
				if ((toDelete & EntityType.Widget) == EntityType.Widget
					&& thing.WidgetListPopulated)
				{
					foreach (Widget childWidget in thing.WidgetList)
					{
						if (!entitiesBeingHandled.ContainsEntity(childWidget))
							WidgetRepo.Delete(entitiesNowBeingHandled, toDelete, childWidget);
					}
				}

				// Now delete the entity
				if (this.Mapper.Delete("DeleteThing", thing.Identity) != 1)
					ThrowThingEntityException(thing.Identity);
				thing.ResetChanged();                 
				//Allow partial/subclasses to perform additional processing
				OnAfterDeleteEntity(thing);
			}
			
			//Save to the repository updates table
			if (things.Count() > 0)
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
			int deleted = this.Mapper.Delete("DeleteAllThing", null);
			
			//Save to the repository updates table
			if (deleted > 0)
				RaiseModelChanged();
		}

		/// <summary>
		/// Save (insert/update) a Thing into the store
		/// </summary>
		/// <param name="things">The Things to save</param>
		public override void Save(params Thing[] things)
		{
            Save(EntityType.None, things);
		}

		/// <summary>
		/// Save (insert/update) a Thing into the store
		/// </summary>
		/// <param name="things">The Things to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, params Thing[] things)
		{
            Save(new List<Entity<int>>(), toSave, things);
		}

		/// <summary>
		/// Save (insert/update) a Thing into the store
		/// </summary>
		/// <param name="thingList">The Things to save</param>
		public override void Save(ThingList thingList)
		{
            Save(EntityType.None, thingList);
		}

		/// <summary>
		/// Save (insert/update) a Thing into the store
		/// </summary>
		/// <param name="thingList">The Things to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, ThingList thingList)
		{
            Save(new List<Entity<int>>(), toSave, thingList.ToArray());
		}

		/// <summary>
		/// Save (insert/update) a Thing into the store
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being saved further up the save stack</param>
		/// <param name="things">The Things to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		internal void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params Thing[] things)
		{
            if (things == null)
				throw new ArgumentNullException("things");
			Log("Save", things.Select<Thing, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(things);
			
			// Loop over each entity and save it.
			foreach (Thing thing in things)
			{
				
			
                // Already being saved higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(thing))
                    continue;
					
				// Allow derived/partial class to do extra work
				OnBeforeSaveEntity(thing);
				
				bool saved = false;
				
				try
				{
					// Save the entity
					if (thing.IsNew)
					{
						this.Mapper.Insert("InsertThing", thing);
						saved = true;
					}
					else if (thing.IsChanged)
					{
						if (this.Mapper.Update("UpdateThing", thing) != 1)
							ThrowThingEntityException(thing.Identity);
						saved = true;
					}
				}
				catch (Exception ex)
				{
					throw EntityLogger<int>.WriteUnexpectedException(
						ex, 
						"Failed to insert/update Entity",
						Category.EntityFramework,
						thing);
				}
				
				// Save child entities... update their key to us
				if ((toSave & EntityType.Widget) == EntityType.Widget
					&& thing.WidgetListPopulated)
				{
					foreach (Widget childWidget in thing.WidgetList)
					{
						childWidget.ThingId = thing.Identity;
						if (!entitiesBeingHandled.ContainsEntity(childWidget))
						{
                            if (childWidget.IsDeleted)
                                WidgetRepo.Delete(entitiesNowBeingHandled, toSave, childWidget);
                            else
								WidgetRepo.Save(entitiesNowBeingHandled, toSave, childWidget);
						}
					}
				}
				
				// Post save protocol 
				if (saved)
				{
					// Allow derived/partial class to do extra work
					OnAfterSaveEntity(thing);
									
					thing.Reset();
					
					//The insert/update will have resulted in a new database_update row, inform interested parties
					RaiseModelChanged();
				}
			
			}
		}

		/// <summary>
		/// Throws an exception describing the entity we could not find
		/// </summary>
		/// <param name="id">The entity objects Id</param>
		private void ThrowThingEntityException(int id)
		{
			throw new EntityNotFoundException<int>(new EntityDescriptor<int>(id, typeof(Thing)));	
		}		
    		
        /// <summary>
        /// Populate the sub entities of thingList
        /// </summary>
        /// <param name="things"></param>
        /// <param name="toPopulate"></param>
        public override void Populate(IEnumerable<Thing> things, Flags toPopulate)
        {
            Log("Populate", things.GetIdenties(), toPopulate);

            // Implement breadth first loading of related entities.
			// Any entity that has been requested to be loaded, should be loaded at this level where possible.
			// Remove all sub entity types that this entity relates to directly.
            Flags stillToPopulate = toPopulate;
			stillToPopulate = stillToPopulate.Remove(EntityType.Widget);

			// Get sub entities: Widget
			if ((toPopulate & EntityType.Widget) == EntityType.Widget)
            {
				// Grab the ones that actually need populating
				IEnumerable<Thing> toBePopulated = things.Where(entity => entity.WidgetListPopulated == false);
				
				// And load the sub entities for those ones.
                WidgetList childWidgetList = toBePopulated.Count() > 0
                    ? WidgetRepo.GetByThingId(
						toBePopulated.GetIdenties(), 
						stillToPopulate)
                    : new WidgetList();
                Dictionary<int, List<Widget>> widgetListByThingId = childWidgetList.MapByThingId;
				
				// Now go over all the entites. For ones that need popualting, populate collection
				// directly. For those already populated, make a check on sub entities to ensure
				// they are loaded to the required level
				WidgetList toBeChecked = new WidgetList();
                foreach (Thing thing in things)
                {
					if (!thing.WidgetListPopulated)
					{
						var WidgetListsForThing = widgetListByThingId.ContainsKey(thing.Identity)
							? widgetListByThingId[thing.Identity]
							: null;
						if (WidgetListsForThing != null)
						{
							WidgetListsForThing.ForEach(entity => entity.Thing = thing);
						}
						thing.PopulateWidgetList(WidgetListsForThing);
					}
					else
					{
						toBeChecked.AddRange(thing.WidgetList);
					}
                }
				
				// If there's any "to be checked" (because they were already loaded) let the entities own
				// repo do whatever checks it needs to do
                if (toBeChecked.Count > 0)
					WidgetRepo.Populate(toBeChecked, stillToPopulate);
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
	/// Provides static access to the Thing store
	/// </summary>
	public static partial class ThingRepo
	{
        private static ThingRepository _instance = new ThingRepository();
		
		/// <summary>
		/// Get all the instances of Thing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ThingList GetAll()
		{
			return _instance.GetAll();
		}

		/// <summary>
		/// Get all the instances of Thing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ThingList GetAll(Flags toPopulate)
		{
			return _instance.GetAll(toPopulate);
		}
		
		/// <summary>
		/// Get several instances of Thing from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ThingList Get(int[] ids)
		{
			return _instance.Get(ids);
		}
		
		/// <summary>
		/// Get several instances of Thing from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ThingList Get(int[] ids, Flags toPopulate)
		{
			return _instance.Get(ids, toPopulate);
		}

		/// <summary>
		/// Get an instance of Thing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static Thing Get(int id)
		{
			return _instance.Get(id);
		}

		/// <summary>
		/// Get an instance of Thing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static Thing Get(int id, Flags toPopulate)
		{
			return _instance.Get(id, toPopulate);
		}
  
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="thing">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(Thing thing, Flags toPopulate)
		{
			_instance.Populate(thing, toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="thing">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(Thing thing, Flags toPopulate)
		{
			_instance.Refresh(new ThingList(thing), toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="thingList">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(ThingList thingList, Flags toPopulate)
		{
			_instance.Refresh(thingList, toPopulate);
		}
		
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="thingList">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(ThingList thingList, Flags toPopulate)
		{
			_instance.Populate(thingList, toPopulate);
		}
  
		/// <summary>
		/// Delete a Thing from the store
		/// Does NOT delete references.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(int id)
		{
			_instance.Delete(id);
		}

		/// <summary>
		/// Delete a Thing from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="thing">The Thing entitiy to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, Thing thing)
		{
			_instance.Delete(toDelete, thing);
		}
		
		/// <summary>
		/// Delete several Thing entities from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="things">The Thing entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, params Thing[] things)
		{
			_instance.Delete(toDelete, things);
		}
		
		/// <summary>
		/// Delete several Thing entities from the store
		/// </summary>
		/// <param name="entitiesBeingHandled"></param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="things">The Thing entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, params Thing[] things)
        {
            _instance.Delete(entitiesBeingHandled, toDelete, things);
        }
		
		/// <summary>
		/// Delete all Thing from the store - use with care!
		/// Does NOT delete references.
		/// </summary>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void DeleteAll()
		{
			_instance.DeleteAll();
		}

		/// <summary>
		/// Save (insert/update) one or more Thing into the store
		/// </summary>
		/// <param name="things">The Things to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(params Thing[] things)
		{
			_instance.Save(things);
		}

		/// <summary>
		/// Save (insert/update) one or more Thing into the store
		/// </summary>
		/// <param name="things">The Things to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, params Thing[] things)
		{
			_instance.Save(toSave, things);
		}

		/// <summary>
		/// Save (insert/update) one or more Thing into the store
		/// </summary>
		/// <param name="thingList">The Things to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, ThingList thingList)
		{
			_instance.Save(toSave, thingList);
		}

		/// <summary>
		/// Save (insert/update) one or more Thing into the store
		/// </summary>
		/// <param name="thingList">The Things to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(ThingList thingList)
		{
			_instance.Save(thingList);
		}
		
		/// <summary>
		/// Save (insert/update) one or more Thing into the store
		/// </summary>
		/// <param name="things">The Things to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params Thing[] things)
        {
            _instance.Save(entitiesBeingHandled, toSave, things);
        }
	}
}