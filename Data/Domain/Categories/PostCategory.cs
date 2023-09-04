using Data.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Categories;

public class PostCategory
{
    public int PostId { get; set; }

    public int CategoryId { get; set; }

    public Post Post { get; set; }

    public Category Category { get; set; }
}

public class PostCategoryConfig : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.HasKey(p => new { p.PostId, p.CategoryId });
    }
}