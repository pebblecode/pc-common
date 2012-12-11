using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Entities
{
    public delegate void EntityEventHandler(Entity entity);

    public delegate void EntityEventHandler<TEntityType>(TEntityType entity)
        where TEntityType : Entity;
}
