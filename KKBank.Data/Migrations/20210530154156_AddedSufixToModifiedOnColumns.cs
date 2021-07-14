using Microsoft.EntityFrameworkCore.Migrations;

namespace KKBank.Data.Migrations
{
    public partial class AddedSufixToModifiedOnColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "RequestTypes",
                newName: "ModifiedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "ModifiedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "ModifiedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "Currency",
                newName: "ModifiedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "AccountTypes",
                newName: "ModifiedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "Accounts",
                newName: "ModifiedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "ModifiedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "17118069",
                table: "AccountRequests",
                newName: "ModifiedOn_17118069");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "RequestTypes",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "Currency",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "AccountTypes",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "Accounts",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn_17118069",
                schema: "17118069",
                table: "AccountRequests",
                newName: "ModifiedOn");
        }
    }
}
