using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.DataAccess.Migrations
{
    public partial class AddProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Purchases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProductKinds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductKindId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("272f0bd7-3896-41fe-8c96-b1772519d306"))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO[dbo].[ProductKinds] ([Id],[Name]) VALUES('272f0bd7-3896-41fe-8c96-b1772519d306', 'undefined');");
            migrationBuilder.Sql(@"INSERT INTO [dbo].[Products] (Id, [Name]) SELECT NEWID() as Id, [Name] FROM [Purchases] group by [Name];");
            migrationBuilder.Sql(@"UPDATE P SET P.[ProductId] = PR.Id FROM [Purchases] P INNER JOIN [Products] PR ON P.[Name] = PR.[Name];") ;

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Purchases");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductKindId",
                table: "Products",
                column: "ProductKindId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductKinds");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Purchases");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}