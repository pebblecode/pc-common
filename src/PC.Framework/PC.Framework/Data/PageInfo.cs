using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Framework;
namespace PebbleCode.Framework
{
    /// <summary>
    /// Paging information - describes a current page within a dataset
    /// </summary>
    public class PageInfo
    {
        private int _pageNumber;
        private int _pageSize;
        private int _totalItems;

        public PageInfo(int pageNumber, int pageSize)
        {
            _pageNumber = Math.Max(1, pageNumber);
            _pageSize = Math.Max(1, pageSize);
        }

        public PageInfo(int pageNumber, int pageSize, int totalItems) : this(pageNumber, pageSize)
        {
            _totalItems = totalItems;
        }

        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
        }

        public int TotalItems
        {
            get
            {
                return _totalItems;
            }
            set
            {
                _totalItems = value;
            }
        }

        internal int LimitStart
        {
            get
            {
                return (PageNumber - 1) * PageSize;
            }
        }

        public int TotalPages
        {
            get
            {
                //int division gives us number of full pages
                int pagesRequired = TotalItems / PageSize;
                if ((TotalItems % PageSize) != 0)
                {
                    pagesRequired++;
                }
                return pagesRequired;
            }
        }
    }

    public static class PageInfoExtensions
    {
        private const string PAGE_NUMBER = "pageNumber";
        private const string PAGE_SIZE = "pageSize";

        /// <summary>
        /// Utility function for building Param objects to be fed into an iBatis query
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public static Param[] ToParams(this PageInfo pageInfo)
        {
            if (pageInfo != null)
            {
                return new[]
                {
                    new Param(PAGE_NUMBER, pageInfo.LimitStart),
                    new Param(PAGE_SIZE, pageInfo.PageSize)
                };
            }
            else
            {
                return new[]
                {
                    new Param(PAGE_NUMBER, null),
                    new Param(PAGE_SIZE, null)
                };
            }
        }
    }
}
