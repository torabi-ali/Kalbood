namespace Data.Domain.Common;

public interface IBaseEntity
{
    public int Id { get; set; }

    public DateTime CreatedOn { get; }
}
