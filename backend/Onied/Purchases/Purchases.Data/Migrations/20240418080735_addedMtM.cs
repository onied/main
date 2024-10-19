using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchases.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedMtM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_course_infos",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_course_infos", x => new { x.user_id, x.course_id });
                    table.ForeignKey(
                        name: "fk_user_course_infos_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_course_infos_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_course_infos_course_id",
                table: "user_course_infos",
                column: "course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_course_infos");
        }
    }
}
