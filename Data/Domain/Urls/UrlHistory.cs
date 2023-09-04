using Data.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Urls;

public class UrlHistory : BaseEntity
{
    public string OldUrl { get; set; }

    public string NewUrl { get; set; }

    public int HttpStatus { get; set; }
}

public class UrlHistoryConfig : IEntityTypeConfiguration<UrlHistory>
{
    public void Configure(EntityTypeBuilder<UrlHistory> builder)
    {
        builder.Property(p => p.OldUrl).HasMaxLength(128).IsRequired();
        builder.Property(p => p.NewUrl).HasMaxLength(128);
        builder.HasIndex(p => new { p.OldUrl }).IsUnique();
    }
}
