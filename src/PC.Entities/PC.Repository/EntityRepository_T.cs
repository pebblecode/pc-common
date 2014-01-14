using System;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PebbleCode.Framework;
using PebbleCode.Framework.Collections;
using PebbleCode.Entities;

namespace PebbleCode.Repository
{
    /// <summary>
    /// The base class for IBatis data repositories (entities and views)
    /// </summary>
    public abstract class EntityRepository<TEntity, TList, TPrimaryKey> : EntityRepository, IEntityRepository<TEntity, TList, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TList : EntityList<TEntity, TPrimaryKey>, new()
        where TPrimaryKey : IComparable
    {
		/// <summary>
		/// Get all the instances of entity from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
        public abstract TList GetAll();
		
		/// <summary>
        /// Get all the instances of entity from the store
		/// </summary>
		/// <returns>All the entities in the system</returns>
        public abstract TList GetAll(Flags toPopulate);

		/// <summary>
        /// Get several instances of entity from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public abstract TList Get(TPrimaryKey[] ids);

        /// <summary>
        /// Get several instances of entity from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        public abstract TList Get(TPrimaryKey[] ids, Flags toPopulate);

		/// <summary>
        /// Get an instance of entity from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
        public abstract TEntity Get(TPrimaryKey id);

		/// <summary>
        /// Get an instance of entity from the store
		/// </summary>
		/// <param name="id">The id of the entity to get</param>
		/// <returns>The entity with the given Id</returns>
        public abstract TEntity Get(TPrimaryKey id, Flags toPopulate);

        /// <summary>
        /// Get a single entity from the database using a specific statement and named parameters
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="toPopulate"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected TEntity GetEntity(string statementName, Flags toPopulate, params Param[] parameters)
        {
            Log(statementName, toPopulate);
            TEntity entity = this.Mapper.QueryForObject<TEntity>(statementName, Params(parameters));
            OnAfterLoadEntity(entity);
            Populate(entity, toPopulate);
            return entity;
        }

        /// <summary>
        /// Get a list of entities from the database using a specific statement and named parameters
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="toPopulate"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected TList GetList(string statementName, Flags toPopulate, params Param[] parameters)
        {
            Log(statementName, toPopulate);
            TList result = (TList)this.Mapper.QueryWithRowDelegate<TEntity>(
                statementName,
                Params(parameters),
                OnAfterLoadRowDelegateHandler);
            Populate(result, toPopulate);
            return result;
        }

        /// <summary>
        /// Get a single entity from the database using a specific statement and named parameters
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="toPopulate"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected bool Exists(string statementName, params Param[] parameters)
        {
            Log(statementName);
            return this.Mapper.QueryForObject<bool>(statementName, Params(parameters));
        }

        /// <summary>
        /// Delete entities from the database using a specific statement and named parameters
        /// WARNING: No version checking will be done and no sub entities will be deleted
        /// </summary>
        /// <param name="statementName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected void Delete(string statementName, params Param[] parameters)
        {
            Log(statementName, Flags.None);
            this.Mapper.Delete(statementName, Params(parameters));
        }

        /// <summary>
        /// Populate the sub entities of a single entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="toPopulate"></param>
        public virtual void Populate(TEntity entity, Flags toPopulate)
        {
            if (entity == null) return;
            TList entityList = new TList();
            entityList.Add(entity);
            Populate(entityList, toPopulate);
        }

        /// <summary>
        /// Refreshes the objects in this collection
        /// </summary>
        /// <param name="entityList"></param>
        public void Refresh(TList entityList, Flags toPopulate)
        {
            List<TPrimaryKey> allIds = new List<TPrimaryKey>(entityList.MapById.Keys);
            TList latestObjects = this.Get(allIds.ToArray());
            entityList.Clear();
            entityList.AddRange(latestObjects);
            this.Populate(entityList, toPopulate);
        }
        
        /// <summary>
        /// Populate the sub entities of a list of entities. Strongly typed version must be implemented by
        /// derived classes.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="toPopulate"></param>
        public abstract void Populate(IEnumerable<TEntity> entities, Flags toPopulate);

        /// <summary>
        /// Used during the fetching of data to allow post load data manipulation.
        /// </summary>
        /// <param name="entity">The entity just loaded</param>
        /// <param name="unknown">It really is unknown</param>
        /// <param name="entityList">The list that this entity will be added to</param>
        protected void OnAfterLoadRowDelegateHandler(object entity, object unknown, IList<TEntity> entityList)
        {
            TEntity loadedEntity = (TEntity)entity;
            Log("OnAfterLoadRowDelegateHandler", loadedEntity.Identity, Flags.None);
            if (OnAfterLoadEntity(loadedEntity))
                entityList.Add(loadedEntity);
        }

        /// <summary>
        /// Helper method to post process a list of loaded entities
        /// </summary>
        /// <param name="entityList"></param>
        protected void ProcessLoadedEntityList(TList entityList)
        {
            foreach (TEntity entity in entityList)
                OnAfterLoadEntity(entity);
        }

        /// <summary>
        /// Stub to be overloaded by repositories that require custom On After Load processing
        /// </summary>
        /// <param name="entity">The entity just loaded</param>
        /// <returns>True if no errors during processing, otherwise false</returns>
        protected virtual bool OnAfterLoadEntity(TEntity entity)
        { return true; }

        /// <summary>
        /// Stub to be overloaded by repositories that require custom On Before Save processing
        /// </summary>
        /// <param name="entity">The entity to be saved</param>
        protected virtual void OnBeforeSaveEntity(TEntity entity)
        { }

        /// <summary>
        /// Stub to be overloaded by repositories that require custom On After Save processing
        /// </summary>
        /// <param name="entity">The entity that was just saved</param>
        protected virtual void OnAfterSaveEntity(TEntity entity)
        { }

        /// <summary>
        /// Stub to be overloaded by repositories that require custom On Before Delete processing
        /// </summary>
        /// <param name="entity">The entity that to be deleted</param>
        protected virtual void OnBeforeDeleteEntity(TEntity entity)
        { }

        /// <summary>
        /// Stub to be overloaded by repositories that require custom On After Delete processing
        /// </summary>
        /// <param name="entity">The entity that was just deleted</param>
        protected virtual void OnAfterDeleteEntity(TEntity entity)
        { }

        /// <summary>
        /// Helper method to generate a SQL "IN" clause
        /// </summary>
        /// <param name="inClauseValues">The array of integers to create an IN clause from</param>
        /// <returns>The in cluase string, without the braces. E.g. "1, 2, 3"</returns>
        protected static string CreateInClauseString(int[] inClauseValues)
        {
            if (inClauseValues == null)
                throw new ArgumentNullException("inClauseValues");

            string[] strings = new string[inClauseValues.Length];
            for (int valueIndex = 0; valueIndex < inClauseValues.Length; valueIndex++)
                strings[valueIndex] = inClauseValues[valueIndex].ToString(CultureInfo.InvariantCulture);
            return string.Join(", ", strings);
        }

        /// <summary>
        /// Log info specific to this repo
        /// </summary>
        /// <param name="method"></param>
        /// <param name="additionalEntities"></param>
        [DebuggerStepThrough]
        protected void Log(string method, Flags additionalEntities)
        {
            Log(method, null, additionalEntities);
        }

        /// <summary>
        /// Log info specific to this repo
        /// </summary>
        /// <param name="method"></param>
        /// <param name="additionalEntities"></param>
        [DebuggerStepThrough]
        protected void Log(string method, TPrimaryKey id, Flags additionalEntities)
        {
            Log(method, new TPrimaryKey[] { id }, additionalEntities);
        }

        /// <summary>
        /// Log info specific to this repo
        /// </summary>
        /// <param name="method"></param>
        /// <param name="additionalEntities"></param>
        [DebuggerStepThrough]
        protected void Log(string method, TPrimaryKey[] ids, Flags additionalEntities)
        {
            Log(string.Format("{0}Repository.{1};{2}{3}",
                typeof(TEntity).Name,
                method,
                ids == null ? "" : string.Format("  Ids:{0};", ids.ToString(",")),
                additionalEntities.IsEmpty ? "" : string.Format("  AdditionalEntities:{0};", additionalEntities)
                ));
        }
    }
}

