using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Tests.Entities;

namespace PebbleCode.Tests.Unit.CtrlUpdateEntityTests
{
    [TestClass]
    public class ClassTypeTests : BaseUnitTest<TestHelper> 
    {
        [TestMethod]
        public void MyGen_ForControlledEntities_EntityIsInstanceOfConcreteControlledUpdateEntity()
        {
            //ARRANGE
            ControlledUpdateThing thing = new ControlledUpdateThing();

            //ACT
            Type expectedType = typeof(ConcreteControlledUpdateEntity);

            //ASSERT
            Assert.IsInstanceOfType(thing, expectedType);
        }

        [TestMethod]
        public void MyGen_ForNonControlledEntities_EntityIsInstanceOfConcreteControlledUpdateEntity()
        {
            //ARRANGE
            VersionedThing thing = new VersionedThing();

            //ACT
            Type unexpectedType = typeof(ConcreteControlledUpdateEntity);

            //ASSERT
            Assert.IsNotInstanceOfType(thing, unexpectedType);
        }
    }
}
