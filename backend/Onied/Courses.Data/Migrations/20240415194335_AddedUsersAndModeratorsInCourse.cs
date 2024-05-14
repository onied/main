using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsersAndModeratorsInCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_course_moderator_users_user1id",
                table: "course_moderator");

            migrationBuilder.RenameColumn(
                name: "user1id",
                table: "course_moderator",
                newName: "moderators_id");

            migrationBuilder.RenameIndex(
                name: "ix_course_moderator_user1id",
                table: "course_moderator",
                newName: "ix_course_moderator_moderators_id");

            migrationBuilder.AddForeignKey(
                name: "fk_course_moderator_users_moderators_id",
                table: "course_moderator",
                column: "moderators_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_course_moderator_users_moderators_id",
                table: "course_moderator");

            migrationBuilder.RenameColumn(
                name: "moderators_id",
                table: "course_moderator",
                newName: "user1id");

            migrationBuilder.RenameIndex(
                name: "ix_course_moderator_moderators_id",
                table: "course_moderator",
                newName: "ix_course_moderator_user1id");

            migrationBuilder.AddForeignKey(
                name: "fk_course_moderator_users_user1id",
                table: "course_moderator",
                column: "user1id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
