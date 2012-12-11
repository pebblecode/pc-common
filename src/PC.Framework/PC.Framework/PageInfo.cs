using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Entities
{
    public class PageInfo
    {
        private int _pageNumber;
        private int _pageSize;
        private int _totalItems;

        public PageInfo(int pageNumber, int pageSize)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _totalItems = 0;
        }

        public PageInfo(int pageNumber, int pageSize, int totalItems)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
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
}
