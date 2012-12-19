using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.FSharp.Collections;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Base class for concrete entity list types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class EntityList<T> : List<T>, ICloneable
          where T : Entity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entries">Initial Business Entities to store in the collection</param>
        protected EntityList(IEnumerable<T> entries) : base(new List<T>(entries)) { }

        /// <summary>
        /// Constructor
        /// </summary>
        protected EntityList() { }

        /// <summary>
        /// Abstract clone method. Force all business entities to be clonable
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// Method provided so that derived classes can request that
        /// base members be copied to the new clone object.
        /// Maybe overridden by child classes where instances of collections etc need to be cloned
        /// </summary>
        /// <param name="clone">The new clone</param>
        protected virtual void OnClone(EntityList<T> clone)
        {
            if (clone == null)
                throw new ArgumentNullException("clone");

            // Clone each entity in the collection
            foreach (T entity in this)
                clone.Add((T)entity.Clone());
        }

        /// <summary>
        /// Get all the identitys as an array
        /// </summary>
        public int[] Identitys
        {
            get
            {
                int[] ids = new int[this.Count];
                for (int index = 0; index < ids.Length; index++)
                    ids[index] = this[index].Identity;
                return ids;
            }
        }

        /// <summary>
        /// Map the list into a dictionary keyed by the primary key
        /// </summary>
        /// <typeparam name="TFieldType"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public Dictionary<int, T> MapById
        {
            get
            {
                Dictionary<int, T> result = new Dictionary<int, T>();
                foreach (T entity in this)
                    result[entity.Identity] = entity;
                return result;
            }
        }

        /// <summary>
        /// Quick way to find an index of an entity by id
        /// </summary>
        /// <param name="id"></param>
        public int IndexById(int id)
        {
            return this.FindIndex((entity) => entity.Identity == id);
        }

        /// <summary>
        /// Quick way to find an entity by id
        /// </summary>
        /// <param name="id"></param>
        public T FindById(int id)
        {
            return this.Find((entity) => entity.Identity == id);
        }

        /// <summary>
        /// An alternative to the Linq.First method, this returns either
        /// the first matching entity, or else null if no matching entity is found.
        /// </summary>
        /// <param name="predicate">The boolean matching test to perform on each element</param>
        /// <returns>Either the first match, or else null</returns>
        public T FirstOrNull(Func<T, bool> predicate)
        {
            foreach (T item in this)
            {
                if (predicate(item))
                {
                    return item; 
                }
            }
            return null;
        }

        /// <summary>
        /// Sort the list by entity Id... ascending
        /// </summary>
        public void SortById()
        {
            this.Sort((entity1, entity2) => entity1.Identity.CompareTo(entity2.Identity));
        }

        /// <summary>
        /// Sort the list by entity Id... decending
        /// </summary>
        public void SortByIdReverse()
        {
            this.Sort((entity1, entity2) => entity2.Identity.CompareTo(entity1.Identity));
        }

        /// <summary>
        /// Map the list into a dictionary keyed by the result of getKey on each entity
        /// </summary>
        /// <typeparam name="TFieldType"></typeparam>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public Dictionary<TFieldType, List<T>> MapByField<TFieldType>(GetKeyHandler<TFieldType> getKey)
        {
                Dictionary<TFieldType, List<T>> result = new Dictionary<TFieldType, List<T>>();
                foreach (T entity in this)
                {
                    TFieldType key = getKey(entity);
                    if (!result.ContainsKey(key))
                        result.Add(key, new List<T>());
                    result[key].Add(entity);
                }
                return result;
        }

        /// <summary>
        /// Delegate for MapByField
        /// </summary>
        /// <typeparam name="TFieldType"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public delegate TFieldType GetKeyHandler<TFieldType>(T entity);

        /// <summary>
        /// Returns an item from the collection at random.
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public T SelectAtRandom(Random rng)
        {
            if (Count == 0)
                return default(T);

            int index = (int)Math.Floor((Count - 1) * rng.NextDouble());
            return this[index];
        }

        /// <summary>
        /// Add several items to the list
        /// </summary>
        /// <param name="items"></param>
        public void Add(params T[] items)
        {
            this.AddRange(items);
        }

        /// <summary>
        /// Convert to an FSharp list
        /// </summary>
        /// <returns></returns>
        public FSharpList<T> ToFSharpList()
        {
            FSharpList<T> result = FSharpList<T>.Empty;
            for (int index = this.Count-1; index >= 0; index--)
                result = new FSharpList<T>(this[index], result);
            return result;
        }

        /// <summary>
        /// Convert to an FSharp list
        /// </summary>
        /// <returns></returns>
        public FSharpList<Type> ToFSharpList<Type>()
            where Type : class
        {
            FSharpList<Type> result = FSharpList<Type>.Empty;
            for (int index = this.Count - 1; index >= 0; index--)
            {
                Type cast = this[index] as Type;
                result = new FSharpList<Type>(cast, result);
            }
            return result;
        }

        /// <summary>
        /// Default collection class implementation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(", ", this.Select(entity => entity.ToString()));
        }
    }
}
