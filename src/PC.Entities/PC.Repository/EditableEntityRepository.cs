using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Entities;
using PebbleCode.Framework.Collections;

namespace PebbleCode.Repository
{
    /// <summary>
    /// The base class for IBatis data repositories (entities only, not views)
    /// </summary>
    public abstract class EditableEntityRepository<TEntity, TList> : EntityRepository<TEntity, TList>, IEditableEntityRepository<TEntity, TList>
        where TEntity : Entity
        where TList : EntityList<TEntity>, new()
    {
        /// <summary>
        /// Delete an entity from the store
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public abstract void Delete(Flags toDelete, TEntity entity);

        /// <summary>
        /// Delete multiple entities from the store.
        /// </summary>
        /// <param name="entities">The entities to delete</param>
        /// <param name="toDelete">Entity types to cascade to, if they are loaded</param>
        public abstract void Delete(Flags toDelete, IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete all entries in this table - use with care!
        /// Does NOT delete references. Make sure all sub entities deleted first.
        /// </summary>
        public abstract void DeleteAll();

        /// <summary>
        /// Save (insert/update) entities into the store
        /// </summary>
        /// <param name="entities">The entities to save</param>
        public abstract void Save(params TEntity[] entities);

        /// <summary>
        /// Save (insert/update) entities into the store
        /// </summary>
        /// <param name="entities">The entities to save</param>
        /// <param name="toSave">Entity types to cascade to, if they are loaded</param>
        public abstract void Save(Flags toSave, params TEntity[] entities);

        /// <summary>
        /// Save (insert/update) entities into the store
        /// </summary>
        /// <param name="entityList">The entities to save</param>
        public abstract void Save(TList entityList);

        /// <summary>
        /// Save (insert/update) entities into the store
        /// </summary>
        /// <param name="entityList">The entities to save</param>
        /// <param name="toSave">Entity types to cascade to, if they are loaded</param>
        public abstract void Save(Flags toSave, TList entityList);
    }
}
