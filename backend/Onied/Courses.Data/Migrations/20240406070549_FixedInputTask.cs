using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedInputTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_case_sensitive",
                table: "task_text_input_answers");

            migrationBuilder.AddColumn<int>(
                name: "accuracy",
                table: "tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_case_sensitive",
                table: "tasks",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_number",
                table: "tasks",
                type: "boolean",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "accuracy", "is_case_sensitive", "is_number" },
                values: new object[] { 0, true, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accuracy",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "is_case_sensitive",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "is_number",
                table: "tasks");

            migrationBuilder.AddColumn<bool>(
                name: "is_case_sensitive",
                table: "task_text_input_answers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "task_text_input_answers",
                keyColumn: "id",
                keyValue: 1,
                column: "is_case_sensitive",
                value: true);
        }
    }
}
