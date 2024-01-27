using Microsoft.EntityFrameworkCore;

namespace FortressConquestApi.Models
{
    public class PaginatedList<T>
    {
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public List<T> Items { get; private set; } = null!;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>
            {
                Page = page,
                PageSize = pageSize,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
        }
    }
}
