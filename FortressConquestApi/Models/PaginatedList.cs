using Microsoft.EntityFrameworkCore;

namespace FortressConquestApi.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int PageCount { get; private set; }

        public PaginatedList(List<T> items, int page, int pageSize, int pageCount)
        {
            Page = page;
            PageSize = pageSize;
            PageCount = pageCount;
            this.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(
                items, 
                page, 
                pageSize, (int)Math.Ceiling(count / (double)pageSize));
        }
    }
}
