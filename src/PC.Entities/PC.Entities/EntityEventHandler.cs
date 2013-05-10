using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Entities
{
    public delegate void EntityEventHandler<TPrimaryKey>(Entity<TPrimaryKey> entity)
        where TPrimaryKey : IComparable;

    public delegate void EntityEventHandler<TEntityType, TPrimaryKey>(TEntityType entity)
        where TEntityType : Entity<TPrimaryKey>
        where TPrimaryKey : IComparable;
}
