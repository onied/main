using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCourseCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "courses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_date",
                table: "courses");
        }
    }
}
