using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTariffPeriodAddVariableRateForBillItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "BillItems",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                defaultValue: 0m);

            // Migrate existing data from Tariffs to BillItems
            migrationBuilder.Sql(@"UPDATE BI "
                              + "SET BI.Rate = T.Rate "
                              + "FROM BillItems BI "
                              + "INNER JOIN Tariffs T ON BI.TariffId = T.Id;");

            migrationBuilder.CreateTable(
                name: "TariffPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: false),
                    StartOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffPeriods_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TariffPeriods_TariffId",
                table: "TariffPeriods",
                column: "TariffId");

            // Migrate existing data from Tariffs to TariffPeriods
            migrationBuilder.Sql(@"INSERT INTO TariffPeriods (Id, TariffId, Rate, StartOn, EndOn) "
                              + "SELECT NEWID(), Id, Rate, StartOn, EndOn "
                              + "FROM Tariffs;");

            migrationBuilder.DropColumn(
                name: "EndOn",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "StartOn",
                table: "Tariffs");

            migrationBuilder.AddColumn<int>(
                name: "RateType",
                table: "Tariffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Set RateType to Constant for existing Tariffs
            migrationBuilder.Sql(@"UPDATE Tariffs SET RateType = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffPeriods");

            migrationBuilder.DropColumn(
                name: "RateType",
                table: "Tariffs");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "BillItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndOn",
                table: "Tariffs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Tariffs",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartOn",
                table: "Tariffs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}