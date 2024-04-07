using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "index",
                table: "blocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 1,
                column: "index",
                value: 0);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 2,
                column: "index",
                value: 1);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 3,
                column: "index",
                value: 2);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 4,
                column: "index",
                value: 3);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 5,
                column: "index",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "index",
                table: "blocks");
        }
    }
}
