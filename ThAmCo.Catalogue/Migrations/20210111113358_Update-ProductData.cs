using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catalogue.Migrations
{
    public partial class UpdateProductData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "ProductData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EAN",
                table: "ProductData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ProductData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "ProductData");

            migrationBuilder.DropColumn(
                name: "EAN",
                table: "ProductData");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductData");
        }
    }
}
