using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Support.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceSupportUserPropertyWithView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.Sql(@"
create view messages_view as
    select
    messages.*
    , support_users.""number"" as support_number
from
    messages
left join support_users on
    support_users.id = messages.user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.Sql("drop view messages_view");
        }
    }
}
