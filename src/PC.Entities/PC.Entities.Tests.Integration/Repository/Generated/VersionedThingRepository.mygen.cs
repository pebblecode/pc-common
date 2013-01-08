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
using PC.Entities.Tests.Integration.Entities;
using PC.Entities.Tests.Integration.Repository;
using PebbleCode.Entities;
using PebbleCode.Framework.Collections;
using PebbleCode.Framework.IoC;
using PebbleCode.Framework.Logging;
using PebbleCode.Repository.Exceptions;

using PebbleCode.Tests.Entities;

namespace PebbleCode.Repository
{
	/// <summary>
	/// Add support for this repo to the global DB context accessor
	/// </summary>
    public partial class TestDbContext
    {
        [Inject]
        public VersionedThingRepository VersionedThingRepo { get; set; }
    }
	
	/// <summary>
	/// Provides access to the VersionedThing Repository
	/// </summary>
	public partial class VersionedThingRepository : EditableEntityRepository<VersionedThing, VersionedThingList>
	{
		/// <summary>
		/// Get all the instances of VersionedThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override VersionedThingList GetAll()
		{
			return GetAll(EntityType.None);
		}
		
		/// <summary>
		/// Get all the instances of VersionedThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override VersionedThingList GetAll(Flags toPopulate)
		{	
            Log("GetAll", toPopulate);
			VersionedThingList result = (VersionedThingList)this.Mapper.QueryWithRowDelegate<VersionedThing>("SelectVersionedThing", null, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get several instances of VersionedThing from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override VersionedThingList Get(int[] ids)
		{
			return Get(ids, EntityType.None);
		}

		/// <summary>
		/// Get several instances of VersionedThing from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override VersionedThingList Get(int[] ids, Flags toPopulate)
		{
			if (ids == null) throw new ArgumentNullException("ids");
			Log("Get(ids)", ids, toPopulate);
			if (ids.Length == 0) return new VersionedThingList();
			VersionedThingList result = (VersionedThingList)this.Mapper.QueryWithRowDelegate<VersionedThing>("SelectVersionedThings", ids, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get an instance of VersionedThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override VersionedThing Get(int id)
		{
			return Get(id, EntityType.None);
		}

		/// <summary>
		/// Get an instance of VersionedThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override VersionedThing Get(int id, Flags toPopulate)
		{
            Log("Get(id)", id, toPopulate);
			VersionedThing versionedThing = this.Mapper.QueryForObject<VersionedThing>("SelectVersionedThing", id);
			if (versionedThing != null)
			{
				OnAfterLoadEntity(versionedThing);
				Populate(versionedThing, toPopulate);
			}
			return versionedThing;
		}

		
		/// <summary>
		/// Delete a VersionedThing from the store
		/// </summary>
        /// <param name="versionedThing">The entity to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, VersionedThing versionedThing)
		{
			Delete(new List<Entity>(), toDelete, new List<VersionedThing>{ versionedThing });
		}

		/// <summary>
		/// Delete multiple VersionedThing entities from the store.
		/// </summary>
        /// <param name="versionedThings">The entities to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, IEnumerable<VersionedThing> versionedThings)
		{
			Delete(new List<Entity>(), toDelete, versionedThings);
		}
		
		/// <summary>
		/// Delete VersionedThing entities from the store, including sub entites marked for deletion
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being deleted further up the delete stack</param>
		/// <param name="versionedThings">The VersionedThings to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
		internal void Delete(List<Entity> entitiesBeingHandled, Flags toDelete, IEnumerable<VersionedThing> versionedThings)
		{
            if (versionedThings == null)
				throw new ArgumentNullException("versionedThings");
			Log("Delete", versionedThings.Select<VersionedThing, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity> entitiesNowBeingHandled = new List<Entity>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(versionedThings);

			// Loop over each entity and delete it.
			foreach (VersionedThing versionedThing in versionedThings)
			{
                // Already being deleted higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(versionedThing))
                    continue;
					
                //Allow partial/subclasses to perform additional processing
                OnBeforeDeleteEntity(versionedThing);

				// Now delete the entity
				if (this.Mapper.Delete("DeleteVersionedThing", versionedThing) != 1)
					ThrowVersionedThingEntityException(versionedThing.Identity);
				versionedThing.ResetChanged();                 
				//Allow partial/subclasses to perform additional processing
				OnAfterDeleteEntity(versionedThing);
			}
			
			//Save to the repository updates table
			if (versionedThings.Count() > 0)
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
			int deleted = this.Mapper.Delete("DeleteAllVersionedThing", null);
			
			//Save to the repository updates table
			if (deleted > 0)
				RaiseModelChanged();
		}

		/// <summary>
		/// Save (insert/update) a VersionedThing into the store
		/// </summary>
		/// <param name="versionedThings">The VersionedThings to save</param>
		public override void Save(params VersionedThing[] versionedThings)
		{
            Save(EntityType.None, versionedThings);
		}

		/// <summary>
		/// Save (insert/update) a VersionedThing into the store
		/// </summary>
		/// <param name="versionedThings">The VersionedThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, params VersionedThing[] versionedThings)
		{
            Save(new List<Entity>(), toSave, versionedThings);
		}

		/// <summary>
		/// Save (insert/update) a VersionedThing into the store
		/// </summary>
		/// <param name="versionedThingList">The VersionedThings to save</param>
		public override void Save(VersionedThingList versionedThingList)
		{
            Save(EntityType.None, versionedThingList);
		}

		/// <summary>
		/// Save (insert/update) a VersionedThing into the store
		/// </summary>
		/// <param name="versionedThingList">The VersionedThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, VersionedThingList versionedThingList)
		{
            Save(new List<Entity>(), toSave, versionedThingList.ToArray());
		}

		/// <summary>
		/// Save (insert/update) a VersionedThing into the store
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being saved further up the save stack</param>
		/// <param name="versionedThings">The VersionedThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		internal void Save(List<Entity> entitiesBeingHandled, Flags toSave, params VersionedThing[] versionedThings)
		{
            if (versionedThings == null)
				throw new ArgumentNullException("versionedThings");
			Log("Save", versionedThings.Select<VersionedThing, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity> entitiesNowBeingHandled = new List<Entity>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(versionedThings);
			
			// Loop over each entity and save it.
			foreach (VersionedThing versionedThing in versionedThings)
			{
				
			
                // Already being saved higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(versionedThing))
                    continue;
					
				// Allow derived/partial class to do extra work
				OnBeforeSaveEntity(versionedThing);
				
				bool saved = false;
				
				try
				{
					// Save the entity
					if (versionedThing.IsNew)
					{
						this.Mapper.Insert("InsertVersionedThing", versionedThing);
						saved = true;
					}
					else if (versionedThing.IsChanged)
					{
						if (this.Mapper.Update("UpdateVersionedThing", versionedThing) != 1)
							ThrowVersionedThingEntityException(versionedThing.Identity);
						saved = true;
					}
				}
				catch (Exception ex)
				{
					throw EntityLogger.WriteUnexpectedException(
						ex, 
						"Failed to insert/update Entity",
						Category.EntityFramework,
						versionedThing);
				}
				
				// Post save protocol 
				if (saved)
				{
					// Allow derived/partial class to do extra work
					OnAfterSaveEntity(versionedThing);
					try
					{
						VersionedEntityInfo versionInfo = this.Mapper.QueryForObject<VersionedEntityInfo>(
							"SelectVersionedThingVersionInfo", 
							versionedThing.Identity);
						versionedThing.Reset(versionInfo);
					}
					catch (Exception ex)
					{
						throw EntityLogger.WriteUnexpectedException(
							ex, 
							"Failed to reset version information",
							Category.EntityFramework,
							versionedThing);
					}
					
					//The insert/update will have resulted in a new database_update row, inform interested parties
					RaiseModelChanged();
				}
			
			}
		}

		/// <summary>
		/// Throws an exception describing the entity we could not find
		/// </summary>
		/// <param name="id">The entity objects Id</param>
		private void ThrowVersionedThingEntityException(int id)
		{
			throw new EntityNotFoundException(new EntityDescriptor(id, typeof(VersionedThing)));	
		}		
    		
        /// <summary>
        /// Populate the sub entities of versionedThingList
        /// </summary>
        /// <param name="versionedThings"></param>
        /// <param name="toPopulate"></param>
        public override void Populate(IEnumerable<VersionedThing> versionedThings, Flags toPopulate)
        {
            Log("Populate", versionedThings.GetIdenties(), toPopulate);

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
	/// Provides static access to the VersionedThing store
	/// </summary>
	public static partial class VersionedThingRepo
	{
        private static VersionedThingRepository _instance = new VersionedThingRepository();
		
		/// <summary>
		/// Get all the instances of VersionedThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static VersionedThingList GetAll()
		{
			return _instance.GetAll();
		}

		/// <summary>
		/// Get all the instances of VersionedThing from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static VersionedThingList GetAll(Flags toPopulate)
		{
			return _instance.GetAll(toPopulate);
		}
		
		/// <summary>
		/// Get several instances of VersionedThing from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static VersionedThingList Get(int[] ids)
		{
			return _instance.Get(ids);
		}
		
		/// <summary>
		/// Get several instances of VersionedThing from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static VersionedThingList Get(int[] ids, Flags toPopulate)
		{
			return _instance.Get(ids, toPopulate);
		}

		/// <summary>
		/// Get an instance of VersionedThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static VersionedThing Get(int id)
		{
			return _instance.Get(id);
		}

		/// <summary>
		/// Get an instance of VersionedThing from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static VersionedThing Get(int id, Flags toPopulate)
		{
			return _instance.Get(id, toPopulate);
		}
  
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="versionedThing">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(VersionedThing versionedThing, Flags toPopulate)
		{
			_instance.Populate(versionedThing, toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="versionedThing">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(VersionedThing versionedThing, Flags toPopulate)
		{
			_instance.Refresh(new VersionedThingList(versionedThing), toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="versionedThingList">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(VersionedThingList versionedThingList, Flags toPopulate)
		{
			_instance.Refresh(versionedThingList, toPopulate);
		}
		
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="versionedThingList">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(VersionedThingList versionedThingList, Flags toPopulate)
		{
			_instance.Populate(versionedThingList, toPopulate);
		}

		/// <summary>
		/// Delete a VersionedThing from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="versionedThing">The VersionedThing entitiy to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, VersionedThing versionedThing)
		{
			_instance.Delete(toDelete, versionedThing);
		}
		
		/// <summary>
		/// Delete several VersionedThing entities from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="versionedThings">The VersionedThing entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, params VersionedThing[] versionedThings)
		{
			_instance.Delete(toDelete, versionedThings);
		}
		
		/// <summary>
		/// Delete several VersionedThing entities from the store
		/// </summary>
		/// <param name="entitiesBeingHandled"></param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="versionedThings">The VersionedThing entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Delete(List<Entity> entitiesBeingHandled, Flags toDelete, params VersionedThing[] versionedThings)
        {
            _instance.Delete(entitiesBeingHandled, toDelete, versionedThings);
        }
		
		/// <summary>
		/// Delete all VersionedThing from the store - use with care!
		/// Does NOT delete references.
		/// </summary>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void DeleteAll()
		{
			_instance.DeleteAll();
		}

		/// <summary>
		/// Save (insert/update) one or more VersionedThing into the store
		/// </summary>
		/// <param name="versionedThings">The VersionedThings to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(params VersionedThing[] versionedThings)
		{
			_instance.Save(versionedThings);
		}

		/// <summary>
		/// Save (insert/update) one or more VersionedThing into the store
		/// </summary>
		/// <param name="versionedThings">The VersionedThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, params VersionedThing[] versionedThings)
		{
			_instance.Save(toSave, versionedThings);
		}

		/// <summary>
		/// Save (insert/update) one or more VersionedThing into the store
		/// </summary>
		/// <param name="versionedThingList">The VersionedThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, VersionedThingList versionedThingList)
		{
			_instance.Save(toSave, versionedThingList);
		}

		/// <summary>
		/// Save (insert/update) one or more VersionedThing into the store
		/// </summary>
		/// <param name="versionedThingList">The VersionedThings to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(VersionedThingList versionedThingList)
		{
			_instance.Save(versionedThingList);
		}
		
		/// <summary>
		/// Save (insert/update) one or more VersionedThing into the store
		/// </summary>
		/// <param name="versionedThings">The VersionedThings to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Save(List<Entity> entitiesBeingHandled, Flags toSave, params VersionedThing[] versionedThings)
        {
            _instance.Save(entitiesBeingHandled, toSave, versionedThings);
        }
	}
}