using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Framework.Utilities;
using PebbleCode.Tests.Entities;
using PebbleCode.Framework;
namespace PebbleCode.Tests.Unit.CtrlUpdateEntityTests
{
    [TestClass]
    public class PropertyOverrideTests : BaseUnitTest<TestHelper> 
    {
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
