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
        public static bool ContainsEntity(this IEnumerable<Entity> sourceList, Entity target)
        {
            foreach (Entity source in sourceList)
            {
                if (source.Identity == target.Identity                            // Same Id
                    && source.GetType() == target.GetType()                       // Same type
                    && source.IsNew == target.IsNew                               // Both new or not
                    && (!source.IsNew || Entity.ReferenceEquals(source, target))) // If new, must be ref equal
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get all the identitys as an array
        /// </summary>
        public static int[] GetIdenties(this IEnumerable<Entity> entities)
        {
            int[] ids = new int[entities.Count()];
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
