using PebbleCode.Entities;

namespace PebbleCode.Entities.Tests.Integration.Entities
{
    public class TestUpdateContext : UpdateContext<TestUpdateContextConstants>
    {
        public TestUpdateContext(string name, params ControlledUpdateEntity<TestUpdateContextConstants>[] entities)
            : base(name, entities)
        {
        }
    }
}
