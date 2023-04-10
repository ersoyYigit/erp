using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _2_warehouseEntryReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Categories_CategoryId",
                table: "Persons");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

           

            migrationBuilder.CreateTable(
                name: "WarehouseReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WarehouseReceiptType = table.Column<int>(type: "int", nullable: false),
                    WayBillNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WayBillDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    RelatedWarehouseId = table.Column<int>(type: "int", nullable: true),
                    WarehouseOfficerId = table.Column<int>(type: "int", nullable: true),
                    RelatedCompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseReceipts_BaseDocs_Id",
                        column: x => x.Id,
                        principalTable: "BaseDocs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseReceipts_Companies_RelatedCompanyId",
                        column: x => x.RelatedCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseReceipts_Persons_WarehouseOfficerId",
                        column: x => x.WarehouseOfficerId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseReceipts_Warehouses_RelatedWarehouseId",
                        column: x => x.RelatedWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseReceipts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseReceiptRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNo = table.Column<int>(type: "int", nullable: false),
                    WarehouseReceiptId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MeasurementUnitId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RackId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseReceiptRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseReceiptRows_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseReceiptRows_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseReceiptRows_Racks_RackId",
                        column: x => x.RackId,
                        principalTable: "Racks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseReceiptRows_WarehouseReceipts_WarehouseReceiptId",
                        column: x => x.WarehouseReceiptId,
                        principalTable: "WarehouseReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptRows_MeasurementUnitId",
                table: "WarehouseReceiptRows",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptRows_ProductId",
                table: "WarehouseReceiptRows",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptRows_RackId",
                table: "WarehouseReceiptRows",
                column: "RackId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptRows_WarehouseReceiptId",
                table: "WarehouseReceiptRows",
                column: "WarehouseReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_RelatedCompanyId",
                table: "WarehouseReceipts",
                column: "RelatedCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_RelatedWarehouseId",
                table: "WarehouseReceipts",
                column: "RelatedWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_WarehouseId",
                table: "WarehouseReceipts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_WarehouseOfficerId",
                table: "WarehouseReceipts",
                column: "WarehouseOfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Categories_CategoryId",
                table: "Persons",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Categories_CategoryId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "WarehouseReceiptRows");

            migrationBuilder.DropTable(
                name: "WarehouseReceipts");


            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Categories_CategoryId",
                table: "Persons",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
