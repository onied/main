using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Support.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSupportUserInMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "support_user_id",
                table: "messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_messages_support_user_id",
                table: "messages",
                column: "support_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_support_users_support_user_id",
                table: "messages",
                column: "support_user_id",
                principalTable: "support_users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_support_users_support_user_id",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_support_user_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "support_user_id",
                table: "messages");
        }
    }
}
