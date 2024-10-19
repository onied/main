using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchases.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPurchaseDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "purchase_date",
                table: "purchase_details",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "purchase_date",
                table: "purchase_details");
        }
    }
}
