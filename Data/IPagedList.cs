namespace Data;

public interface IPagedList<T> : IBasePagedList
{
    public IEnumerable<T> Data { get; }
}