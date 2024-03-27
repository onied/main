using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class FixedBugsAddedOnDeleteRuleAddedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("198252ef-ed29-47e5-98bc-8b49109a1958"));

            migrationBuilder.DropColumn(
                name: "discriminator",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "avatar_href",
                table: "users",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AddColumn<int>(
                name: "gender",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "author_id",
                table: "courses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1,
                column: "author_id",
                value: new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar_href", "first_name", "gender", "last_name" },
                values: new object[] { new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"), "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg", "Василий", null, "Теркин" });

            migrationBuilder.AddForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"));

            migrationBuilder.DropColumn(
                name: "gender",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "avatar_href",
                table: "users",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "discriminator",
                table: "users",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "author_id",
                table: "courses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
