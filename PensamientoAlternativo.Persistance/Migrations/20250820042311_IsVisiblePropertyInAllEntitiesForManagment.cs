using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensamientoAlternativo.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class IsVisiblePropertyInAllEntitiesForManagment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Images",
                newName: "IsVisible");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Opinions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Faqs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Opinions");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Faqs");

            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "Images",
                newName: "IsActive");
        }
    }
}
