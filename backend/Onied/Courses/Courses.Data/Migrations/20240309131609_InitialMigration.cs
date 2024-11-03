using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_modules", x => x.id);
                    table.ForeignKey(
                        name: "fk_modules_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_id = table.Column<int>(type: "integer", nullable: false),
                    block_type = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    markdown_text = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    file_href = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_blocks", x => x.id);
                    table.ForeignKey(
                        name: "fk_blocks_modules_module_id",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tasks_block_id = table.Column<int>(type: "integer", nullable: false),
                    task_type = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    points = table.Column<int>(type: "integer", nullable: true),
                    max_points = table.Column<int>(type: "integer", nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_tasks_blocks_tasks_block_id",
                        column: x => x.tasks_block_id,
                        principalTable: "blocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_text_input_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    answer = table.Column<string>(type: "text", nullable: false),
                    is_case_sensitive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_text_input_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_text_input_answers_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_variants",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_variants", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_variants_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "id", "title" },
                values: new object[] { 1, "Название курса. Как я встретил вашу маму. Осуждаю." });

            migrationBuilder.InsertData(
                table: "modules",
                columns: new[] { "id", "course_id", "title" },
                values: new object[,]
                {
                    { 1, 1, "Такой-то" },
                    { 2, 1, "Сякой-то" }
                });

            migrationBuilder.InsertData(
                table: "blocks",
                columns: new[] { "id", "block_type", "discriminator", "file_href", "file_name", "markdown_text", "module_id", "title" },
                values: new object[] { 1, 0, "SummaryBlock", "/assets/react.svg", "file_name.svg", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.", 1, "Титульник" });

            migrationBuilder.InsertData(
                table: "blocks",
                columns: new[] { "id", "block_type", "discriminator", "module_id", "title", "url" },
                values: new object[,]
                {
                    { 2, 1, "VideoBlock", 1, "MAKIMA BEAN", "https://www.youtube.com/watch?v=YfBlwC44gDQ" },
                    { 3, 1, "VideoBlock", 1, "Техас покидает родную гавань", "https://vk.com/video-50883936_456243146" },
                    { 4, 1, "VideoBlock", 1, "Александр Асафов о предстоящих президентских выборах", "https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd" }
                });

            migrationBuilder.InsertData(
                table: "blocks",
                columns: new[] { "id", "block_type", "discriminator", "module_id", "title" },
                values: new object[] { 5, 2, "TasksBlock", 1, "Заголовок блока с заданиями" });

            migrationBuilder.InsertData(
                table: "tasks",
                columns: new[] { "id", "discriminator", "max_points", "points", "task_type", "tasks_block_id", "title" },
                values: new object[,]
                {
                    { 1, "VariantsTask", 1, null, 1, 5, "1. Что произошло на пло́щади Тяньаньмэ́нь в 1989 году?" },
                    { 2, "VariantsTask", 1, 0, 0, 5, "2. Чипи чипи чапа чапа дуби дуби даба даба?" },
                    { 3, "InputTask", 5, 5, 2, 5, "3. Кто?" },
                    { 4, "Task", 1, null, 3, 5, "4. Напишите эссе на тему: “Как я провел лето”" }
                });

            migrationBuilder.InsertData(
                table: "task_text_input_answers",
                columns: new[] { "id", "answer", "is_case_sensitive", "task_id" },
                values: new object[] { 1, "Жак Фреско", true, 3 });

            migrationBuilder.InsertData(
                table: "task_variants",
                columns: new[] { "id", "description", "is_correct", "task_id" },
                values: new object[,]
                {
                    { 1, "Ничего", true, 1 },
                    { 2, "Ничего", true, 1 },
                    { 3, "Ничего", true, 1 },
                    { 4, "Ничего", true, 1 },
                    { 5, "Чипи чипи", true, 2 },
                    { 6, "Чапа чапа", false, 2 },
                    { 7, "Дуби дуби", false, 2 },
                    { 8, "Даба даба", false, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_blocks_module_id",
                table: "blocks",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "ix_modules_course_id",
                table: "modules",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_text_input_answers_task_id",
                table: "task_text_input_answers",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_variants_task_id",
                table: "task_variants",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_tasks_block_id",
                table: "tasks",
                column: "tasks_block_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task_text_input_answers");

            migrationBuilder.DropTable(
                name: "task_variants");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "blocks");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "courses");
        }
    }
}
