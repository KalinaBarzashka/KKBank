using Microsoft.EntityFrameworkCore.Migrations;

namespace KKBank.Data.Migrations
{
    public partial class AddedAccountIdInAccRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                schema: "17118069",
                table: "AccountRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRequests_AccountId",
                schema: "17118069",
                table: "AccountRequests",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRequests_Accounts_AccountId",
                schema: "17118069",
                table: "AccountRequests",
                column: "AccountId",
                principalSchema: "17118069",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRequests_Accounts_AccountId",
                schema: "17118069",
                table: "AccountRequests");

            migrationBuilder.DropIndex(
                name: "IX_AccountRequests_AccountId",
                schema: "17118069",
                table: "AccountRequests");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "17118069",
                table: "AccountRequests");
        }
    }
}
