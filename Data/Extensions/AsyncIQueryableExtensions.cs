using Microsoft.EntityFrameworkCore;

namespace Data.Extensions;

public static class AsyncIQueryableExtensions
{
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
    {
        if (source == null)
        {
            return new PagedList<T>(new List<T>(), pageIndex, pageSize);
        }

        pageSize = Math.Max(pageSize, 1);
        pageIndex = Math.Max(pageIndex, 1);

        var count = await source.CountAsync();
        var data = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(data, pageIndex, pageSize, count);
    }
}