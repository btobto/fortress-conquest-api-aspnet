using Microsoft.EntityFrameworkCore;

namespace FortressConquestApi.Models
{
    public class PaginatedList<T>
    {
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int PageCount { get; private set; }
        public List<T> Items { get; private set; }

        private PaginatedList(List<T> items, int page, int pageSize, int pageCount)
        {
            Page = page;
            PageSize = pageSize;
            PageCount = pageCount;
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new(
                items, 
                page, 
                pageSize, 
                (int)Math.Ceiling(count / (double)pageSize));
        }
    }
}
