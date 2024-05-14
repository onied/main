using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoleAndDefaultAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[] { "b7e95e9c-19fb-4b3d-81a4-ccb77379f4c6", "eb9e8982-218e-47f8-8d4f-75055b023f98", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "id", "access_failed_count", "avatar", "concurrency_stamp", "email", "email_confirmed", "first_name", "gender", "last_name", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[] { "a544f3f9-3c5f-4eda-9166-082210d50588", 0, null, "b39ac881-780e-40f0-a560-97c9c8018e96", "admin@onied.com", true, "Admin", null, "Admin", false, null, "ADMIN@ONIED.COM", "ADMIN", "AQAAAAIAAYagAAAAEB1qk8whYR9UirUH76sySwGuYSHw4eNSzn8DxjoMy8uw/Vb6bti2B33s582ewoF+zg==", null, false, "FRIXFW6KU665IZU6CDPKR4I7GWKAQPZ4", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { "b7e95e9c-19fb-4b3d-81a4-ccb77379f4c6", "a544f3f9-3c5f-4eda-9166-082210d50588" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { "b7e95e9c-19fb-4b3d-81a4-ccb77379f4c6", "a544f3f9-3c5f-4eda-9166-082210d50588" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "b7e95e9c-19fb-4b3d-81a4-ccb77379f4c6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "id",
                keyValue: "a544f3f9-3c5f-4eda-9166-082210d50588");
        }
    }
}
