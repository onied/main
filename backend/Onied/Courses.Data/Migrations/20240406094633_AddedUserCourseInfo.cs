using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserCourseInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_course_user_courses_courses_id",
                table: "course_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_course_user",
                table: "course_user");

            migrationBuilder.DropIndex(
                name: "ix_course_user_user_id",
                table: "course_user");

            migrationBuilder.RenameColumn(
                name: "courses_id",
                table: "course_user",
                newName: "course_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_course_user",
                table: "course_user",
                columns: new[] { "user_id", "course_id" });

            migrationBuilder.CreateTable(
                name: "user_task_points",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_task_points", x => new { x.user_id, x.task_id });
                    table.ForeignKey(
                        name: "fk_user_task_points_course_user_user_id_course_id",
                        columns: x => new { x.user_id, x.course_id },
                        principalTable: "course_user",
                        principalColumns: new[] { "user_id", "course_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_task_points_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_task_points_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_task_points_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_course_user_course_id",
                table: "course_user",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_task_points_course_id",
                table: "user_task_points",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_task_points_task_id",
                table: "user_task_points",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_task_points_user_id_course_id",
                table: "user_task_points",
                columns: new[] { "user_id", "course_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_course_user_courses_course_id",
                table: "course_user",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_course_user_courses_course_id",
                table: "course_user");

            migrationBuilder.DropTable(
                name: "user_task_points");

            migrationBuilder.DropPrimaryKey(
                name: "pk_course_user",
                table: "course_user");

            migrationBuilder.DropIndex(
                name: "ix_course_user_course_id",
                table: "course_user");

            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "course_user",
                newName: "courses_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_course_user",
                table: "course_user",
                columns: new[] { "courses_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_course_user_user_id",
                table: "course_user",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_course_user_courses_courses_id",
                table: "course_user",
                column: "courses_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
