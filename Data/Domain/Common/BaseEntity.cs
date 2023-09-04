namespace Data.Domain.Common;

public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; set; }

    public DateTime CreatedOn { get; set; }
}