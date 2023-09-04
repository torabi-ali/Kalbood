using Data.Domain.Categories;
using Data.Domain.Comments;
using Data.Domain.Common;
using Data.Domain.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Posts;

public class Post : ContentEntity
{
    public bool IsPublished { get; set; }

    public bool IsPin { get; set; }

    public int Views { get; set; }

    public int Likes { get; set; }

    public ICollection<PostCategory> Categories { get; set; }

    public ICollection<PostTag> Tags { get; set; }

    public ICollection<Comment> Comments { get; set; }
}

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(128).IsRequired();
        builder.Property(p => p.ImageUrl).HasMaxLength(128).IsRequired();
        builder.Property(p => p.Url).HasMaxLength(128).IsRequired();
        builder.HasIndex(p => new { p.Url }).IsUnique();
    }
}