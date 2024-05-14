using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpgradedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "author_id",
                table: "courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "courses",
                type: "character varying(15000)",
                maxLength: 15000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "has_certificates",
                table: "courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "hours_count",
                table: "courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_glowing",
                table: "courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "picture_href",
                table: "courses",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "price_rubles",
                table: "courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    avatar_href = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "authors",
                columns: new[] { "id", "avatar_href", "first_name", "last_name" },
                values: new object[] { 1, "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg", "Василий", "Теркин" });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "цифровые технологии" });

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "author_id", "category_id", "description", "has_certificates", "hours_count", "is_archived", "is_glowing", "picture_href", "price_rubles" },
                values: new object[] { 1, 1, "Описание курса. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus. Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.", true, 100, true, false, "https://upload.wikimedia.org/wikipedia/commons/f/fa/Kitten_sleeping.jpg", 0 });

            migrationBuilder.CreateIndex(
                name: "ix_courses_author_id",
                table: "courses",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_courses_category_id",
                table: "courses",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_courses_authors_author_id",
                table: "courses",
                column: "author_id",
                principalTable: "authors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_courses_categories_category_id",
                table: "courses",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_authors_author_id",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "fk_courses_categories_category_id",
                table: "courses");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "ix_courses_author_id",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "ix_courses_category_id",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "author_id",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "description",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "has_certificates",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "hours_count",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "is_glowing",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "picture_href",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "price_rubles",
                table: "courses");
        }
    }
}
