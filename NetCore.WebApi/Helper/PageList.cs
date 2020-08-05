using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetCore.WebApi.Helper
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public PageList(List<T>items,int pageSize,int pageNum,int count)
        {
            CurrentPage = pageNum;

            TotalPages = (int) Math.Ceiling(count / (double)pageSize);

            AddRange(items);

            PageSize = pageSize;

            TotalCount = count;
        }

        public static async Task<PageList<T>> Create(IQueryable<T>source,int pageNum,int pageSize)
        {
            var count =await source.CountAsync();

            var items =await source.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToListAsync();

            return new PageList<T>(items,pageSize,pageNum,count);
        }
    }
}
