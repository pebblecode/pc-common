using System;
using System.Collections.Generic;
using NUnit.Framework;
using PC.Entities.Tests.Integration.Entities;
using PebbleCode.Framework.Utilities;
using PebbleCode.Tests.Entities;

namespace PebbleCode.Tests.Unit.CtrlUpdateEntityTests
{
    [TestFixture]
    public class PropertyOverrideTests : BaseUnitTest<TestHelper> 
    {
        [Test]
        [ExpectedException(typeof(Exception))]
        public void ControlledUpdate_EntityWithNoUpdateContext_ThrowsException()
        {
            //ARRANGE
            ControlledUpdateThing thing
                = new ControlledUpdateThing
                      {
                          DbName = "Test Thing",
                          DbPropertyAuthorization = GetBasicAuthXml()
                      };
            const string newName = "New Thing";

            //ACT
            thing.Name = newName;
        }

        [Test]
        public void ControlledUpdate_EntityWithNoPrviousAuthorization_AllowPropertyToBeSet()
        {
            //ARRANGE
            ControlledUpdateThing thing
                = new ControlledUpdateThing
                {
                    DbName = "Test Thing",
                    DbPropertyAuthorization = GetBasicAuthXml()
                };
            const string newName = "New Thing";

            //ACT
            using (PebblecodeUpdateContexts.LowerUser(thing))
            {
                thing.Name = newName;
            }

            //ASSERT
            Assert.AreEqual(newName, thing.Name);
        }

        [Test]
        public void ControlledUpdate_EntityWithLowerAuthorization_DontAllowPropertyToBeSet()
        {
            //ARRANGE
            const string higherContext = "HigherUser";
            var auth = new Dictionary<string, string> { { "Name", higherContext } };
            string authXml = SerialisationUtils.ToXml(auth);
            ControlledUpdateThing thing
                = new ControlledUpdateThing
                {
                    DbName = "Test Thing",
                    DbPropertyAuthorization = authXml
                };
            const string newName = "New Thing";

            //ACT
            using (PebblecodeUpdateContexts.LowerUser(thing))
            {
                thing.Name = newName;
            }

            //ASSERT
            Assert.AreNotEqual(newName, thing.Name);
        }

        [Test]
        public void ControlledUpdate_EntityWithMigrationLevelAuthorization_OverrideAnyLevel()
        {
            //ARRANGE
            const string higherContext = "HigherUser";
            var auth = new Dictionary<string, string> { { "Name", higherContext } };
            string authXml = SerialisationUtils.ToXml(auth);
            ControlledUpdateThing thing
                = new ControlledUpdateThing
                {
                    DbName = "Test Thing",
                    DbPropertyAuthorization = authXml
                };
            const string newName = "New Thing";

            //ACT
            using (PebblecodeUpdateContexts.Migration(thing))
            {
                thing.Name = newName;
            }

            //ASSERT
            Assert.AreEqual(newName, thing.Name);
        }

        [Test]
        public void ControlledUpdate_EntityWithMigrationLevelAuthorization_KeepUpdateLevelUnchanged()
        {
            //ARRANGE
            const string previousHigherContext = "HigherUser";
            var auth = new Dictionary<string, string> { { "Name", previousHigherContext } };
            string authXml = SerialisationUtils.ToXml(auth);
            ControlledUpdateThing thing
                = new ControlledUpdateThing
                {
                    DbName = "Test Thing",
                    DbPropertyAuthorization = authXml
                };
            const string newName = "New Thing";

            //ACT
            using (PebblecodeUpdateContexts.Migration(thing))
            {
                thing.Name = newName;
            }
            Dictionary<string, string> currentAuth =
                SerialisationUtils.FromXml<Dictionary<string, string>>(thing.DbPropertyAuthorization);

            //ASSERT
            Assert.IsTrue(currentAuth.ContainsKey("Name"));
            Assert.AreEqual(previousHigherContext, currentAuth["Name"]);
        }

        private string GetBasicAuthXml()
        {
            var auth = new Dictionary<string, string>();
            return SerialisationUtils.ToXml(auth);
        }
    }
}
