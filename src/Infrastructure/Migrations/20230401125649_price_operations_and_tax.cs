using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class price_operations_and_tax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "WarehouseReceiptRows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "WarehouseReceiptRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "WarehouseReceiptRows",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "WarehouseReceiptRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "WarehouseReceiptRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TaxId",
                table: "WarehouseReceiptRows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "WarehouseReceiptRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseOrders",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "PurchaseOrderRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "PurchaseOrderRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "PurchaseOrderRows",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseOrderRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PrevRowId",
                table: "PurchaseOrderRows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "PurchaseOrderRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TaxId",
                table: "PurchaseOrderRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "PurchaseOrderRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "PurchaseOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExchangeDate",
                table: "PurchaseOffers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseOffers",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "PurchaseOfferRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "PurchaseOfferRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "PurchaseOfferRows",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseOfferRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "PurchaseOfferRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TaxId",
                table: "PurchaseOfferRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "PurchaseOfferRows",
                type: "decimal(18,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percent = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptRows_CurrencyId",
                table: "WarehouseReceiptRows",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptRows_TaxId",
                table: "WarehouseReceiptRows",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CurrencyId",
                table: "PurchaseOrders",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRows_CurrencyId",
                table: "PurchaseOrderRows",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRows_TaxId",
                table: "PurchaseOrderRows",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOffers_CurrencyId",
                table: "PurchaseOffers",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOfferRows_CurrencyId",
                table: "PurchaseOfferRows",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOfferRows_TaxId",
                table: "PurchaseOfferRows",
                column: "TaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOfferRows_Currencies_CurrencyId",
                table: "PurchaseOfferRows",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOfferRows_Taxes_TaxId",
                table: "PurchaseOfferRows",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOffers_Currencies_CurrencyId",
                table: "PurchaseOffers",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderRows_Currencies_CurrencyId",
                table: "PurchaseOrderRows",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderRows_Taxes_TaxId",
                table: "PurchaseOrderRows",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Currencies_CurrencyId",
                table: "PurchaseOrders",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceiptRows_Currencies_CurrencyId",
                table: "WarehouseReceiptRows",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceiptRows_Taxes_TaxId",
                table: "WarehouseReceiptRows",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOfferRows_Currencies_CurrencyId",
                table: "PurchaseOfferRows");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOfferRows_Taxes_TaxId",
                table: "PurchaseOfferRows");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOffers_Currencies_CurrencyId",
                table: "PurchaseOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderRows_Currencies_CurrencyId",
                table: "PurchaseOrderRows");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderRows_Taxes_TaxId",
                table: "PurchaseOrderRows");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Currencies_CurrencyId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceiptRows_Currencies_CurrencyId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceiptRows_Taxes_TaxId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceiptRows_CurrencyId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceiptRows_TaxId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_CurrencyId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderRows_CurrencyId",
                table: "PurchaseOrderRows");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderRows_TaxId",
                table: "PurchaseOrderRows");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOffers_CurrencyId",
                table: "PurchaseOffers");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOfferRows_CurrencyId",
                table: "PurchaseOfferRows");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOfferRows_TaxId",
                table: "PurchaseOfferRows");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExchangeDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "PrevRowId",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "PurchaseOrderRows");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "PurchaseOffers");

            migrationBuilder.DropColumn(
                name: "ExchangeDate",
                table: "PurchaseOffers");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "PurchaseOffers");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "PurchaseOfferRows");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "PurchaseOfferRows");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "PurchaseOfferRows");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "PurchaseOfferRows");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "PurchaseOfferRows");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "PurchaseOfferRows");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "PurchaseOfferRows");
        }
    }
}
