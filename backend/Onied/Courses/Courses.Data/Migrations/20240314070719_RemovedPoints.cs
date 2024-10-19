using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "points",
                table: "tasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "points",
                table: "tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "id",
                keyValue: 1,
                column: "points",
                value: null);

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "id",
                keyValue: 2,
                column: "points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "id",
                keyValue: 3,
                column: "points",
                value: 5);

            migrationBuilder.UpdateData(
                table: "tasks",
                keyColumn: "id",
                keyValue: 4,
                column: "points",
                value: null);
        }
    }
}
