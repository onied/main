using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Purchases.Migrations
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "subscriptions",
                columns: new[] { "id", "active_courses_number", "ads_enabled", "certificates_enabled", "courses_highlighting_enabled", "price", "title" },
                values: new object[,]
                {
                    { 1, 0, false, false, false, 0m, "Микрочелик" },
                    { 2, 3, false, false, false, 2000m, "Я карлик" },
                    { 3, -1, true, true, true, 10000m, "Король инфоцыган" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "subscription_id" },
                values: new object[] { new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"), 1 });

            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "id", "author_id", "has_certificates", "price", "title" },
                values: new object[] { 1, new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"), true, 0m, "Название курса. Как я встретил вашу маму. Осуждаю." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"));

            migrationBuilder.DeleteData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
