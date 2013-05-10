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
using PebbleCode.Framework;
using PebbleCode.Framework.Collections;
using PebbleCode.Framework.Dates;
using PebbleCode.Framework.IoC;
using PebbleCode.Framework.Logging;
using PebbleCode.Repository;
using PebbleCode.Repository.Exceptions;

using PC.Entities.Tests.Integration.Entities;

namespace PC.Entities.Tests.Integration.Repository
{
	/// <summary>
	/// Add support for this repo to the global DB context accessor
	/// </summary>
    public partial class DbContext
    {
        [Inject]
        public ControlledUpdateThingRepository ControlledUpdateThingRepo { get; set; }
    }
	
	/// <summary>
	/// Provides access to the ControlledUpdateThing Repository
	/// </summary>
	public partial class ControlledUpdateThingRepository : EditableEntityRepository<ControlledUpdateThing, ControlledUpdateThingList, int>
	{
		/// <summary>
		/// Get all the instances of ControlledUpdateThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override ControlledUpdateThingList GetAll()
		{
			return GetAll(EntityType.None);
		}
		
		/// <summary>
		/// Get all the instances of ControlledUpdateThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override ControlledUpdateThingList GetAll(Flags toPopulate)
		{	
            Log("GetAll", toPopulate);
			ControlledUpdateThingList result = (ControlledUpdateThingList)this.Mapper.QueryWithRowDelegate<ControlledUpdateThing>("SelectControlledUpdateThing", null, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get several instances of ControlledUpdateThing from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override ControlledUpdateThingList Get(int[] ids)
		{
			return Get(ids, EntityType.None);
		}

		/// <summary>
		/// Get several instances of ControlledUpdateThing from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override ControlledUpdateThingList Get(int[] ids, Flags toPopulate)
		{
			if (ids == null) throw new ArgumentNullException("ids");
			Log("Get(ids)", ids, toPopulate);
			if (ids.Length == 0) return new ControlledUpdateThingList();
			ControlledUpdateThingList result = (ControlledUpdateThingList)this.Mapper.QueryWithRowDelegate<ControlledUpdateThing>("SelectControlledUpdateThings", ids, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get an instance of ControlledUpdateThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override ControlledUpdateThing Get(int id)
		{
			return Get(id, EntityType.None);
		}

		/// <summary>
		/// Get an instance of ControlledUpdateThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override ControlledUpdateThing Get(int id, Flags toPopulate)
		{
            Log("Get(id)", id, toPopulate);
			ControlledUpdateThing controlledUpdateThing = this.Mapper.QueryForObject<ControlledUpdateThing>("SelectControlledUpdateThing", id);
			if (controlledUpdateThing != null)
			{
				OnAfterLoadEntity(controlledUpdateThing);
				Populate(controlledUpdateThing, toPopulate);
			}
			return controlledUpdateThing;
		}

		
		/// <summary>
		/// Delete a ControlledUpdateThing from the store
		/// </summary>
        /// <param name="controlledUpdateThing">The entity to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, ControlledUpdateThing controlledUpdateThing)
		{
			Delete(new List<Entity<int>>(), toDelete, new List<ControlledUpdateThing>{ controlledUpdateThing });
		}

		/// <summary>
		/// Delete multiple ControlledUpdateThing entities from the store.
		/// </summary>
        /// <param name="controlledUpdateThings">The entities to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, IEnumerable<ControlledUpdateThing> controlledUpdateThings)
		{
			Delete(new List<Entity<int>>(), toDelete, controlledUpdateThings);
		}
		
		/// <summary>
		/// Delete ControlledUpdateThing entities from the store, including sub entites marked for deletion
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being deleted further up the delete stack</param>
		/// <param name="controlledUpdateThings">The ControlledUpdateThings to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
		internal void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, IEnumerable<ControlledUpdateThing> controlledUpdateThings)
		{
            if (controlledUpdateThings == null)
				throw new ArgumentNullException("controlledUpdateThings");
			Log("Delete", controlledUpdateThings.Select<ControlledUpdateThing, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(controlledUpdateThings);

			// Loop over each entity and delete it.
			foreach (ControlledUpdateThing controlledUpdateThing in controlledUpdateThings)
			{
                // Already being deleted higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(controlledUpdateThing))
                    continue;
					
                //Allow partial/subclasses to perform additional processing
                OnBeforeDeleteEntity(controlledUpdateThing);

				// Now delete the entity
				if (this.Mapper.Delete("DeleteControlledUpdateThing", controlledUpdateThing) != 1)
					ThrowControlledUpdateThingEntityException(controlledUpdateThing.Identity);
				controlledUpdateThing.ResetChanged();                 
				//Allow partial/subclasses to perform additional processing
				OnAfterDeleteEntity(controlledUpdateThing);
			}
			
			//Save to the repository updates table
			if (controlledUpdateThings.Count() > 0)
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
			int deleted = this.Mapper.Delete("DeleteAllControlledUpdateThing", null);
			
			//Save to the repository updates table
			if (deleted > 0)
				RaiseModelChanged();
		}

		/// <summary>
		/// Save (insert/update) a ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThings">The ControlledUpdateThings to save</param>
		public override void Save(params ControlledUpdateThing[] controlledUpdateThings)
		{
            Save(EntityType.None, controlledUpdateThings);
		}

		/// <summary>
		/// Save (insert/update) a ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThings">The ControlledUpdateThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, params ControlledUpdateThing[] controlledUpdateThings)
		{
            Save(new List<Entity<int>>(), toSave, controlledUpdateThings);
		}

		/// <summary>
		/// Save (insert/update) a ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThingList">The ControlledUpdateThings to save</param>
		public override void Save(ControlledUpdateThingList controlledUpdateThingList)
		{
            Save(EntityType.None, controlledUpdateThingList);
		}

		/// <summary>
		/// Save (insert/update) a ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThingList">The ControlledUpdateThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, ControlledUpdateThingList controlledUpdateThingList)
		{
            Save(new List<Entity<int>>(), toSave, controlledUpdateThingList.ToArray());
		}

		/// <summary>
		/// Save (insert/update) a ControlledUpdateThing into the store
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being saved further up the save stack</param>
		/// <param name="controlledUpdateThings">The ControlledUpdateThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		internal void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params ControlledUpdateThing[] controlledUpdateThings)
		{
            if (controlledUpdateThings == null)
				throw new ArgumentNullException("controlledUpdateThings");
			Log("Save", controlledUpdateThings.Select<ControlledUpdateThing, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(controlledUpdateThings);
			
			// Loop over each entity and save it.
			foreach (ControlledUpdateThing controlledUpdateThing in controlledUpdateThings)
			{
				using (PC.Entities.Tests.IntegrationUpdateContexts.PebbleAdmin(controlledUpdateThing)) {
			
                // Already being saved higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(controlledUpdateThing))
                    continue;
					
				// Allow derived/partial class to do extra work
				OnBeforeSaveEntity(controlledUpdateThing);
				
				bool saved = false;
				
				try
				{
					// Save the entity
					if (controlledUpdateThing.IsNew)
					{
						this.Mapper.Insert("InsertControlledUpdateThing", controlledUpdateThing);
						saved = true;
					}
					else if (controlledUpdateThing.IsChanged)
					{
						if (this.Mapper.Update("UpdateControlledUpdateThing", controlledUpdateThing) != 1)
							ThrowControlledUpdateThingEntityException(controlledUpdateThing.Identity);
						saved = true;
					}
				}
				catch (Exception ex)
				{
					throw EntityLogger<int>.WriteUnexpectedException(
						ex, 
						"Failed to insert/update Entity",
						Category.EntityFramework,
						controlledUpdateThing);
				}
				
				// Post save protocol 
				if (saved)
				{
					// Allow derived/partial class to do extra work
					OnAfterSaveEntity(controlledUpdateThing);
					try
					{
						VersionedEntityInfo versionInfo = this.Mapper.QueryForObject<VersionedEntityInfo>(
							"SelectControlledUpdateThingVersionInfo", 
							controlledUpdateThing.Identity);
						controlledUpdateThing.Reset(versionInfo);
					}
					catch (Exception ex)
					{
						throw EntityLogger<int>.WriteUnexpectedException(
							ex, 
							"Failed to reset version information",
							Category.EntityFramework,
							controlledUpdateThing);
					}
					
					//The insert/update will have resulted in a new database_update row, inform interested parties
					RaiseModelChanged();
				}
			}
			}
		}

		/// <summary>
		/// Throws an exception describing the entity we could not find
		/// </summary>
		/// <param name="id">The entity objects Id</param>
		private void ThrowControlledUpdateThingEntityException(int id)
		{
			throw new EntityNotFoundException<int>(new EntityDescriptor<int>(id, typeof(ControlledUpdateThing)));	
		}		
    		
        /// <summary>
        /// Populate the sub entities of controlledUpdateThingList
        /// </summary>
        /// <param name="controlledUpdateThings"></param>
        /// <param name="toPopulate"></param>
        public override void Populate(IEnumerable<ControlledUpdateThing> controlledUpdateThings, Flags toPopulate)
        {
            Log("Populate", controlledUpdateThings.GetIdenties(), toPopulate);

            // Implement breadth first loading of related entities.
			// Any entity that has been requested to be loaded, should be loaded at this level where possible.
			// Remove all sub entity types that this entity relates to directly.
            Flags stillToPopulate = toPopulate;
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
	/// Provides static access to the ControlledUpdateThing store
	/// </summary>
	public static partial class ControlledUpdateThingRepo
	{
        private static ControlledUpdateThingRepository _instance = new ControlledUpdateThingRepository();
		
		/// <summary>
		/// Get all the instances of ControlledUpdateThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ControlledUpdateThingList GetAll()
		{
			return _instance.GetAll();
		}

		/// <summary>
		/// Get all the instances of ControlledUpdateThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ControlledUpdateThingList GetAll(Flags toPopulate)
		{
			return _instance.GetAll(toPopulate);
		}
		
		/// <summary>
		/// Get several instances of ControlledUpdateThing from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ControlledUpdateThingList Get(int[] ids)
		{
			return _instance.Get(ids);
		}
		
		/// <summary>
		/// Get several instances of ControlledUpdateThing from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ControlledUpdateThingList Get(int[] ids, Flags toPopulate)
		{
			return _instance.Get(ids, toPopulate);
		}

		/// <summary>
		/// Get an instance of ControlledUpdateThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ControlledUpdateThing Get(int id)
		{
			return _instance.Get(id);
		}

		/// <summary>
		/// Get an instance of ControlledUpdateThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static ControlledUpdateThing Get(int id, Flags toPopulate)
		{
			return _instance.Get(id, toPopulate);
		}
  
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="controlledUpdateThing">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(ControlledUpdateThing controlledUpdateThing, Flags toPopulate)
		{
			_instance.Populate(controlledUpdateThing, toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="controlledUpdateThing">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(ControlledUpdateThing controlledUpdateThing, Flags toPopulate)
		{
			_instance.Refresh(new ControlledUpdateThingList(controlledUpdateThing), toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="controlledUpdateThingList">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(ControlledUpdateThingList controlledUpdateThingList, Flags toPopulate)
		{
			_instance.Refresh(controlledUpdateThingList, toPopulate);
		}
		
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="controlledUpdateThingList">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(ControlledUpdateThingList controlledUpdateThingList, Flags toPopulate)
		{
			_instance.Populate(controlledUpdateThingList, toPopulate);
		}

		/// <summary>
		/// Delete a ControlledUpdateThing from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="controlledUpdateThing">The ControlledUpdateThing entitiy to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, ControlledUpdateThing controlledUpdateThing)
		{
			_instance.Delete(toDelete, controlledUpdateThing);
		}
		
		/// <summary>
		/// Delete several ControlledUpdateThing entities from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="controlledUpdateThings">The ControlledUpdateThing entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, params ControlledUpdateThing[] controlledUpdateThings)
		{
			_instance.Delete(toDelete, controlledUpdateThings);
		}
		
		/// <summary>
		/// Delete several ControlledUpdateThing entities from the store
		/// </summary>
		/// <param name="entitiesBeingHandled"></param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="controlledUpdateThings">The ControlledUpdateThing entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, params ControlledUpdateThing[] controlledUpdateThings)
        {
            _instance.Delete(entitiesBeingHandled, toDelete, controlledUpdateThings);
        }
		
		/// <summary>
		/// Delete all ControlledUpdateThing from the store - use with care!
		/// Does NOT delete references.
		/// </summary>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void DeleteAll()
		{
			_instance.DeleteAll();
		}

		/// <summary>
		/// Save (insert/update) one or more ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThings">The ControlledUpdateThings to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(params ControlledUpdateThing[] controlledUpdateThings)
		{
			_instance.Save(controlledUpdateThings);
		}

		/// <summary>
		/// Save (insert/update) one or more ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThings">The ControlledUpdateThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, params ControlledUpdateThing[] controlledUpdateThings)
		{
			_instance.Save(toSave, controlledUpdateThings);
		}

		/// <summary>
		/// Save (insert/update) one or more ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThingList">The ControlledUpdateThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, ControlledUpdateThingList controlledUpdateThingList)
		{
			_instance.Save(toSave, controlledUpdateThingList);
		}

		/// <summary>
		/// Save (insert/update) one or more ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThingList">The ControlledUpdateThings to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(ControlledUpdateThingList controlledUpdateThingList)
		{
			_instance.Save(controlledUpdateThingList);
		}
		
		/// <summary>
		/// Save (insert/update) one or more ControlledUpdateThing into the store
		/// </summary>
		/// <param name="controlledUpdateThings">The ControlledUpdateThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params ControlledUpdateThing[] controlledUpdateThings)
        {
            _instance.Save(entitiesBeingHandled, toSave, controlledUpdateThings);
        }
	}
}