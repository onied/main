using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    /// <inheritdoc />
    public partial class InheritedManualReviewAnswerFromUserTaskPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_user_task_points_user_id_course_id",
                table: "user_task_points",
                newName: "IX_user_task_points_user_id_course_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_task_points_task_id",
                table: "user_task_points",
                newName: "IX_user_task_points_task_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_task_points_course_id",
                table: "user_task_points",
                newName: "IX_user_task_points_course_id");

            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "user_task_points",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "discriminator",
                table: "user_task_points",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "user_task_points",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "user_task_points");

            migrationBuilder.DropColumn(
                name: "discriminator",
                table: "user_task_points");

            migrationBuilder.DropColumn(
                name: "id",
                table: "user_task_points");

            migrationBuilder.RenameIndex(
                name: "IX_user_task_points_user_id_course_id",
                table: "user_task_points",
                newName: "ix_user_task_points_user_id_course_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_task_points_task_id",
                table: "user_task_points",
                newName: "ix_user_task_points_task_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_task_points_course_id",
                table: "user_task_points",
                newName: "ix_user_task_points_course_id");
        }
    }
}
