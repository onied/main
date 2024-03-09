using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class RemovedBlockType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "block_type",
                table: "blocks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "block_type",
                table: "blocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 1,
                column: "block_type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 2,
                column: "block_type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 3,
                column: "block_type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 4,
                column: "block_type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 5,
                column: "block_type",
                value: 2);
        }
    }
}
