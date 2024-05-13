using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchases.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedTwoFieldsForSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "auto_tests_review",
                table: "subscriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "students_on_course_limit",
                table: "subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "auto_tests_review", "students_on_course_limit" },
                values: new object[] { true, -1 });

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "auto_tests_review", "students_on_course_limit" },
                values: new object[] { true, -1 });

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "auto_tests_review", "students_on_course_limit" },
                values: new object[] { true, -1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "auto_tests_review",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "students_on_course_limit",
                table: "subscriptions");
        }
    }
}
