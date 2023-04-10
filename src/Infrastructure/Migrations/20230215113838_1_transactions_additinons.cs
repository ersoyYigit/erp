using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _1_transactions_additinons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrder_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrder");

            migrationBuilder.RenameColumn(
                name: "templateWorkOrderType",
                table: "TemplateWorkOrder",
                newName: "TemplateWorkOrderType");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WorkStartDate",
                table: "TemplateWorkOrder",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WorkEndDate",
                table: "TemplateWorkOrder",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ProductionLineId",
                table: "TemplateWorkOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedWorkEndDate",
                table: "TemplateWorkOrder",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocState",
                table: "BaseDocs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrder_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrder",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrder_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrder");

            migrationBuilder.DropColumn(
                name: "PlannedWorkEndDate",
                table: "TemplateWorkOrder");

            migrationBuilder.DropColumn(
                name: "DocState",
                table: "BaseDocs");

            migrationBuilder.RenameColumn(
                name: "TemplateWorkOrderType",
                table: "TemplateWorkOrder",
                newName: "templateWorkOrderType");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WorkStartDate",
                table: "TemplateWorkOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WorkEndDate",
                table: "TemplateWorkOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductionLineId",
                table: "TemplateWorkOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrder_ProductionLines_ProductionLineId",
                table: "TemplateWorkOrder",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
