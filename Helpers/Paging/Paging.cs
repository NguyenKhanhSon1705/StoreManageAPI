using Microsoft.EntityFrameworkCore;

namespace StoreManageAPI.Helpers.Paging
{
    public class Paging<T,U>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<U> Items { get; set; } = [];
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public Paging(List<U> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Items.AddRange(items);
        }

        public static async Task<Paging<T, U>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync() as List<U> ?? new List<U>();
            return new Paging<T, U>(items, count, pageIndex, pageSize);
        }

    }
}
