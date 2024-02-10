using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceChangeProjections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceChangeProjections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductKindName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shop = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PreviousPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ChangePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceChangeProjections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceChangeProjections_ProductId",
                table: "PriceChangeProjections",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceChangeProjections_ProductId_Shop",
                table: "PriceChangeProjections",
                columns: new[] { "ProductId", "Shop" },
                unique: true,
                filter: "[Shop] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PriceChangeProjections_Shop",
                table: "PriceChangeProjections",
                column: "Shop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceChangeProjections");
        }
    }
}
