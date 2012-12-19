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
using PebbleCode.Tests.Repository;
using PebbleCode.Tests.Entities;

namespace PebbleCode.Repository
{
	/// <summary>
	/// Add support for this repo to the global DB context accessor
	/// </summary>
    public partial class TestDbContext
    {
        [Inject]
        public NodeBuilderTestRepository NodeBuilderTestRepo { get; set; }
    }
	
	/// <summary>
	/// Provides access to the NodeBuilderTest Repository
	/// </summary>
	public partial class NodeBuilderTestRepository : EditableEntityRepository<NodeBuilderTest, NodeBuilderTestList>
	{
		/// <summary>
		/// Get all the instances of NodeBuilderTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override NodeBuilderTestList GetAll()
		{
			return GetAll(EntityType.None);
		}
		
		/// <summary>
		/// Get all the instances of NodeBuilderTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		public override NodeBuilderTestList GetAll(Flags toPopulate)
		{	
            Log("GetAll", toPopulate);
			NodeBuilderTestList result = (NodeBuilderTestList)this.Mapper.QueryWithRowDelegate<NodeBuilderTest>("SelectNodeBuilderTest", null, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get several instances of NodeBuilderTest from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override NodeBuilderTestList Get(int[] ids)
		{
			return Get(ids, EntityType.None);
		}

		/// <summary>
		/// Get several instances of NodeBuilderTest from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public override NodeBuilderTestList Get(int[] ids, Flags toPopulate)
		{
			if (ids == null) throw new ArgumentNullException("ids");
			Log("Get(ids)", ids, toPopulate);
			if (ids.Length == 0) return new NodeBuilderTestList();
			NodeBuilderTestList result = (NodeBuilderTestList)this.Mapper.QueryWithRowDelegate<NodeBuilderTest>("SelectNodeBuilderTests", ids, OnAfterLoadRowDelegateHandler);
			Populate(result, toPopulate);
			return result;
		}

		/// <summary>
		/// Get an instance of NodeBuilderTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override NodeBuilderTest Get(int id)
		{
			return Get(id, EntityType.None);
		}

		/// <summary>
		/// Get an instance of NodeBuilderTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		public override NodeBuilderTest Get(int id, Flags toPopulate)
		{
            Log("Get(id)", id, toPopulate);
			NodeBuilderTest nodeBuilderTest = this.Mapper.QueryForObject<NodeBuilderTest>("SelectNodeBuilderTest", id);
			if (nodeBuilderTest != null)
			{
				OnAfterLoadEntity(nodeBuilderTest);
				Populate(nodeBuilderTest, toPopulate);
			}
			return nodeBuilderTest;
		}
  
		/// <summary>
		/// Delete a NodeBuilderTest from the store.
		/// Does NOT delete references. Make sure all sub entities deleted first.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
        public virtual void Delete(int id)
		{
            Log("Delete", id, EntityType.None);
			if (this.Mapper.Delete("DeleteNodeBuilderTest", id) != 1)
				ThrowNodeBuilderTestEntityException(id);
			RaiseModelChanged();
		}
		
		/// <summary>
		/// Delete a NodeBuilderTest from the store
		/// </summary>
        /// <param name="nodeBuilderTest">The entity to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, NodeBuilderTest nodeBuilderTest)
		{
			Delete(new List<Entity>(), toDelete, new List<NodeBuilderTest>{ nodeBuilderTest });
		}

		/// <summary>
		/// Delete multiple NodeBuilderTest entities from the store.
		/// </summary>
        /// <param name="nodeBuilderTests">The entities to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public override void Delete(Flags toDelete, IEnumerable<NodeBuilderTest> nodeBuilderTests)
		{
			Delete(new List<Entity>(), toDelete, nodeBuilderTests);
		}
		
		/// <summary>
		/// Delete NodeBuilderTest entities from the store, including sub entites marked for deletion
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being deleted further up the delete stack</param>
		/// <param name="nodeBuilderTests">The NodeBuilderTests to delete</param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
		internal void Delete(List<Entity> entitiesBeingHandled, Flags toDelete, IEnumerable<NodeBuilderTest> nodeBuilderTests)
		{
            if (nodeBuilderTests == null)
				throw new ArgumentNullException("nodeBuilderTests");
			Log("Delete", nodeBuilderTests.Select<NodeBuilderTest, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity> entitiesNowBeingHandled = new List<Entity>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(nodeBuilderTests);

			// Loop over each entity and delete it.
			foreach (NodeBuilderTest nodeBuilderTest in nodeBuilderTests)
			{
                // Already being deleted higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(nodeBuilderTest))
                    continue;
					
                //Allow partial/subclasses to perform additional processing
                OnBeforeDeleteEntity(nodeBuilderTest);

				// Now delete the entity
				if (this.Mapper.Delete("DeleteNodeBuilderTest", nodeBuilderTest.Identity) != 1)
					ThrowNodeBuilderTestEntityException(nodeBuilderTest.Identity);
				nodeBuilderTest.ResetChanged();                 
				//Allow partial/subclasses to perform additional processing
				OnAfterDeleteEntity(nodeBuilderTest);
			}
			
			//Save to the repository updates table
			if (nodeBuilderTests.Count() > 0)
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
			int deleted = this.Mapper.Delete("DeleteAllNodeBuilderTest", null);
			
			//Save to the repository updates table
			if (deleted > 0)
				RaiseModelChanged();
		}

		/// <summary>
		/// Save (insert/update) a NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTests">The NodeBuilderTests to save</param>
		public override void Save(params NodeBuilderTest[] nodeBuilderTests)
		{
            Save(EntityType.None, nodeBuilderTests);
		}

		/// <summary>
		/// Save (insert/update) a NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTests">The NodeBuilderTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, params NodeBuilderTest[] nodeBuilderTests)
		{
            Save(new List<Entity>(), toSave, nodeBuilderTests);
		}

		/// <summary>
		/// Save (insert/update) a NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTestList">The NodeBuilderTests to save</param>
		public override void Save(NodeBuilderTestList nodeBuilderTestList)
		{
            Save(EntityType.None, nodeBuilderTestList);
		}

		/// <summary>
		/// Save (insert/update) a NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTestList">The NodeBuilderTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		public override void Save(Flags toSave, NodeBuilderTestList nodeBuilderTestList)
		{
            Save(new List<Entity>(), toSave, nodeBuilderTestList.ToArray());
		}

		/// <summary>
		/// Save (insert/update) a NodeBuilderTest into the store
		/// </summary>
        /// <param name="entitiesBeingHandled">Entities already being saved further up the save stack</param>
		/// <param name="nodeBuilderTests">The NodeBuilderTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		internal void Save(List<Entity> entitiesBeingHandled, Flags toSave, params NodeBuilderTest[] nodeBuilderTests)
		{
            if (nodeBuilderTests == null)
				throw new ArgumentNullException("nodeBuilderTests");
			Log("Save", nodeBuilderTests.Select<NodeBuilderTest, int>(entity => entity.Identity).ToArray<int>(), EntityType.None);
			
			// Copy the list of entities being handled, and add this new set of entities to it.
			// We're handling those now.
            List<Entity> entitiesNowBeingHandled = new List<Entity>(entitiesBeingHandled);
            entitiesNowBeingHandled.AddRange(nodeBuilderTests);
			
			// Loop over each entity and save it.
			foreach (NodeBuilderTest nodeBuilderTest in nodeBuilderTests)
			{
				
			
                // Already being saved higher up the stack?
                if (entitiesBeingHandled.ContainsEntity(nodeBuilderTest))
                    continue;
					
				// Allow derived/partial class to do extra work
				OnBeforeSaveEntity(nodeBuilderTest);
				
				bool saved = false;
				
				try
				{
					// Save the entity
					if (nodeBuilderTest.IsNew)
					{
						this.Mapper.Insert("InsertNodeBuilderTest", nodeBuilderTest);
						saved = true;
					}
					else if (nodeBuilderTest.IsChanged)
					{
						if (this.Mapper.Update("UpdateNodeBuilderTest", nodeBuilderTest) != 1)
							ThrowNodeBuilderTestEntityException(nodeBuilderTest.Identity);
						saved = true;
					}
				}
				catch (Exception ex)
				{
					throw EntityLogger.WriteUnexpectedException(
						ex, 
						"Failed to insert/update Entity",
						Category.EntityFramework,
						nodeBuilderTest);
				}
				
				// Post save protocol 
				if (saved)
				{
					// Allow derived/partial class to do extra work
					OnAfterSaveEntity(nodeBuilderTest);
									
					nodeBuilderTest.Reset();
					
					//The insert/update will have resulted in a new database_update row, inform interested parties
					RaiseModelChanged();
				}
			
			}
		}

		/// <summary>
		/// Throws an exception describing the entity we could not find
		/// </summary>
		/// <param name="id">The entity objects Id</param>
		private void ThrowNodeBuilderTestEntityException(int id)
		{
			throw new EntityNotFoundException(new EntityDescriptor(id, typeof(NodeBuilderTest)));	
		}		
    		
        /// <summary>
        /// Populate the sub entities of nodeBuilderTestList
        /// </summary>
        /// <param name="nodeBuilderTests"></param>
        /// <param name="toPopulate"></param>
        public override void Populate(IEnumerable<NodeBuilderTest> nodeBuilderTests, Flags toPopulate)
        {
            Log("Populate", nodeBuilderTests.GetIdenties(), toPopulate);

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
            PebbleCode.Repository.DatabaseUpdateMonitor.IndicateChanged();
		}
	}

	/// <summary>
	/// Provides static access to the NodeBuilderTest store
	/// </summary>
	public static partial class NodeBuilderTestRepo
	{
        private static NodeBuilderTestRepository _instance = new NodeBuilderTestRepository();
		
		/// <summary>
		/// Get all the instances of NodeBuilderTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static NodeBuilderTestList GetAll()
		{
			return _instance.GetAll();
		}

		/// <summary>
		/// Get all the instances of NodeBuilderTest from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static NodeBuilderTestList GetAll(Flags toPopulate)
		{
			return _instance.GetAll(toPopulate);
		}
		
		/// <summary>
		/// Get several instances of NodeBuilderTest from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static NodeBuilderTestList Get(int[] ids)
		{
			return _instance.Get(ids);
		}
		
		/// <summary>
		/// Get several instances of NodeBuilderTest from the store
		/// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A list of the entities with the given ids</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static NodeBuilderTestList Get(int[] ids, Flags toPopulate)
		{
			return _instance.Get(ids, toPopulate);
		}

		/// <summary>
		/// Get an instance of NodeBuilderTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static NodeBuilderTest Get(int id)
		{
			return _instance.Get(id);
		}

		/// <summary>
		/// Get an instance of NodeBuilderTest from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static NodeBuilderTest Get(int id, Flags toPopulate)
		{
			return _instance.Get(id, toPopulate);
		}
  
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="nodeBuilderTest">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(NodeBuilderTest nodeBuilderTest, Flags toPopulate)
		{
			_instance.Populate(nodeBuilderTest, toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="nodeBuilderTest">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(NodeBuilderTest nodeBuilderTest, Flags toPopulate)
		{
			_instance.Refresh(new NodeBuilderTestList(nodeBuilderTest), toPopulate);
		}

		/// <summary>
		/// Refreshes the objects in this collection
		/// </summary>
		/// <param name="nodeBuilderTestList">The list to refresh from database</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Refresh(NodeBuilderTestList nodeBuilderTestList, Flags toPopulate)
		{
			_instance.Refresh(nodeBuilderTestList, toPopulate);
		}
		
		/// <summary>
		/// Ensure the specified types of sub entities are populated
		/// </summary>
		/// <param name="nodeBuilderTestList">The entity to check</param>
		/// <param name="toPopulate">The types of entities to populate</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Populate(NodeBuilderTestList nodeBuilderTestList, Flags toPopulate)
		{
			_instance.Populate(nodeBuilderTestList, toPopulate);
		}
  
		/// <summary>
		/// Delete a NodeBuilderTest from the store
		/// Does NOT delete references.
		/// </summary>
        /// <param name="id">The id of the entity to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(int id)
		{
			_instance.Delete(id);
		}

		/// <summary>
		/// Delete a NodeBuilderTest from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="nodeBuilderTest">The NodeBuilderTest entitiy to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, NodeBuilderTest nodeBuilderTest)
		{
			_instance.Delete(toDelete, nodeBuilderTest);
		}
		
		/// <summary>
		/// Delete several NodeBuilderTest entities from the store
		/// </summary>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="nodeBuilderTests">The NodeBuilderTest entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        public static void Delete(Flags toDelete, params NodeBuilderTest[] nodeBuilderTests)
		{
			_instance.Delete(toDelete, nodeBuilderTests);
		}
		
		/// <summary>
		/// Delete several NodeBuilderTest entities from the store
		/// </summary>
		/// <param name="entitiesBeingHandled"></param>
		/// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        /// <param name="nodeBuilderTests">The NodeBuilderTest entities to delete</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Delete(List<Entity> entitiesBeingHandled, Flags toDelete, params NodeBuilderTest[] nodeBuilderTests)
        {
            _instance.Delete(entitiesBeingHandled, toDelete, nodeBuilderTests);
        }
		
		/// <summary>
		/// Delete all NodeBuilderTest from the store - use with care!
		/// Does NOT delete references.
		/// </summary>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void DeleteAll()
		{
			_instance.DeleteAll();
		}

		/// <summary>
		/// Save (insert/update) one or more NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTests">The NodeBuilderTests to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(params NodeBuilderTest[] nodeBuilderTests)
		{
			_instance.Save(nodeBuilderTests);
		}

		/// <summary>
		/// Save (insert/update) one or more NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTests">The NodeBuilderTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, params NodeBuilderTest[] nodeBuilderTests)
		{
			_instance.Save(toSave, nodeBuilderTests);
		}

		/// <summary>
		/// Save (insert/update) one or more NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTestList">The NodeBuilderTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(Flags toSave, NodeBuilderTestList nodeBuilderTestList)
		{
			_instance.Save(toSave, nodeBuilderTestList);
		}

		/// <summary>
		/// Save (insert/update) one or more NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTestList">The NodeBuilderTests to save</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
		public static void Save(NodeBuilderTestList nodeBuilderTestList)
		{
			_instance.Save(nodeBuilderTestList);
		}
		
		/// <summary>
		/// Save (insert/update) one or more NodeBuilderTest into the store
		/// </summary>
		/// <param name="nodeBuilderTests">The NodeBuilderTests to save</param>
		/// <param name="toSave">Entity types to cascade to, if they are loaded</param>
		[Obsolete("Create an instance of the repository instead of static repository methods.")]
        internal static void Save(List<Entity> entitiesBeingHandled, Flags toSave, params NodeBuilderTest[] nodeBuilderTests)
        {
            _instance.Save(entitiesBeingHandled, toSave, nodeBuilderTests);
        }
	}
}