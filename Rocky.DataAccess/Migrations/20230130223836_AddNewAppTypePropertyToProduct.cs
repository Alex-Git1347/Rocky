using Microsoft.EntityFrameworkCore.Migrations;

namespace Rocky.Migrations
{
    public partial class AddNewAppTypePropertyToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppTypeId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_AppTypeId",
                table: "Product",
                column: "AppTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationType_AppTypeId",
                table: "Product",
                column: "AppTypeId",
                principalTable: "ApplicationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationType_AppTypeId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_AppTypeId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "AppTypeId",
                table: "Product");
        }
    }
}
