using Microsoft.EntityFrameworkCore.Migrations;

namespace KKBank.Data.Migrations
{
    public partial class AddedColumnsToPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentOrders_Currency_ToAccountCurrencyId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaymentOrders_ToAccountCurrencyId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropColumn(
                name: "ToAccountCurrencyId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "FromAmount");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyFromId",
                schema: "17118069",
                table: "PaymentOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyToId",
                schema: "17118069",
                table: "PaymentOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ToAmount",
                schema: "17118069",
                table: "PaymentOrders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrders_CurrencyFromId",
                schema: "17118069",
                table: "PaymentOrders",
                column: "CurrencyFromId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrders_CurrencyToId",
                schema: "17118069",
                table: "PaymentOrders",
                column: "CurrencyToId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentOrders_Currency_CurrencyFromId",
                schema: "17118069",
                table: "PaymentOrders",
                column: "CurrencyFromId",
                principalSchema: "17118069",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentOrders_Currency_CurrencyToId",
                schema: "17118069",
                table: "PaymentOrders",
                column: "CurrencyToId",
                principalSchema: "17118069",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentOrders_Currency_CurrencyFromId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentOrders_Currency_CurrencyToId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaymentOrders_CurrencyFromId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropIndex(
                name: "IX_PaymentOrders_CurrencyToId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropColumn(
                name: "CurrencyFromId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropColumn(
                name: "CurrencyToId",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.DropColumn(
                name: "ToAmount",
                schema: "17118069",
                table: "PaymentOrders");

            migrationBuilder.RenameColumn(
                name: "FromAmount",
                schema: "17118069",
                table: "PaymentOrders",
                newName: "Amount");

            migrationBuilder.AddColumn<int>(
                name: "ToAccountCurrencyId",
                schema: "17118069",
                table: "PaymentOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrders_ToAccountCurrencyId",
                schema: "17118069",
                table: "PaymentOrders",
                column: "ToAccountCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentOrders_Currency_ToAccountCurrencyId",
                schema: "17118069",
                table: "PaymentOrders",
                column: "ToAccountCurrencyId",
                principalSchema: "17118069",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
