using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KKBank.Data.Migrations
{
    public partial class RenameAccRequestStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRequests_Status_StatusId",
                schema: "17118069",
                table: "AccountRequests");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "17118069");

            migrationBuilder.CreateTable(
                name: "AccountRequestStatus",
                schema: "17118069",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRequestStatus", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRequests_AccountRequestStatus_StatusId",
                schema: "17118069",
                table: "AccountRequests",
                column: "StatusId",
                principalSchema: "17118069",
                principalTable: "AccountRequestStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRequests_AccountRequestStatus_StatusId",
                schema: "17118069",
                table: "AccountRequests");

            migrationBuilder.DropTable(
                name: "AccountRequestStatus",
                schema: "17118069");

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "17118069",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRequests_Status_StatusId",
                schema: "17118069",
                table: "AccountRequests",
                column: "StatusId",
                principalSchema: "17118069",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
