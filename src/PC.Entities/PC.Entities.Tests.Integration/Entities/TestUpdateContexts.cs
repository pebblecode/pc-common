using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PebbleCode.Entities;
using PebbleCode.Entities.Tests.Integration.Entities;

namespace PC.Entities.Tests
{
    public sealed class IntegrationUpdateContexts : UpdateContext<TestUpdateContextConstants, int>
    {
        private IntegrationUpdateContexts(string name, params ConcreteControlledUpdateEntity<int>[] entities)
            : base(name, entities)
        {
        }

        public static IntegrationUpdateContexts PebbleAdmin(params ConcreteControlledUpdateEntity<int>[] entities)
        {
            return new IntegrationUpdateContexts("PebbleAdmin", entities);
        }

        public static IntegrationUpdateContexts HigherUser(params ConcreteControlledUpdateEntity<int>[] entities)
        {
            return new IntegrationUpdateContexts("HigherUser", entities);
        }

        public static IntegrationUpdateContexts LowerUser(params ConcreteControlledUpdateEntity<int>[] entities)
        {
            return new IntegrationUpdateContexts("LowerUser", entities);
        }

        public static IntegrationUpdateContexts Migration(params ConcreteControlledUpdateEntity<int>[] entities)
        {
            return new IntegrationUpdateContexts("Migration", entities);
        }
    }
}
