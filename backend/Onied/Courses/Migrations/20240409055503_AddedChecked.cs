using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class AddedChecked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_completed",
                table: "blocks");

            migrationBuilder.AddColumn<bool>(
                name: "checked",
                table: "user_task_points",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "checked",
                table: "user_task_points");

            migrationBuilder.AddColumn<bool>(
                name: "is_completed",
                table: "blocks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 1,
                column: "is_completed",
                value: false);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 2,
                column: "is_completed",
                value: false);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 3,
                column: "is_completed",
                value: false);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 4,
                column: "is_completed",
                value: false);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 5,
                column: "is_completed",
                value: false);
        }
    }
}
