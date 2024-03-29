﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PebbleCode.Entities.Tests.Integration.Entities;
using PebbleCode.Entities.Tests.Integration.Repository;
using PebbleCode.Framework.IoC;
using PebbleCode.Repository;
using PC.Entities.Tests.Integration.Repository;

namespace PebbleCode.Tests.Unit.MockTests
{
    [TestFixture]
    public class AutoWireMockTests : BaseUnitTest<TestHelper>
    {
        [Test]
        public void check_that_get_by_id_returns_null_for_non_mocked_id()
        {
            const int MOCK_ID = 10000000;
            const int NON_MOCK_ID = 10000001;

            // Wire up a random repo
            Widget expectedWidget = new Widget();
            expectedWidget.Description = "Testing";
            expectedWidget.DbId = MOCK_ID;
            AutoWireUpMockRepositoryGetById<WidgetRepository, Widget, int>(expectedWidget);

            // Make a call to WidgetRepo.Get to get a non specified widget
            Widget widget = Kernel.Get<WidgetRepository>().Get(NON_MOCK_ID);
            Assert.IsNull(widget, "widget returned incorrectly");
        }

        [Test]
        public void check_that_get_NOT_by_id_returns_null_for_non_mocked_values()
        {
            const int MOCK_ID = 10000000;

            // Wire up a random repo
            Widget expectedWidget = new Widget();
            expectedWidget.Description = "Testing";
            expectedWidget.DbId = MOCK_ID;
            AutoWireUpMockRepositoryGetById<WidgetRepository, Widget, int>(expectedWidget);

            // Make a call to WidgetRepo.GetByThingId to get a non specified widget
            WidgetList widgets = Kernel.Get<WidgetRepository>().GetByThingId(MOCK_ID);
            Assert.IsNull(widgets, "widget returned incorrectly");
            widgets = Kernel.Get<WidgetRepository>().GetAll();
            Assert.IsNull(widgets, "widget returned incorrectly");
        }

        [Test]
        public void check_that_get_by_id_returns_mocked_item()
        {
            const int MOCK_ID = 10000000;

            // Wire up a random repo
            Widget expectedWidget = new Widget();
            expectedWidget.Description = "Testing";
            expectedWidget.DbId = MOCK_ID;
            AutoWireUpMockRepositoryGetById<WidgetRepository, Widget, int>(expectedWidget);

            // Make a call to WidgetRepo.Get to get the specified widget
            Widget widget = Kernel.Get<WidgetRepository>().Get(MOCK_ID);
            Assert.IsNotNull(widget, "no widget returned");
            Assert.AreEqual(expectedWidget.Description, widget.Description, "incorrect widget returned");
        }

        [Test]
        public void check_that_get_by_id_returns_mocked_item_for_two_different_ids()
        {
            const int MOCK_ID_1 = 10000000;
            const int MOCK_ID_2 = 10000001;

            // Wire up a random repo twice
            Widget expectedWidget1 = new Widget();
            expectedWidget1.Description = "Testing";
            expectedWidget1.DbId = MOCK_ID_1;
            AutoWireUpMockRepositoryGetById<WidgetRepository, Widget, int>(expectedWidget1);

            Widget expectedWidget2 = new Widget();
            expectedWidget2.Description = "TestingAgain";
            expectedWidget2.DbId = MOCK_ID_2;
            AutoWireUpMockRepositoryGetById<WidgetRepository, Widget, int>(expectedWidget2);

            // Make a call to WidgetRepo.Get to get the specified widget
            Widget widget = Kernel.Get<WidgetRepository>().Get(MOCK_ID_1);
            Assert.IsNotNull(widget, "no widget returned");
            Assert.AreEqual(expectedWidget1.Description, widget.Description, "incorrect widget returned");
            widget = Kernel.Get<WidgetRepository>().Get(MOCK_ID_2);
            Assert.IsNotNull(widget, "no widget returned");
            Assert.AreEqual(expectedWidget2.Description, widget.Description, "incorrect widget returned");
        }

        [Test]
        public void AutoWireUpMockRepositorySave_SaveParamArray()
        {
            IEnumerable<Widget> savedItems = null;
            AutoWireUpMockRepositorySave<WidgetRepository, Widget, int>(instruments => savedItems = instruments);

            var repo = Kernel.Get<WidgetRepository>();
            repo.Save(
                new Widget
                {
                    Description = "Item A"
                },
                new Widget
                {
                    Description = "Item B"
                });

            Assert.IsNotNull(savedItems);
            Assert.AreEqual(2, savedItems.Count());
        }

        [Test]
        public void AutoWireUpMockRepositorySave_SaveWidgetList()
        {
            IEnumerable<Widget> savedItems = null;
            AutoWireUpMockRepositorySave<WidgetRepository, Widget, int>(instruments => savedItems = instruments);

            var repo = Kernel.Get<WidgetRepository>();
            repo.Save(new WidgetList(
                new Widget
                {
                    Description = "Item A"
                },
                new Widget
                {
                    Description = "Item B"
                }));

            Assert.IsNotNull(savedItems);
            Assert.AreEqual(2, savedItems.Count());
        }


        [Test]
        public void AutoWireUpMockRepositorySave_SaveParamArray_WithFlags()
        {
            IEnumerable<Widget> savedItems = null;
            AutoWireUpMockRepositorySave<WidgetRepository, Widget, int>(instruments => savedItems = instruments);

            var repo = Kernel.Get<WidgetRepository>();
            repo.Save(EntityType.Widget, 
                new Widget
                {
                    Description = "Item A"
                },
                new Widget
                {
                    Description = "Item B"
                });

            Assert.IsNotNull(savedItems);
            Assert.AreEqual(2, savedItems.Count());
        }

        [Test]
        public void AutoWireUpMockRepositorySave_SaveWidgetList_WithFlags()
        {
            IEnumerable<Widget> savedItems = null;
            AutoWireUpMockRepositorySave<WidgetRepository, Widget, int>(items => savedItems = items);

            var repo = Kernel.Get<WidgetRepository>();
            repo.Save(EntityType.Widget, new WidgetList(
                new Widget
                {
                    Description = "Item A"
                },
                new Widget
                {
                    Description = "Item B"
                }));

            Assert.IsNotNull(savedItems);
            Assert.AreEqual(2, savedItems.Count());
        }

    }
}
