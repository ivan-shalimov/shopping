using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.DataAccess.Migrations
{
    public partial class ExtendPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Purchases");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Purchases",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Purchases",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Purchases");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
