using Microsoft.EntityFrameworkCore.Migrations;

namespace csharp_group_homework_64_ivan_kobtsev.Migrations
{
    public partial class ChangeFieldAccountIdInEntityMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_AccountId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AccountId1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AccountId",
                table: "Messages",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_AccountId",
                table: "Messages",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_AccountId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AccountId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountId1",
                table: "Messages",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AccountId1",
                table: "Messages",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_AccountId1",
                table: "Messages",
                column: "AccountId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
