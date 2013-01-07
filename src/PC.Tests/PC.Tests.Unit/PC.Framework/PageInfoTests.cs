using NUnit.Framework;
using PebbleCode.Framework;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestFixture]
    public class PageInfoTests
    {
        [Test]
        public void less_than_one_page_of_data()
        {
            PageInfo pageInfo = new PageInfo(pageNumber: 1, pageSize: 50, totalItems: 49);
            Assert.AreEqual(1, pageInfo.TotalPages);
            Assert.AreEqual(1, pageInfo.PageNumber);
            Assert.AreEqual(50, pageInfo.PageSize);
        }

        [Test]
        public void more_than_one_page_of_data()
        {
            PageInfo pageInfo = new PageInfo(pageNumber: 1, pageSize: 50, totalItems: 51);
            Assert.AreEqual(2, pageInfo.TotalPages);
            Assert.AreEqual(1, pageInfo.PageNumber);
            Assert.AreEqual(50, pageInfo.PageSize);
        }

        [Test]
        public void exactly_one_page_of_data()
        {
            PageInfo pageInfo = new PageInfo(pageNumber: 1, pageSize: 50, totalItems: 50);
            Assert.AreEqual(1, pageInfo.TotalPages);
            Assert.AreEqual(1, pageInfo.PageNumber);
            Assert.AreEqual(50, pageInfo.PageSize);
        }

        [Test]
        public void exactly_two_pages_of_data()
        {
            PageInfo pageInfo = new PageInfo(pageNumber: 1, pageSize: 25, totalItems: 50);
            Assert.AreEqual(2, pageInfo.TotalPages);
            Assert.AreEqual(1, pageInfo.PageNumber);
            Assert.AreEqual(25, pageInfo.PageSize);
        }

        [Test]
        public void no_data()
        {
            PageInfo pageInfo = new PageInfo(pageNumber: 1, pageSize: 25, totalItems: 0);
            Assert.AreEqual(0, pageInfo.TotalPages);
            Assert.AreEqual(1, pageInfo.PageNumber);
            Assert.AreEqual(25, pageInfo.PageSize);
        }

        [Test]
        public void invalid_page_number_first_returned()
        {
            PageInfo pageInfo = new PageInfo(pageNumber: 0, pageSize: 25, totalItems: 50);
            Assert.AreEqual(2, pageInfo.TotalPages);
            Assert.AreEqual(1, pageInfo.PageNumber);
            Assert.AreEqual(25, pageInfo.PageSize);
        }

        [Test]
        public void invalid_page_size_non_zero_used()
        {
            PageInfo pageInfo = new PageInfo(pageNumber: 1, pageSize: 0, totalItems: 50);
            Assert.AreNotEqual(0, pageInfo.TotalPages);
            Assert.AreEqual(1, pageInfo.PageNumber);
            Assert.AreNotEqual(0, pageInfo.PageSize);
        }
    }
}
