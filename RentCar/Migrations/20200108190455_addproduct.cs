using Microsoft.EntityFrameworkCore.Migrations;

namespace RentCar.Migrations
{
    public partial class addproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Products",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PropId",
                table: "Products",
                column: "PropId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_PropId",
                table: "Products",
                column: "PropId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_PropId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PropId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PropId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Products");
        }
    }
}
