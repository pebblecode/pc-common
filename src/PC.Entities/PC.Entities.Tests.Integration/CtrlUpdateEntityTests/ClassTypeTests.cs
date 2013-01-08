using NUnit.Framework;
using PC.Entities.Tests.Integration.Entities;
using PebbleCode.Tests;
using PebbleCode.Tests.Entities;

namespace PC.Entities.Tests.Integration.CtrlUpdateEntityTests
{
    [TestFixture]
    public class ClassTypeTests : BaseUnitTest<TestHelper> 
    {
        [Test]
        public void MyGen_ForControlledEntities_EntityIsInstanceOfConcreteControlledUpdateEntity()
        {
            //ARRANGE
            ControlledUpdateThing thing = new ControlledUpdateThing();

            //ASSERT
            Assert.IsInstanceOf<ConcreteControlledUpdateEntity>(thing);
        }

        [Test]
        public void MyGen_ForNonControlledEntities_EntityIsInstanceOfConcreteControlledUpdateEntity()
        {
            //ARRANGE
            VersionedThing thing = new VersionedThing();

            //ASSERT
            Assert.IsNotInstanceOf<ConcreteControlledUpdateEntity>(thing);
        }
    }
}
