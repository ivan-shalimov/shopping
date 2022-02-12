using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.DataAccess.Migrations
{
    public partial class AddReciepts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO [dbo].[Receipts]([Id],[Description],[Total],[CreatedOn]) "
                                 + @"SELECT NEWID(), ' - ' AS [Description], SUM([Amount] * [Price]) AS Total, "
                                 + @"DATEFROMPARTS (DATEPART(yy, [Created]), DATEPART(mm, [Created]), DATEPART(dd, [Created])) AS [CreatedOn] FROM [dbo].[Purchases] "
                                 + "GROUP BY DATEFROMPARTS (DATEPART(yy, [Created]), DATEPART(mm, [Created]), DATEPART(dd, [Created]));");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[ReceiptItems]([Id], [ReceiptId],[ProductId],[Price],[Amount]) "
                               + @"SELECT [Id],[Id] AS [ReceiptId],[ProductId],[Amount],[Price] FROM [dbo].[Purchases];");
            migrationBuilder.Sql(@"UPDATE RI SET RI.ReceiptId = R.Id "
                               + @"FROM [dbo].[ReceiptItems] RI "
                               + @"INNER JOIN [dbo].[Purchases] P ON RI.Id = P.Id "
                               + @"INNER JOIN [dbo].[Receipts] R ON DATEFROMPARTS (DATEPART(yy, [Created]), DATEPART(mm, [Created]), DATEPART(dd, [Created])) = R.CreatedOn;");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ProductId",
                table: "ReceiptItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ReceiptId",
                table: "ReceiptItems",
                column: "ReceiptId");

            migrationBuilder.DropTable(
                name: "Purchases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptItems");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases",
                column: "ProductId");
        }
    }
}