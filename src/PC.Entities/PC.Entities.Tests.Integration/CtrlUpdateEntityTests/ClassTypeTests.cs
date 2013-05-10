using NUnit.Framework;
using PebbleCode.Entities.Tests.Integration.Entities;
using PebbleCode.Tests;

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
            Assert.IsInstanceOf<ConcreteControlledUpdateEntity<int>>(thing);
        }

        [Test]
        public void MyGen_ForNonControlledEntities_EntityIsInstanceOfConcreteControlledUpdateEntity()
        {
            //ARRANGE
            VersionedThing thing = new VersionedThing();

            //ASSERT
            Assert.IsNotInstanceOf<ConcreteControlledUpdateEntity<int>>(thing);
        }
    }
}
