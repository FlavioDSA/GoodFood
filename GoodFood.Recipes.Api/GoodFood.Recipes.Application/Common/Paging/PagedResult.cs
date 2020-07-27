using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodFood.Recipes.Application.Common.Paging
{
    public class PagedResult<T>
    {
        public PagedResult(PagedRequest pagedRequest, int totalItems, IEnumerable<T> data)
        {
            if (pagedRequest == null)
            {
                throw new ArgumentNullException("pagedRequest");
            }

            if (data == null)
            {
                data = Enumerable.Empty<T>();
            }

            this.TotalItems = totalItems;
            this.CurrentPage = pagedRequest.CurrentPage;
            this.PageSize = pagedRequest.PageSize;
            this.TotalPages = this.TotalItems / this.PageSize + (this.TotalItems % this.PageSize > 0 ? 1 : 0);
            this.Data = data;
        }

        public int CurrentPage { get; }
        
        public int PageSize { get; }

        public int TotalItems { get; }

        public int TotalPages { get; }

        public IEnumerable<T> Data { get; }
    }
}