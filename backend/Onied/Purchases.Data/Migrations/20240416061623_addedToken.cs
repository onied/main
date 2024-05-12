using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchases.Migrations
{
    /// <inheritdoc />
    public partial class addedToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "purchases",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "token",
                table: "purchases");
        }
    }
}
