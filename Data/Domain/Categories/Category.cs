using Data.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Categories;

public class Category : ContentEntity
{
    public bool IsPin { get; set; }

    public int? ParentId { get; set; }

    public Category Parent { get; set; }

    public ICollection<PostCategory> Posts { get; set; }
}

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(128).IsRequired();
        builder.Property(p => p.ImageUrl).HasMaxLength(128).IsRequired();
        builder.Property(p => p.Url).HasMaxLength(128).IsRequired();
        builder.HasIndex(p => new { p.Url }).IsUnique();
    }
}