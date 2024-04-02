using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class AddedModerators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course_moderator",
                columns: table => new
                {
                    moderating_courses_id = table.Column<int>(type: "integer", nullable: false),
                    user1id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_moderator", x => new { x.moderating_courses_id, x.user1id });
                    table.ForeignKey(
                        name: "fk_course_moderator_courses_moderating_courses_id",
                        column: x => x.moderating_courses_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_moderator_users_user1id",
                        column: x => x.user1id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_course_moderator_user1id",
                table: "course_moderator",
                column: "user1id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_moderator");
        }
    }
}
