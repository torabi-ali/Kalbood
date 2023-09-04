namespace Data;

public interface IBasePagedList
{
    int PageIndex { get; }

    int PageSize { get; }

    int TotalCount { get; }

    int TotalPages { get; }

    bool HasPreviousPage { get; }

    bool HasNextPage { get; }
}