using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _1_transactions_additions_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedWorkStartDate",
                table: "TemplateWorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "TemplateWorkOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DocDate",
                table: "BaseDocs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateWorkOrders_TemplateId",
                table: "TemplateWorkOrders",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateWorkOrders_Templates_TemplateId",
                table: "TemplateWorkOrders",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateWorkOrders_Templates_TemplateId",
                table: "TemplateWorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_TemplateWorkOrders_TemplateId",
                table: "TemplateWorkOrders");

            migrationBuilder.DropColumn(
                name: "PlannedWorkStartDate",
                table: "TemplateWorkOrders");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "TemplateWorkOrders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DocDate",
                table: "BaseDocs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
