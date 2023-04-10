using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class measurement_fix_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Multipler",
                table: "MeasurementUnits",
                type: "decimal(8,8)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Multipler",
                table: "MeasurementUnits");
        }
    }
}
