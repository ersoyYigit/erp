using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class warehouse_fix_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceipts_Persons_WarehouseOfficerId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceipts_WarehouseOfficerId",
                table: "WarehouseReceipts");

            migrationBuilder.AlterColumn<string>(
                name: "WarehouseOfficerId",
                table: "WarehouseReceipts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrderDocNo",
                table: "WarehouseReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseRequestDocNo",
                table: "WarehouseReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseRequestId",
                table: "WarehouseReceipts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_PurchaseRequestId",
                table: "WarehouseReceipts",
                column: "PurchaseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceipts_PurchaseRequests_PurchaseRequestId",
                table: "WarehouseReceipts",
                column: "PurchaseRequestId",
                principalTable: "PurchaseRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceipts_PurchaseRequests_PurchaseRequestId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceipts_PurchaseRequestId",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderDocNo",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestDocNo",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestId",
                table: "WarehouseReceipts");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseOfficerId",
                table: "WarehouseReceipts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_WarehouseOfficerId",
                table: "WarehouseReceipts",
                column: "WarehouseOfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceipts_Persons_WarehouseOfficerId",
                table: "WarehouseReceipts",
                column: "WarehouseOfficerId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
