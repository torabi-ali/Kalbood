using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations;

/// <inheritdoc />
public partial class AddUrlHistory : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UrlHistory",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                OldUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                NewUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                HttpStatus = table.Column<int>(type: "int", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_UrlHistory", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_UrlHistory_OldUrl",
            table: "UrlHistory",
            column: "OldUrl",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UrlHistory");
    }
}
