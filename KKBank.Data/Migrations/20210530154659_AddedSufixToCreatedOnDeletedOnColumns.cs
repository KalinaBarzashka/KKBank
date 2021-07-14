using Microsoft.EntityFrameworkCore.Migrations;

namespace KKBank.Data.Migrations
{
    public partial class AddedSufixToCreatedOnDeletedOnColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "RequestTypes",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "RequestTypes",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "RequestTypes",
                newName: "CreatedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "CreatedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "CreatedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "Currency",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "Currency",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "Currency",
                newName: "CreatedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "AccountTypes",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "AccountTypes",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "AccountTypes",
                newName: "CreatedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "Accounts",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "Accounts",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "Accounts",
                newName: "CreatedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "CreatedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "17118069",
                table: "AccountRequests",
                newName: "IsDeleted_17118069");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "17118069",
                table: "AccountRequests",
                newName: "DeletedOn_17118069");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "17118069",
                table: "AccountRequests",
                newName: "CreatedOn_17118069");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "RequestTypes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "RequestTypes",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "RequestTypes",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "PaymentOrderStatus",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "Currency",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "Currency",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "Currency",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "AccountTypes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "AccountTypes",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "AccountTypes",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "Accounts",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "Accounts",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "Accounts",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "AccountRequestStatus",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted_17118069",
                schema: "17118069",
                table: "AccountRequests",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOn_17118069",
                schema: "17118069",
                table: "AccountRequests",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedOn_17118069",
                schema: "17118069",
                table: "AccountRequests",
                newName: "CreatedOn");
        }
    }
}
