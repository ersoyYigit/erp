using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class purchaserequestadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.CreateTable(
                name: "ProductSearchResultDtos",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementUnitName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementUnitSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementUnitId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockUnitCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PurchaseProcessResults",
                columns: table => new
                {
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false),
                    PurchaseRequestDocNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRequestDocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequesterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOfferId = table.Column<int>(type: "int", nullable: true),
                    PurchaseOfferDocNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOfferDocDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseOfferCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOfferSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOfferCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: true),
                    PurchaseOrderDocNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderDocDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseOrderCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOrderCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseEntryId = table.Column<int>(type: "int", nullable: true),
                    WarehouseEntryDocNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseEntryDocDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarehouseName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PurchaseProcessRowResults",
                columns: table => new
                {
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRequestQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductMeasurementUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOfferQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOfferPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOfferCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOfferExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOfferDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOrderPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOrderCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseOrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseEntryQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WarehouseEntryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
            */
            migrationBuilder.CreateTable(
                name: "WarehouseRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    RelatedWarehouseId = table.Column<int>(type: "int", nullable: false),
                    WarehouseOfficerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseOfficerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseReceiptType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseRequests_BaseDocs_Id",
                        column: x => x.Id,
                        principalTable: "BaseDocs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseRequests_Warehouses_RelatedWarehouseId",
                        column: x => x.RelatedWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseRequests_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarehouseRequestRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNo = table.Column<int>(type: "int", nullable: false),
                    WarehouseRequestId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    MeasurementUnitId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RackId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseRequestRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseRequestRows_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseRequestRows_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseRequestRows_Racks_RackId",
                        column: x => x.RackId,
                        principalTable: "Racks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseRequestRows_WarehouseRequests_WarehouseRequestId",
                        column: x => x.WarehouseRequestId,
                        principalTable: "WarehouseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestRows_MeasurementUnitId",
                table: "WarehouseRequestRows",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestRows_ProductId",
                table: "WarehouseRequestRows",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestRows_RackId",
                table: "WarehouseRequestRows",
                column: "RackId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestRows_WarehouseRequestId",
                table: "WarehouseRequestRows",
                column: "WarehouseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequests_RelatedWarehouseId",
                table: "WarehouseRequests",
                column: "RelatedWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequests_WarehouseId",
                table: "WarehouseRequests",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropTable(
                name: "ProductSearchResultDtos");

            migrationBuilder.DropTable(
                name: "PurchaseProcessResults");

            migrationBuilder.DropTable(
                name: "PurchaseProcessRowResults");
            */
            migrationBuilder.DropTable(
                name: "WarehouseRequestRows");

            migrationBuilder.DropTable(
                name: "WarehouseRequests");
        }
    }
}
