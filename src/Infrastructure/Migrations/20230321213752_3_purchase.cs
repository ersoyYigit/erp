using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _3_purchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TemplateWorkOrders");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WarehouseReceiptRows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BaseDocs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WarehouseReceiptRows");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BaseDocs");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TemplateWorkOrders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
