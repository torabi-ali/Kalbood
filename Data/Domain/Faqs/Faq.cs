using Data.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Faqs;

public class Faq : BaseEntity
{
    public string Url { get; set; }

    public string Question { get; set; }

    public string Answer { get; set; }

    public int DisplayOrder { get; set; }
}

public class FaqConfig : IEntityTypeConfiguration<Faq>
{
    public void Configure(EntityTypeBuilder<Faq> builder)
    {
        builder.Property(p => p.Question).HasMaxLength(2048).IsRequired();
        builder.Property(p => p.Answer).HasMaxLength(4096).IsRequired();
        builder.Property(p => p.Url).HasMaxLength(128).IsRequired();
        builder.HasIndex(p => new { p.Url });
    }
}
