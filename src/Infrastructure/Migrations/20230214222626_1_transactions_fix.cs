using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _1_transactions_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseDocs_ProductionLines_ProductionLineId",
                table: "BaseDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseDocs_Products_ConsumeProductId",
                table: "BaseDocs");

            migrationBuilder.DropIndex(
                name: "IX_BaseDocs_ConsumeProductId",
                table: "BaseDocs");

            migrationBuilder.DropIndex(
                name: "IX_BaseDocs_ProductionLineId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "ConsumeProductId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "PatternId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "ProductionLineId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "ResponseUserId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "WorkEndDate",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "WorkStartDate",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "templateWorkOrderType",
                table: "BaseDocs");

            migrationBuilder.CreateTable(
                name: "TemplateWorkOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponseUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    templateWorkOrderType = table.Column<int>(type: "int", nullable: false),
                    WorkStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConsumeProductId = table.Column<int>(type: "int", nullable: true),
                    ProductionLineId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatternId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateWorkOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateWorkOrder_BaseDocs_Id",
                        column: x => x.Id,
                        principalTable: "BaseDocs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemplateWorkOrder_ProductionLines_ProductionLineId",
                        column: x => x.ProductionLineId,
                        principalTable: "ProductionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateWorkOrder_Products_ConsumeProductId",
                        column: x => x.ConsumeProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateWorkOrder_ConsumeProductId",
                table: "TemplateWorkOrder",
                column: "ConsumeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateWorkOrder_ProductionLineId",
                table: "TemplateWorkOrder",
                column: "ProductionLineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateWorkOrder");

            migrationBuilder.AddColumn<int>(
                name: "ConsumeProductId",
                table: "BaseDocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BaseDocs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BaseDocs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerUserId",
                table: "BaseDocs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatternId",
                table: "BaseDocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionLineId",
                table: "BaseDocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResponseUserId",
                table: "BaseDocs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkEndDate",
                table: "BaseDocs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkStartDate",
                table: "BaseDocs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "templateWorkOrderType",
                table: "BaseDocs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseDocs_ConsumeProductId",
                table: "BaseDocs",
                column: "ConsumeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseDocs_ProductionLineId",
                table: "BaseDocs",
                column: "ProductionLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseDocs_ProductionLines_ProductionLineId",
                table: "BaseDocs",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseDocs_Products_ConsumeProductId",
                table: "BaseDocs",
                column: "ConsumeProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
