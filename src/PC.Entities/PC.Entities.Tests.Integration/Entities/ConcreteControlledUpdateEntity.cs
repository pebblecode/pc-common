using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Entities;

namespace PC.Entities.Tests.Integration.Entities
{
    public abstract class ConcreteControlledUpdateEntity<TPrimaryKey> : ControlledUpdateEntity<TestUpdateContextConstants, int>
    {
    }
}
