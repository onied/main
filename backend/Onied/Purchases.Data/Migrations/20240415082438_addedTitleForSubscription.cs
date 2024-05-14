using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchases.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedTitleForSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "subscriptions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "subscriptions");
        }
    }
}
