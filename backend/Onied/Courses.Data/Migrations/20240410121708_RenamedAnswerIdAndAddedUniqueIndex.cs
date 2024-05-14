using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamedAnswerIdAndAddedUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_task_points",
                newName: "manual_review_task_user_answer_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_task_points_manual_review_task_user_answer_id",
                table: "user_task_points",
                column: "manual_review_task_user_answer_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_task_points_manual_review_task_user_answer_id",
                table: "user_task_points");

            migrationBuilder.RenameColumn(
                name: "manual_review_task_user_answer_id",
                table: "user_task_points",
                newName: "id");
        }
    }
}
