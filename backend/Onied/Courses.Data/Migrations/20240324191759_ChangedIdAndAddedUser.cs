using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIdAndAddedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_authors_author_id",
                table: "courses");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.RenameIndex(
                name: "ix_tasks_tasks_block_id",
                table: "tasks",
                newName: "IX_tasks_tasks_block_id");

            migrationBuilder.RenameIndex(
                name: "ix_blocks_module_id",
                table: "blocks",
                newName: "IX_blocks_module_id");

            migrationBuilder.AlterColumn<string>(
                name: "discriminator",
                table: "tasks",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            /*migrationBuilder.AlterColumn<Guid>(
                name: "author_id",
                table: "courses",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");*/

            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");

            migrationBuilder.Sql(
                "ALTER TABLE courses ALTER COLUMN author_id DROP DEFAULT, ALTER COLUMN author_id SET DATA TYPE UUID USING (uuid_generate_v4()), ALTER COLUMN author_id SET DEFAULT uuid_generate_v4();");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    avatar_href = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course_user",
                columns: table => new
                {
                    courses_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_user", x => new { x.courses_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_course_user_courses_courses_id",
                        column: x => x.courses_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_user_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1,
                column: "author_id",
                value: new Guid("198252ef-ed29-47e5-98bc-8b49109a1958"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar_href", "discriminator", "first_name", "last_name" },
                values: new object[] { new Guid("198252ef-ed29-47e5-98bc-8b49109a1958"), "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg", "Author", "Василий", "Теркин" });

            migrationBuilder.CreateIndex(
                name: "ix_course_user_user_id",
                table: "course_user",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses");

            migrationBuilder.DropTable(
                name: "course_user");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.RenameIndex(
                name: "IX_tasks_tasks_block_id",
                table: "tasks",
                newName: "ix_tasks_tasks_block_id");

            migrationBuilder.RenameIndex(
                name: "IX_blocks_module_id",
                table: "blocks",
                newName: "ix_blocks_module_id");

            migrationBuilder.AlterColumn<string>(
                name: "discriminator",
                table: "tasks",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);

            migrationBuilder.AlterColumn<int>(
                name: "author_id",
                table: "courses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    avatar_href = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authors", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "authors",
                columns: new[] { "id", "avatar_href", "first_name", "last_name" },
                values: new object[] { 1, "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg", "Василий", "Теркин" });

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1,
                column: "author_id",
                value: 1);

            migrationBuilder.AddForeignKey(
                name: "fk_courses_authors_author_id",
                table: "courses",
                column: "author_id",
                principalTable: "authors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
