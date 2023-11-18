using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations;

/// <inheritdoc />
public partial class AddFaq : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Faq",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Url = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Question = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                Answer = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                DisplayOrder = table.Column<int>(type: "int", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Faq", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Faq_Url",
            table: "Faq",
            column: "Url");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Faq");
    }
}
