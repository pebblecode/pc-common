using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Entities;

namespace PebbleCode.Entities.Tests.Integration.Entities
{
    public sealed class PebblecodeUpdateContexts : UpdateContext<TestUpdateContextConstants>
    {
        private PebblecodeUpdateContexts(string name, params ConcreteControlledUpdateEntity[] entities)
            : base(name, entities)
        {
        }

        public static PebblecodeUpdateContexts PebbleAdmin(params ConcreteControlledUpdateEntity[] entities)
        {
            return new PebblecodeUpdateContexts("PebbleAdmin", entities);
        }

        public static PebblecodeUpdateContexts HigherUser(params ConcreteControlledUpdateEntity[] entities)
        {
            return new PebblecodeUpdateContexts("HigherUser", entities);
        }

        public static PebblecodeUpdateContexts LowerUser(params ConcreteControlledUpdateEntity[] entities)
        {
            return new PebblecodeUpdateContexts("LowerUser", entities);
        }

        public static PebblecodeUpdateContexts Migration(params ConcreteControlledUpdateEntity[] entities)
        {
            return new PebblecodeUpdateContexts("Migration", entities);
        }
    }
}
