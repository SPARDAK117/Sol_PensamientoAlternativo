using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensamientoAlternativo.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ViewSectionOnImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewSection",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewSection",
                table: "Images");
        }
    }
}
