using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PebbleCode.Entities;
using PebbleCode.Framework.Collections;

namespace PebbleCode.Repository
{
    public interface IEntityRepository<TEntity, TList, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TList : EntityList<TEntity, TPrimaryKey>, new()
        where TPrimaryKey : IComparable
    {
        /// <summary>
        /// Get all the instances of entity from the store
        /// </summary>
        /// <returns>All the entities in the system</returns>
        TList GetAll();

        /// <summary>
        /// Get all the instances of entity from the store
        /// </summary>
        /// <returns>All the entities in the system</returns>
        TList GetAll(Flags toPopulate);

        /// <summary>
        /// Get several instances of entity from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        TList Get(TPrimaryKey[] ids);

        /// <summary>
        /// Get several instances of entity from the store
        /// </summary>
        /// <param name="ids">The ids of the entities to get</param>
        /// <returns>A collection of the entities with the given ids</returns>
        TList Get(TPrimaryKey[] ids, Flags toPopulate);

        /// <summary>
        /// Get an instance of entity from the store
        /// </summary>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>The entity with the given Id</returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// Get an instance of entity from the store
        /// </summary>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>The entity with the given Id</returns>
        TEntity Get(TPrimaryKey id, Flags toPopulate);

        /// <summary>
        /// Populate the sub entities of a single entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="toPopulate"></param>
        void Populate(TEntity entity, Flags toPopulate);

        /// <summary>
        /// Refreshes the objects in this collection
        /// </summary>
        /// <param name="entityList"></param>
        void Refresh(TList entityList, Flags toPopulate);

        /// <summary>
        /// Populate the sub entities of a list of entities. Strongly typed version must be implemented by
        /// derived classes.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="toPopulate"></param>
        void Populate(IEnumerable<TEntity> entities, Flags toPopulate);
    }
}
