using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class warehouse_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "WarehouseReceipts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequesterDepartment",
                table: "WarehouseReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequesterId",
                table: "WarehouseReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequesterName",
                table: "WarehouseReceipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderRowId",
                table: "WarehouseReceiptRows",
                type: "int",
                nullable: true);
            /*
            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_PurchaseOrderId",
                table: "WarehouseReceipts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptRows_PurchaseOrderRowId",
                table: "WarehouseReceiptRows",
                column: "PurchaseOrderRowId");
            */
            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceiptRows_PurchaseOrderRows_PurchaseOrderRowId",
                table: "WarehouseReceiptRows",
                column: "PurchaseOrderRowId",
                principalTable: "PurchaseOrderRows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceipts_PurchaseOrders_PurchaseOrderId",
                table: "WarehouseReceipts",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceiptRows_PurchaseOrderRows_PurchaseOrderRowId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceipts_PurchaseOrders_PurchaseOrderId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceipts_PurchaseOrderId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceiptRows_PurchaseOrderRowId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "RequesterDepartment",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "RequesterName",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderRowId",
                table: "WarehouseReceiptRows");
        }
    }
}
