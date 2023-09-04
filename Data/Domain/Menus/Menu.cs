using Data.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Domain.Menus;

public class Menu : BaseEntity
{
    public string Title { get; set; }

    public string Url { get; set; }

    public int DisplayOrder { get; set; }

    public int? ParentId { get; set; }

    public Menu Parent { get; set; }

    public ICollection<Menu> SubMenus { get; set; }
}

public class MenuConfig : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(128).IsRequired();
        builder.Property(p => p.Url).HasMaxLength(128).IsRequired();
        builder.HasIndex(p => new { p.Url }).IsUnique();
    }
}
