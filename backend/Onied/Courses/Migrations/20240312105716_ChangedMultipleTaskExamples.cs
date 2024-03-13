using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMultipleTaskExamples : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 1,
                column: "description",
                value: "Ничего 1");

            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 2,
                column: "description",
                value: "Ничего 2");

            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 3,
                column: "description",
                value: "Ничего 3");

            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 4,
                column: "description",
                value: "Ничего 4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 1,
                column: "description",
                value: "Ничего");

            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 2,
                column: "description",
                value: "Ничего");

            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 3,
                column: "description",
                value: "Ничего");

            migrationBuilder.UpdateData(
                table: "task_variants",
                keyColumn: "id",
                keyValue: 4,
                column: "description",
                value: "Ничего");
        }
    }
}
