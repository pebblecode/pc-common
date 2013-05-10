using PebbleCode.Entities;

namespace PebbleCode.Entities.Tests.Integration.Entities
{
    public class TestUpdateContext : UpdateContext<TestUpdateContextConstants, int>
    {
        public TestUpdateContext(string name, params ControlledUpdateEntity<TestUpdateContextConstants, int>[] entities)
            : base(name, entities)
        {
        }
    }
}
