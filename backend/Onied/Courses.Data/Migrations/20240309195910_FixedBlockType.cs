using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedBlockType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 1,
                column: "block_type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 2,
                column: "block_type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 3,
                column: "block_type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 4,
                column: "block_type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 5,
                column: "block_type",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "blocks",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "blocks",
                columns: new[] { "id", "block_type", "file_href", "file_name", "is_completed", "markdown_text", "module_id", "title" },
                values: new object[] { 1, 0, "/assets/react.svg", "file_name.svg", false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.", 1, "Титульник" });

            migrationBuilder.InsertData(
                table: "blocks",
                columns: new[] { "id", "block_type", "is_completed", "module_id", "title", "url" },
                values: new object[,]
                {
                    { 2, 1, true, 1, "MAKIMA BEAN", "https://www.youtube.com/watch?v=YfBlwC44gDQ" },
                    { 3, 1, false, 1, "Техас покидает родную гавань", "https://vk.com/video-50883936_456243146" },
                    { 4, 1, false, 1, "Александр Асафов о предстоящих президентских выборах", "https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd" }
                });

            migrationBuilder.InsertData(
                table: "blocks",
                columns: new[] { "id", "block_type", "is_completed", "module_id", "title" },
                values: new object[] { 5, 2, false, 1, "Заголовок блока с заданиями" });
        }
    }
}
