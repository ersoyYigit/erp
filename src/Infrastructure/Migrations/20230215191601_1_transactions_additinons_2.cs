using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _1_transactions_additinons_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerUserName",
                table: "TemplateWorkOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseUserName",
                table: "TemplateWorkOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Molds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    ModelOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemplatePatternId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Molds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Molds_Patterns_TemplatePatternId",
                        column: x => x.TemplatePatternId,
                        principalTable: "Patterns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Molds_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Molds_TemplatePatternId",
                table: "Molds",
                column: "TemplatePatternId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Molds");

            migrationBuilder.DropColumn(
                name: "OwnerUserName",
                table: "TemplateWorkOrder");

            migrationBuilder.DropColumn(
                name: "ResponseUserName",
                table: "TemplateWorkOrder");
        }
    }
}
