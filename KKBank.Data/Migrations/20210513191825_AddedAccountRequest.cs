using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KKBank.Data.Migrations
{
    public partial class AddedAccountRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddAccountRequests",
                schema: "17118069");

            migrationBuilder.CreateTable(
                name: "AccountRequests",
                schema: "17118069",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    SignedFromBankEmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRequests_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalSchema: "17118069",
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRequests_AspNetUsers_SignedFromBankEmployeeId",
                        column: x => x.SignedFromBankEmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRequests_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "17118069",
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRequests_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalSchema: "17118069",
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRequests_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "17118069",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRequests_AccountTypeId",
                schema: "17118069",
                table: "AccountRequests",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRequests_CurrencyId",
                schema: "17118069",
                table: "AccountRequests",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRequests_RequestTypeId",
                schema: "17118069",
                table: "AccountRequests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRequests_SignedFromBankEmployeeId",
                schema: "17118069",
                table: "AccountRequests",
                column: "SignedFromBankEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRequests_StatusId",
                schema: "17118069",
                table: "AccountRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRequests_UserId",
                schema: "17118069",
                table: "AccountRequests",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRequests",
                schema: "17118069");

            migrationBuilder.CreateTable(
                name: "AddAccountRequests",
                schema: "17118069",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    SignedFromBankEmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddAccountRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddAccountRequests_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalSchema: "17118069",
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddAccountRequests_AspNetUsers_SignedFromBankEmployeeId",
                        column: x => x.SignedFromBankEmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddAccountRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddAccountRequests_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "17118069",
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddAccountRequests_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalSchema: "17118069",
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddAccountRequests_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "17118069",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddAccountRequests_AccountTypeId",
                schema: "17118069",
                table: "AddAccountRequests",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddAccountRequests_CurrencyId",
                schema: "17118069",
                table: "AddAccountRequests",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AddAccountRequests_RequestTypeId",
                schema: "17118069",
                table: "AddAccountRequests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddAccountRequests_SignedFromBankEmployeeId",
                schema: "17118069",
                table: "AddAccountRequests",
                column: "SignedFromBankEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddAccountRequests_StatusId",
                schema: "17118069",
                table: "AddAccountRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AddAccountRequests_UserId",
                schema: "17118069",
                table: "AddAccountRequests",
                column: "UserId");
        }
    }
}
