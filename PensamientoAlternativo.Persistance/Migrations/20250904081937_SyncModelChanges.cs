using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensamientoAlternativo.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Opinions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OpinionText2",
                table: "Opinions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OpinionText3",
                table: "Opinions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptTermsAndConditions",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailNotificationsAvailable",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_CustomerId",
                table: "Opinions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Opinions_Customers_CustomerId",
                table: "Opinions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opinions_Customers_CustomerId",
                table: "Opinions");

            migrationBuilder.DropIndex(
                name: "IX_Opinions_CustomerId",
                table: "Opinions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Opinions");

            migrationBuilder.DropColumn(
                name: "OpinionText2",
                table: "Opinions");

            migrationBuilder.DropColumn(
                name: "OpinionText3",
                table: "Opinions");

            migrationBuilder.DropColumn(
                name: "AcceptTermsAndConditions",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsEmailNotificationsAvailable",
                table: "Customers");
        }
    }
}
