using Data.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Tags;

public class PostTag
{
    public int PostId { get; set; }

    public int TagId { get; set; }

    public Post Post { get; set; }

    public Tag Category { get; set; }
}

public class PostTagConfig : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder.HasKey(p => new { p.PostId, p.TagId });
    }
}