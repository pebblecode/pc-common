using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Framework;
using PebbleCode.Entities;

namespace PC.Entities
{
    /// <summary>
    /// Collects together a fragment of a result set, together 
    /// with paging information to indicate which fragment
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TList"></typeparam>
    public class SearchResults<TEntity, TList, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TList : EntityList<TEntity, TPrimaryKey>, new()
        where TPrimaryKey : IComparable
    {
        /// <param name="fragment">A result set fragment</param>
        /// <param name="pageInfo">Information about the page of results represented by this fragment</param>
        /// <param name="totalItems">The total number of items in the search query</param>
        public SearchResults(TList fragment, PageInfo pageInfo, int totalItems)
        {
            this.Results = fragment;
            this.PageInfo = pageInfo != null
                ? pageInfo
                : new PageInfo(1, totalItems);
            this.PageInfo.TotalItems = totalItems;
        }

        public PageInfo PageInfo { get; private set; }
        public TList Results { get; private set; }
    }
}
