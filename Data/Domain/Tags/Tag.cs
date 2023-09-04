using Data.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Tags;

public class Tag : BaseEntity
{
    public string Title { get; set; }

    public string Url { get; set; }

    public ICollection<PostTag> Posts { get; set; }
}

public class TagConfig : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(128).IsRequired();
        builder.Property(p => p.Url).HasMaxLength(128).IsRequired();
        builder.HasIndex(p => new { p.Url }).IsUnique();
    }
}
