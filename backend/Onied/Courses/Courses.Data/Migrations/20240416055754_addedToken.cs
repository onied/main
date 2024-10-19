using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "course_user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "token",
                table: "course_user");
        }
    }
}
