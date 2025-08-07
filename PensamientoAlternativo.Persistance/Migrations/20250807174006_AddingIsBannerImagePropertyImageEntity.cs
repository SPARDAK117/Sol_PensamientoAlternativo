using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensamientoAlternativo.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsBannerImagePropertyImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBannerImage",
                table: "Images",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBannerImage",
                table: "Images");
        }
    }
}
