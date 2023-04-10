using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _4_purchase_offer_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequest_BaseDocs_Id",
                table: "PurchaseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestRows_PurchaseRequest_PurchaseRequestId",
                table: "PurchaseRequestRows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseRequest",
                table: "PurchaseRequest");

            migrationBuilder.RenameTable(
                name: "PurchaseRequest",
                newName: "PurchaseRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseRequests",
                table: "PurchaseRequests",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PurchaseOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RequirementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOffers_BaseDocs_Id",
                        column: x => x.Id,
                        principalTable: "BaseDocs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOffers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOffers_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOfferRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNo = table.Column<int>(type: "int", nullable: false),
                    PurchaseOfferId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    MeasurementUnitId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRequestRowId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOfferRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOfferRows_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOfferRows_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOfferRows_PurchaseOffers_PurchaseOfferId",
                        column: x => x.PurchaseOfferId,
                        principalTable: "PurchaseOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOfferRows_PurchaseRequestRows_PurchaseRequestRowId",
                        column: x => x.PurchaseRequestRowId,
                        principalTable: "PurchaseRequestRows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RequirementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequesterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: true),
                    PurchaseOfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_BaseDocs_Id",
                        column: x => x.Id,
                        principalTable: "BaseDocs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseOffers_PurchaseOfferId",
                        column: x => x.PurchaseOfferId,
                        principalTable: "PurchaseOffers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNo = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    MeasurementUnitId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRequestRowId = table.Column<int>(type: "int", nullable: true),
                    PurchaseOfferRowId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderRows_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderRows_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderRows_PurchaseOfferRows_PurchaseOfferRowId",
                        column: x => x.PurchaseOfferRowId,
                        principalTable: "PurchaseOfferRows",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderRows_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderRows_PurchaseRequestRows_PurchaseRequestRowId",
                        column: x => x.PurchaseRequestRowId,
                        principalTable: "PurchaseRequestRows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOfferRows_MeasurementUnitId",
                table: "PurchaseOfferRows",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOfferRows_ProductId",
                table: "PurchaseOfferRows",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOfferRows_PurchaseOfferId",
                table: "PurchaseOfferRows",
                column: "PurchaseOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOfferRows_PurchaseRequestRowId",
                table: "PurchaseOfferRows",
                column: "PurchaseRequestRowId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOffers_CompanyId",
                table: "PurchaseOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOffers_PurchaseRequestId",
                table: "PurchaseOffers",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRows_MeasurementUnitId",
                table: "PurchaseOrderRows",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRows_ProductId",
                table: "PurchaseOrderRows",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRows_PurchaseOfferRowId",
                table: "PurchaseOrderRows",
                column: "PurchaseOfferRowId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRows_PurchaseOrderId",
                table: "PurchaseOrderRows",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRows_PurchaseRequestRowId",
                table: "PurchaseOrderRows",
                column: "PurchaseRequestRowId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CompanyId",
                table: "PurchaseOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOfferId",
                table: "PurchaseOrders",
                column: "PurchaseOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseRequestId",
                table: "PurchaseOrders",
                column: "PurchaseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestRows_PurchaseRequests_PurchaseRequestId",
                table: "PurchaseRequestRows",
                column: "PurchaseRequestId",
                principalTable: "PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_BaseDocs_Id",
                table: "PurchaseRequests",
                column: "Id",
                principalTable: "BaseDocs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestRows_PurchaseRequests_PurchaseRequestId",
                table: "PurchaseRequestRows");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_BaseDocs_Id",
                table: "PurchaseRequests");

            migrationBuilder.DropTable(
                name: "PurchaseOrderRows");

            migrationBuilder.DropTable(
                name: "PurchaseOfferRows");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "PurchaseOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseRequests",
                table: "PurchaseRequests");

            migrationBuilder.RenameTable(
                name: "PurchaseRequests",
                newName: "PurchaseRequest");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseRequest",
                table: "PurchaseRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequest_BaseDocs_Id",
                table: "PurchaseRequest",
                column: "Id",
                principalTable: "BaseDocs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestRows_PurchaseRequest_PurchaseRequestId",
                table: "PurchaseRequestRows",
                column: "PurchaseRequestId",
                principalTable: "PurchaseRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
