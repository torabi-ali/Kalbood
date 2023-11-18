using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations;

/// <inheritdoc />
public partial class AddIsPublished : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Menu_Menu_ParentMenuId",
            table: "Menu");

        migrationBuilder.DropIndex(
            name: "IX_Menu_ParentMenuId",
            table: "Menu");

        migrationBuilder.DropColumn(
            name: "ParentMenuId",
            table: "Menu");

        migrationBuilder.AddColumn<bool>(
            name: "IsPublished",
            table: "Post",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsPublished",
            table: "Comment",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "DataProtectionKeys",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Xml = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Menu_ParentId",
            table: "Menu",
            column: "ParentId");

        migrationBuilder.AddForeignKey(
            name: "FK_Menu_Menu_ParentId",
            table: "Menu",
            column: "ParentId",
            principalTable: "Menu",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Menu_Menu_ParentId",
            table: "Menu");

        migrationBuilder.DropTable(
            name: "DataProtectionKeys");

        migrationBuilder.DropIndex(
            name: "IX_Menu_ParentId",
            table: "Menu");

        migrationBuilder.DropColumn(
            name: "IsPublished",
            table: "Post");

        migrationBuilder.DropColumn(
            name: "IsPublished",
            table: "Comment");

        migrationBuilder.AddColumn<int>(
            name: "ParentMenuId",
            table: "Menu",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Menu_ParentMenuId",
            table: "Menu",
            column: "ParentMenuId");

        migrationBuilder.AddForeignKey(
            name: "FK_Menu_Menu_ParentMenuId",
            table: "Menu",
            column: "ParentMenuId",
            principalTable: "Menu",
            principalColumn: "Id");
    }
}
