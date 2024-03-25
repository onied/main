using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class MovedAuthorToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("198252ef-ed29-47e5-98bc-8b49109a1958"));

            migrationBuilder.DropColumn(
                name: "discriminator",
                table: "users");

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1,
                column: "author_id",
                value: new Guid("442adaca-06d8-43b4-8e12-faebbaad930b"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar_href", "first_name", "last_name" },
                values: new object[] { new Guid("442adaca-06d8-43b4-8e12-faebbaad930b"), "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg", "Василий", "Теркин" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("442adaca-06d8-43b4-8e12-faebbaad930b"));

            migrationBuilder.AddColumn<string>(
                name: "discriminator",
                table: "users",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1,
                column: "author_id",
                value: new Guid("198252ef-ed29-47e5-98bc-8b49109a1958"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar_href", "discriminator", "first_name", "last_name" },
                values: new object[] { new Guid("198252ef-ed29-47e5-98bc-8b49109a1958"), "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg", "Author", "Василий", "Теркин" });
        }
    }
}
