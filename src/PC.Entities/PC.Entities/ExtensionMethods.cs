using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Entities;

namespace PebbleCode.Entities
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Check if a list of entities contains a given entity
        /// </summary>
        /// <param name="sourceList"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool ContainsEntity<TPrimaryKey>(this IEnumerable<Entity<TPrimaryKey>> sourceList, Entity<TPrimaryKey> target)
            where TPrimaryKey : IComparable
        {
            foreach (Entity<TPrimaryKey> source in sourceList)
            {
                if (source.Identity.Equals(target.Identity)                            // Same Id
                    && source.GetType() == target.GetType()                       // Same type
                    && source.IsNew == target.IsNew                               // Both new or not
                    && (!source.IsNew || Entity<TPrimaryKey>.ReferenceEquals(source, target))) // If new, must be ref equal
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get all the identitys as an array
        /// </summary>
        public static TPrimaryKey[] GetIdenties<TPrimaryKey>(this IEnumerable<Entity<TPrimaryKey>> entities)
            where TPrimaryKey : IComparable
        {
            TPrimaryKey[] ids = new TPrimaryKey[entities.Count()];
            int index = 0;
            foreach (var entity in entities)
            {
                ids[index] = entity.Identity;
                index++;
            }
            return ids;
        }
    }
}
