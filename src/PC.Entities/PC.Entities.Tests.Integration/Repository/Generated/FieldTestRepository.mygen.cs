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
        public FieldTestRepository FieldTestRepo { get; set; }
    }
	
	/// <summary>
	/// Provides access to the FieldTest Repository
	/// </summary>
	public partial class FieldTestRepository : EditableEntityRepository<FieldTest, FieldTestList, int>
	{
		/// <summary>
		/// Get all the instances of FieldTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override FieldTestList GetAll()
		{
			return GetAll(EntityType.None);
		}
		
		/// <summary>
		/// Get all the instances of FieldTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override FieldTestList GetAll(Flags toPopulate)
		{	
            Log("GetAll", toPopulate);
			FieldTestList result = (FieldTestList)this.Mapper.QueryWithRowDelegate<FieldTest>("SelectFieldTest", null, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get several instances of FieldTest from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override FieldTestList Get(int[] ids)
		{
			return Get(ids, EntityType.None);
		}

		/// <summary>
		/// Get several instances of FieldTest from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override FieldTestList Get(int[] ids, Flags toPopulate)
		{
			if (ids == null) throw new ArgumentNullException("ids");
			Log("Get(ids)", ids, toPopulate);
			if (ids.Length == 0) return new FieldTestList();
			FieldTestList result = (FieldTestList)this.Mapper.QueryWithRowDelegate<FieldTest>("SelectFieldTests", ids, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get an instance of FieldTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override FieldTest Get(int id)
		{
			return Get(id, EntityType.None);
		}

		/// <summary>
		/// Get an instance of FieldTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override FieldTest Get(int id, Flags toPopulate)
		{
            Log("Get(id)", id, toPopulate);
			FieldTest fieldTest = this.Mapper.QueryForObject<FieldTest>("SelectFieldTest", id);
			if (fieldTest != null)
			{
				OnAfterLoadEntity(fieldTest);
				Populate(fieldTest, toPopulate);
			}
			return fieldTest;
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyField
        /// </summary>
        /// <param name="foreignKeyField">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public virtual FieldTestList GetByForeignKeyField(int foreignKeyField)
		{
			return GetByForeignKeyField(foreignKeyField, EntityType.None);
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyField
        /// </summary>
        /// <param name="foreignKeyField">Search value</param>
        /// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with the given search value</returns>
        public virtual FieldTestList GetByForeignKeyField(int foreignKeyField, Flags toPopulate)
		{
            Log("GetByForeignKeyField", toPopulate);
			FieldTestList result = (FieldTestList)this.Mapper.QueryWithRowDelegate<FieldTest>("SelectFieldTestByForeignKeyField", foreignKeyField, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFields
        /// </summary>
        /// <param name="foreignKeyFields"></param>
        /// <returns>All entities with one of the given search values</returns>
        public virtual FieldTestList GetByForeignKeyField(int[] foreignKeyFields)
		{
			return GetByForeignKeyField(foreignKeyFields, EntityType.None);
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFields
        /// </summary>
        /// <param name="foreignKeyFields">Search values</param>
		/// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with one of the given search values</returns>
        public virtual FieldTestList GetByForeignKeyField(int[] foreignKeyFields, Flags toPopulate)
		{
            Log("GetByForeignKeyField", toPopulate);
			FieldTestList result = (FieldTestList)this.Mapper.QueryWithRowDelegate<FieldTest>("SelectFieldTestByForeignKeyFields", foreignKeyFields, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
        /// Delete FieldTest from the store, by foreignKeyField.
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyField">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public virtual void DeleteByForeignKeyField(int foreignKeyField)
		{
            Log("DeleteByForeignKeyField");
			int deleted = this.Mapper.Delete("DeleteFieldTestByForeignKeyField", foreignKeyField);
			if (deleted > 0) RaiseModelChanged();
		}

		/// <summary>
		/// Delete entities from the database, by multiple foreignKeyFields
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyFields">Search values</param>
        public virtual void DeleteByForeignKeyField(int[] foreignKeyFields)
		{
            Log("DeleteByForeignKeyField");
			int deleted = this.Mapper.Delete("DeleteFieldTestByForeignKeyFields", foreignKeyFields);
			if (deleted > 0) RaiseModelChanged();
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyFieldNullable
        /// </summary>
        /// <param name="foreignKeyFieldNullable">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public virtual FieldTestList GetByForeignKeyFieldNullable(int? foreignKeyFieldNullable)
		{
			return GetByForeignKeyFieldNullable(foreignKeyFieldNullable, EntityType.None);
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyFieldNullable
        /// </summary>
        /// <param name="foreignKeyFieldNullable">Search value</param>
        /// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with the given search value</returns>
        public virtual FieldTestList GetByForeignKeyFieldNullable(int? foreignKeyFieldNullable, Flags toPopulate)
		{
            Log("GetByForeignKeyFieldNullable", toPopulate);
			FieldTestList result = (FieldTestList)this.Mapper.QueryWithRowDelegate<FieldTest>("SelectFieldTestByForeignKeyFieldNullable", foreignKeyFieldNullable, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFieldNullables
        /// </summary>
        /// <param name="foreignKeyFieldNullables"></param>
        /// <returns>All entities with one of the given search values</returns>
        public virtual FieldTestList GetByForeignKeyFieldNullable(int?[] foreignKeyFieldNullables)
		{
			return GetByForeignKeyFieldNullable(foreignKeyFieldNullables, EntityType.None);
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFieldNullables
        /// </summary>
        /// <param name="foreignKeyFieldNullables">Search values</param>
		/// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with one of the given search values</returns>
        public virtual FieldTestList GetByForeignKeyFieldNullable(int?[] foreignKeyFieldNullables, Flags toPopulate)
		{
            Log("GetByForeignKeyFieldNullable", toPopulate);
			FieldTestList result = (FieldTestList)this.Mapper.QueryWithRowDelegate<FieldTest>("SelectFieldTestByForeignKeyFieldNullables", foreignKeyFieldNullables, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
        /// Delete FieldTest from the store, by foreignKeyFieldNullable.
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyFieldNullable">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public virtual void DeleteByForeignKeyFieldNullable(int? foreignKeyFieldNullable)
		{
            Log("DeleteByForeignKeyFieldNullable");
			int deleted = this.Mapper.Delete("DeleteFieldTestByForeignKeyFieldNullable", foreignKeyFieldNullable);
			if (deleted > 0) RaiseModelChanged();
		}

		/// <summary>
		/// Delete entities from the database, by multiple foreignKeyFieldNullables
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyFieldNullables">Search values</param>
        public virtual void DeleteByForeignKeyFieldNullable(int?[] foreignKeyFieldNullables)
		{
            Log("DeleteByForeignKeyFieldNullable");
			int deleted = this.Mapper.Delete("DeleteFieldTestByForeignKeyFieldNullables", foreignKeyFieldNullables);
			if (deleted > 0) RaiseModelChanged();
		}
  
		/// <summary>
		/// Delete a FieldTest from the store.
		/// Does NOT delete references. Make sure all sub entities deleted first.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
        public virtual void Delete(int id)
		{
            Log("Delete", id, EntityType.None);
			if (this.Mapper.Delete("DeleteFieldTest", id) != 1)
				ThrowFieldTestEntityException(id);
			RaiseModelChanged();
		}
		
		/// <summary>
		/// Delete a FieldTest from the store
		/// </summary>
        /// <param name="fieldTest">The entity to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, FieldTest fieldTest)
		{
			Delete(new List<Entity<int>>(), toDelete, new List<FieldTest>{ fieldTest });
		}

		/// <summary>
		/// Delete multiple FieldTest entities from the store.
		/// </summary>
        /// <param name="fieldTests">The entities to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, IEnumerable<FieldTest> fieldTests)
		{
			Delete(new List<Entity<int>>(), toDelete, fieldTests);
		}
		
		/// <summary>
		/// Delete FieldTest entities from the store, including sub entites marked for deletion
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being deleted further up the delete stack</param>
		/// <param name="fieldTests">The FieldTests to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
		internal void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, IEnumerable<FieldTest> fieldTests)
		{
            if (fieldTests == null)
				throw new ArgumentNullException("fieldTests");
			Log("Delete", fieldTests.Select<FieldTest, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(fieldTests);

			// Loop over each entity and delete it.
			foreach (FieldTest fieldTest in fieldTests)
			{
                // Already being deleted higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(fieldTest))
                    continue;
					
                //Allow partial/subclasses to perform additional processing
                OnBeforeDeleteEntity(fieldTest);

				// Now delete the entity
				if (this.Mapper.Delete("DeleteFieldTest", fieldTest.Identity) != 1)
					ThrowFieldTestEntityException(fieldTest.Identity);
				fieldTest.ResetChanged();                 
				//Allow partial/subclasses to perform additional processing
				OnAfterDeleteEntity(fieldTest);
			}
			
			//Save to the repository updates table
			if (fieldTests.Count() > 0)
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
			int deleted = this.Mapper.Delete("DeleteAllFieldTest", null);
			
			//Save to the repository updates table
			if (deleted > 0)
				RaiseModelChanged();
		}

		/// <summary>
		/// Save (insert/update) a FieldTest into the store
		/// </summary>
		/// <param name="fieldTests">The FieldTests to save</param>
		public override void Save(params FieldTest[] fieldTests)
		{
            Save(EntityType.None, fieldTests);
		}

		/// <summary>
		/// Save (insert/update) a FieldTest into the store
		/// </summary>
		/// <param name="fieldTests">The FieldTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, params FieldTest[] fieldTests)
		{
            Save(new List<Entity<int>>(), toSave, fieldTests);
		}

		/// <summary>
		/// Save (insert/update) a FieldTest into the store
		/// </summary>
		/// <param name="fieldTestList">The FieldTests to save</param>
		public override void Save(FieldTestList fieldTestList)
		{
            Save(EntityType.None, fieldTestList);
		}

		/// <summary>
		/// Save (insert/update) a FieldTest into the store
		/// </summary>
		/// <param name="fieldTestList">The FieldTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, FieldTestList fieldTestList)
		{
            Save(new List<Entity<int>>(), toSave, fieldTestList.ToArray());
		}

		/// <summary>
		/// Save (insert/update) a FieldTest into the store
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being saved further up the save stack</param>
		/// <param name="fieldTests">The FieldTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		internal void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params FieldTest[] fieldTests)
		{
            if (fieldTests == null)
				throw new ArgumentNullException("fieldTests");
			Log("Save", fieldTests.Select<FieldTest, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity<int>> entitiesNowBeingHandled = new List<Entity<int>>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(fieldTests);
			
			// Loop over each entity and save it.
			foreach (FieldTest fieldTest in fieldTests)
			{
				
			
                // Already being saved higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(fieldTest))
                    continue;
					
				// Allow derived/partial class to do extra work
				OnBeforeSaveEntity(fieldTest);
				
				// Save parent entity... so we can update our key to it
				if ((toSave & EntityType.Widget) == EntityType.Widget
					&& fieldTest.ForeignKeyFieldWidgetPopulated)
				{
					if (!entitiesBeingHandled.ContainsEntity(fieldTest.ForeignKeyFieldWidget))
					{
						WidgetRepo.Save(entitiesNowBeingHandled, toSave, fieldTest.ForeignKeyFieldWidget);
						fieldTest.ForeignKeyField = fieldTest.ForeignKeyFieldWidget.Identity;
					}
				}
				
				// Save parent entity... so we can update our key to it
				if ((toSave & EntityType.Widget) == EntityType.Widget
					&& fieldTest.ForeignKeyFieldNullableWidgetPopulated)
				{
					if (!entitiesBeingHandled.ContainsEntity(fieldTest.ForeignKeyFieldNullableWidget))
					{
						WidgetRepo.Save(entitiesNowBeingHandled, toSave, fieldTest.ForeignKeyFieldNullableWidget);
						fieldTest.ForeignKeyFieldNullable = fieldTest.ForeignKeyFieldNullableWidget.Identity;
					}
				}
				
				bool saved = false;
				
				try
				{
					// Save the entity
					if (fieldTest.IsNew)
					{
						this.Mapper.Insert("InsertFieldTest", fieldTest);
						saved = true;
					}
					else if (fieldTest.IsChanged)
					{
						if (this.Mapper.Update("UpdateFieldTest", fieldTest) != 1)
							ThrowFieldTestEntityException(fieldTest.Identity);
						saved = true;
					}
				}
				catch (Exception ex)
				{
					throw EntityLogger<int>.WriteUnexpectedException(
						ex, 
						"Failed to insert/update Entity",
						Category.EntityFramework,
						fieldTest);
				}
				
				// Post save protocol 
				if (saved)
				{
					// Allow derived/partial class to do extra work
					OnAfterSaveEntity(fieldTest);
									
					fieldTest.Reset();
					
					//The insert/update will have resulted in a new database_update row, inform interested parties
					RaiseModelChanged();
				}
			
			}
		}

		/// <summary>
		/// Throws an exception describing the entity we could not find
		/// </summary>
		/// <param name="id">The entity objects Id</param>
		private void ThrowFieldTestEntityException(int id)
		{
			throw new EntityNotFoundException<int>(new EntityDescriptor<int>(id, typeof(FieldTest)));	
		}		
    		
        /// <summary>
        /// Populate the sub entities of fieldTestList
        /// </summary>
        /// <param name="fieldTests"></param>
        /// <param name="toPopulate"></param>
        public override void Populate(IEnumerable<FieldTest> fieldTests, Flags toPopulate)
        {
            Log("Populate", fieldTests.GetIdenties(), toPopulate);

            // Implement breadth first loading of related entities.
			// Any entity that has been requested to be loaded, should be loaded at this level where possible.
			// Remove all sub entity types that this entity relates to directly.
            Flags stillToPopulate = toPopulate;
			stillToPopulate = stillToPopulate.Remove(EntityType.Widget);
			stillToPopulate = stillToPopulate.Remove(EntityType.Widget);
			
			// Get parent entities: Widget
			if ((toPopulate & EntityType.Widget) == EntityType.Widget)
            {
				// Grab the ids for the ones that actually need populating
				IEnumerable<FieldTest> toBePopulated = fieldTests.Where(entity => entity.ForeignKeyFieldWidgetRequiresPopulation);
				int[] idsToLoad = toBePopulated.Select(entity => entity.ForeignKeyField).ToList().ToArray();
				
				// And load the sub entities for those ones.
                WidgetList parentWidgetList = toBePopulated.Count() > 0
                    ? WidgetRepo.Get(idsToLoad, stillToPopulate)
                    : new WidgetList();
                Dictionary<int, Widget> widgetListById = parentWidgetList.MapById;
				
				// Now go over all the entites. For ones that need popualting, populate entities
				// directly. For those already populated, make a check on sub entities to ensure
				// they are loaded to the required level
				WidgetList toBeChecked = new WidgetList();
                foreach (FieldTest fieldTest in fieldTests)
                {
					if (fieldTest.ForeignKeyFieldWidgetRequiresPopulation)
					{
						fieldTest.ForeignKeyFieldWidget =
							widgetListById.ContainsKey(fieldTest.ForeignKeyField)
							? widgetListById[fieldTest.ForeignKeyField]
							: null;
					}
					else if (fieldTest.ForeignKeyFieldWidgetPopulated)
					{
						toBeChecked.Add(fieldTest.ForeignKeyFieldWidget);
					}
                }
				
				// If there's any "to be checked" (because they were already loaded) let the entiies own
				// repo do whatever checks it needs to do
                if (toBeChecked.Count > 0)
					WidgetRepo.Populate(toBeChecked, stillToPopulate);
            }
			
			// Get parent entities: Widget
			if ((toPopulate & EntityType.Widget) == EntityType.Widget)
            {
				// Grab the ids for the ones that actually need populating
				IEnumerable<FieldTest> toBePopulated = fieldTests.Where(entity => entity.ForeignKeyFieldNullableWidgetRequiresPopulation);
				int[] idsToLoad = toBePopulated.Select(entity => entity.ForeignKeyFieldNullable.Value).ToList().ToArray();
				
				// And load the sub entities for those ones.
                WidgetList parentWidgetList = toBePopulated.Count() > 0
                    ? WidgetRepo.Get(idsToLoad, stillToPopulate)
                    : new WidgetList();
                Dictionary<int, Widget> widgetListById = parentWidgetList.MapById;
				
				// Now go over all the entites. For ones that need popualting, populate entities
				// directly. For those already populated, make a check on sub entities to ensure
				// they are loaded to the required level
				WidgetList toBeChecked = new WidgetList();
                foreach (FieldTest fieldTest in fieldTests)
                {
					if (fieldTest.ForeignKeyFieldNullableWidgetRequiresPopulation)
					{
						fieldTest.ForeignKeyFieldNullableWidget =
							widgetListById.ContainsKey(fieldTest.ForeignKeyFieldNullable.Value)
							? widgetListById[fieldTest.ForeignKeyFieldNullable.Value]
							: null;
					}
					else if (fieldTest.ForeignKeyFieldNullableWidgetPopulated)
					{
						toBeChecked.Add(fieldTest.ForeignKeyFieldNullableWidget);
					}
                }
				
				// If there's any "to be checked" (because they were already loaded) let the entiies own
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
	/// Provides static access to the FieldTest store
	/// </summary>
	public static partial class FieldTestRepo
	{
        private static FieldTestRepository _instance = new FieldTestRepository();
		
		/// <summary>
		/// Get all the instances of FieldTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static FieldTestList GetAll()
		{
			return _instance.GetAll();
		}

		/// <summary>
		/// Get all the instances of FieldTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static FieldTestList GetAll(Flags toPopulate)
		{
			return _instance.GetAll(toPopulate);
		}
		
		/// <summary>
		/// Get several instances of FieldTest from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static FieldTestList Get(int[] ids)
		{
			return _instance.Get(ids);
		}
		
		/// <summary>
		/// Get several instances of FieldTest from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static FieldTestList Get(int[] ids, Flags toPopulate)
		{
			return _instance.Get(ids, toPopulate);
		}

		/// <summary>
		/// Get an instance of FieldTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static FieldTest Get(int id)
		{
			return _instance.Get(id);
		}

		/// <summary>
		/// Get an instance of FieldTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static FieldTest Get(int id, Flags toPopulate)
		{
			return _instance.Get(id, toPopulate);
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyField
        /// </summary>
        /// <param name="foreignKeyField">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public static FieldTestList GetByForeignKeyField(int foreignKeyField)
		{
			return GetByForeignKeyField(foreignKeyField, EntityType.None);
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyField
        /// </summary>
        /// <param name="foreignKeyField">Search value</param>
        /// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with the given search value</returns>
        public static FieldTestList GetByForeignKeyField(int foreignKeyField, Flags toPopulate)
		{
			return _instance.GetByForeignKeyField(foreignKeyField, toPopulate);
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFields
        /// </summary>
        /// <param name="foreignKeyFields"></param>
        /// <returns>All entities with one of the given search values</returns>
        public static FieldTestList GetByForeignKeyField(int[] foreignKeyFields)
		{
			return GetByForeignKeyField(foreignKeyFields, EntityType.None);
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFields
        /// </summary>
        /// <param name="foreignKeyFields">Search values</param>
		/// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with one of the given search values</returns>
        public static FieldTestList GetByForeignKeyField(int[] foreignKeyFields, Flags toPopulate)
		{
			return _instance.GetByForeignKeyField(foreignKeyFields, toPopulate);
		}

		/// <summary>
        /// Delete FieldTest from the store, by foreignKeyField.
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyField">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public static void DeleteByForeignKeyField(int foreignKeyField)
		{
			_instance.DeleteByForeignKeyField(foreignKeyField);
		}

		/// <summary>
		/// Delete entities from the database, by multiple foreignKeyFields
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyFields">Search values</param>
        public static void DeleteByForeignKeyField(int[] foreignKeyFields)
		{
			_instance.DeleteByForeignKeyField(foreignKeyFields);
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyFieldNullable
        /// </summary>
        /// <param name="foreignKeyFieldNullable">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public static FieldTestList GetByForeignKeyFieldNullable(int? foreignKeyFieldNullable)
		{
			return GetByForeignKeyFieldNullable(foreignKeyFieldNullable, EntityType.None);
		}

		/// <summary>
        /// Get FieldTest from the store, by foreignKeyFieldNullable
        /// </summary>
        /// <param name="foreignKeyFieldNullable">Search value</param>
        /// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with the given search value</returns>
        public static FieldTestList GetByForeignKeyFieldNullable(int? foreignKeyFieldNullable, Flags toPopulate)
		{
			return _instance.GetByForeignKeyFieldNullable(foreignKeyFieldNullable, toPopulate);
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFieldNullables
        /// </summary>
        /// <param name="foreignKeyFieldNullables"></param>
        /// <returns>All entities with one of the given search values</returns>
        public static FieldTestList GetByForeignKeyFieldNullable(int?[] foreignKeyFieldNullables)
		{
			return GetByForeignKeyFieldNullable(foreignKeyFieldNullables, EntityType.None);
		}

		/// <summary>
		/// Get entities from the database, by multiple foreignKeyFieldNullables
        /// </summary>
        /// <param name="foreignKeyFieldNullables">Search values</param>
		/// <param name="toPopulate">Entities to populate</param>
        /// <returns>All entities with one of the given search values</returns>
        public static FieldTestList GetByForeignKeyFieldNullable(int?[] foreignKeyFieldNullables, Flags toPopulate)
		{
			return _instance.GetByForeignKeyFieldNullable(foreignKeyFieldNullables, toPopulate);
		}

		/// <summary>
        /// Delete FieldTest from the store, by foreignKeyFieldNullable.
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyFieldNullable">Search value</param>
        /// <returns>All entities with the given search value</returns>
        public static void DeleteByForeignKeyFieldNullable(int? foreignKeyFieldNullable)
		{
			_instance.DeleteByForeignKeyFieldNullable(foreignKeyFieldNullable);
		}

		/// <summary>
		/// Delete entities from the database, by multiple foreignKeyFieldNullables
		/// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="foreignKeyFieldNullables">Search values</param>
        public static void DeleteByForeignKeyFieldNullable(int?[] foreignKeyFieldNullables)
		{
			_instance.DeleteByForeignKeyFieldNullable(foreignKeyFieldNullables);
		}
  
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="fieldTest">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(FieldTest fieldTest, Flags toPopulate)
		{
			_instance.Populate(fieldTest, toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="fieldTest">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(FieldTest fieldTest, Flags toPopulate)
		{
			_instance.Refresh(new FieldTestList(fieldTest), toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="fieldTestList">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(FieldTestList fieldTestList, Flags toPopulate)
		{
			_instance.Refresh(fieldTestList, toPopulate);
		}
		
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="fieldTestList">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(FieldTestList fieldTestList, Flags toPopulate)
		{
			_instance.Populate(fieldTestList, toPopulate);
		}
  
		/// <summary>
		/// Delete a FieldTest from the store
		/// Does NOT delete references.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(int id)
		{
			_instance.Delete(id);
		}

		/// <summary>
		/// Delete a FieldTest from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="fieldTest">The FieldTest entitiy to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, FieldTest fieldTest)
		{
			_instance.Delete(toDelete, fieldTest);
		}
		
		/// <summary>
		/// Delete several FieldTest entities from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="fieldTests">The FieldTest entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, params FieldTest[] fieldTests)
		{
			_instance.Delete(toDelete, fieldTests);
		}
		
		/// <summary>
		/// Delete several FieldTest entities from the store
		/// </summary>
		/// <param name="entitiesBeingHandled"></param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="fieldTests">The FieldTest entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Delete(List<Entity<int>> entitiesBeingHandled, Flags toDelete, params FieldTest[] fieldTests)
        {
            _instance.Delete(entitiesBeingHandled, toDelete, fieldTests);
        }
		
		/// <summary>
		/// Delete all FieldTest from the store - use with care!
		/// Does NOT delete references.
		/// </summary>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void DeleteAll()
		{
			_instance.DeleteAll();
		}

		/// <summary>
		/// Save (insert/update) one or more FieldTest into the store
		/// </summary>
		/// <param name="fieldTests">The FieldTests to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(params FieldTest[] fieldTests)
		{
			_instance.Save(fieldTests);
		}

		/// <summary>
		/// Save (insert/update) one or more FieldTest into the store
		/// </summary>
		/// <param name="fieldTests">The FieldTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, params FieldTest[] fieldTests)
		{
			_instance.Save(toSave, fieldTests);
		}

		/// <summary>
		/// Save (insert/update) one or more FieldTest into the store
		/// </summary>
		/// <param name="fieldTestList">The FieldTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, FieldTestList fieldTestList)
		{
			_instance.Save(toSave, fieldTestList);
		}

		/// <summary>
		/// Save (insert/update) one or more FieldTest into the store
		/// </summary>
		/// <param name="fieldTestList">The FieldTests to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(FieldTestList fieldTestList)
		{
			_instance.Save(fieldTestList);
		}
		
		/// <summary>
		/// Save (insert/update) one or more FieldTest into the store
		/// </summary>
		/// <param name="fieldTests">The FieldTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Save(List<Entity<int>> entitiesBeingHandled, Flags toSave, params FieldTest[] fieldTests)
        {
            _instance.Save(entitiesBeingHandled, toSave, fieldTests);
        }
	}
}