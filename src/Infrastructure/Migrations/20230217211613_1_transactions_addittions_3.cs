using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _1_transactions_addittions_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrder_BaseDocs_Id",
                table: "TemplateWorkOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrder_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrder_Products_ConsumeProductId",
                table: "TemplateWorkOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateWorkOrder",
                table: "TemplateWorkOrder");

            migrationBuilder.RenameTable(
                name: "TemplateWorkOrder",
                newName: "TemplateWorkOrders");

            migrationBuilder.RenameColumn(
                name: "DocumentNo",
                table: "BaseDocs",
                newName: "DocNo");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateWorkOrder_ProductionLineId",
                table: "TemplateWorkOrders",
                newName: "IX_TemplateWorkOrders_ProductionLineId");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateWorkOrder_ConsumeProductId",
                table: "TemplateWorkOrders",
                newName: "IX_TemplateWorkOrders_ConsumeProductId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Molds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsageYear",
                table: "Molds",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateWorkOrders",
                table: "TemplateWorkOrders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Molds_CompanyId",
                table: "Molds",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Molds_Companies_CompanyId",
                table: "Molds",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrders_BaseDocs_Id",
                table: "TemplateWorkOrders",
                column: "Id",
                principalTable: "BaseDocs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrders_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrders",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrders_Products_ConsumeProductId",
                table: "TemplateWorkOrders",
                column: "ConsumeProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Molds_Companies_CompanyId",
                table: "Molds");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrders_BaseDocs_Id",
                table: "TemplateWorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrders_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrders_Products_ConsumeProductId",
                table: "TemplateWorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_Molds_CompanyId",
                table: "Molds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateWorkOrders",
                table: "TemplateWorkOrders");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Molds");

            migrationBuilder.DropColumn(
                name: "UsageYear",
                table: "Molds");

            migrationBuilder.RenameTable(
                name: "TemplateWorkOrders",
                newName: "TemplateWorkOrder");

            migrationBuilder.RenameColumn(
                name: "DocNo",
                table: "BaseDocs",
                newName: "DocumentNo");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateWorkOrders_ProductionLineId",
                table: "TemplateWorkOrder",
                newName: "IX_TemplateWorkOrder_ProductionLineId");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateWorkOrders_ConsumeProductId",
                table: "TemplateWorkOrder",
                newName: "IX_TemplateWorkOrder_ConsumeProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateWorkOrder",
                table: "TemplateWorkOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrder_BaseDocs_Id",
                table: "TemplateWorkOrder",
                column: "Id",
                principalTable: "BaseDocs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrder_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrder",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrder_Products_ConsumeProductId",
                table: "TemplateWorkOrder",
                column: "ConsumeProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
