using PebbleCode.Entities;

namespace PebbleCode.Tests.Entities
{
    public class TestUpdateContext : UpdateContext<TestUpdateContextConstants>
    {
        public TestUpdateContext(string name, params ControlledUpdateEntity<TestUpdateContextConstants>[] entities)
            : base(name, entities)
        {
        }
    }
}
