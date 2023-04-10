using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class corecurrency3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellRateCash",
                table: "ExchangeRates",
                newName: "ForexSelling");

            migrationBuilder.RenameColumn(
                name: "SellRate",
                table: "ExchangeRates",
                newName: "ForexBuying");

            migrationBuilder.RenameColumn(
                name: "BuyRateCash",
                table: "ExchangeRates",
                newName: "BanknoteSelling");

            migrationBuilder.RenameColumn(
                name: "BuyRate",
                table: "ExchangeRates",
                newName: "BanknoteBuying");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForexSelling",
                table: "ExchangeRates",
                newName: "SellRateCash");

            migrationBuilder.RenameColumn(
                name: "ForexBuying",
                table: "ExchangeRates",
                newName: "SellRate");

            migrationBuilder.RenameColumn(
                name: "BanknoteSelling",
                table: "ExchangeRates",
                newName: "BuyRateCash");

            migrationBuilder.RenameColumn(
                name: "BanknoteBuying",
                table: "ExchangeRates",
                newName: "BuyRate");
        }
    }
}
