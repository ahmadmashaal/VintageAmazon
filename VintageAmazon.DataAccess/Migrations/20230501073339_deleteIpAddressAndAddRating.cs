using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VintageAmazon.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class deleteIpAddressAndAddRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarRating_Products_ProductId",
                table: "StarRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StarRating",
                table: "StarRating");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "StarRating");

            migrationBuilder.RenameTable(
                name: "StarRating",
                newName: "StarsRatings");

            migrationBuilder.RenameIndex(
                name: "IX_StarRating_ProductId",
                table: "StarsRatings",
                newName: "IX_StarsRatings_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Products",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StarsRatings",
                table: "StarsRatings",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StarsRatings_Products_ProductId",
                table: "StarsRatings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StarsRatings_Products_ProductId",
                table: "StarsRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StarsRatings",
                table: "StarsRatings");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "StarsRatings",
                newName: "StarRating");

            migrationBuilder.RenameIndex(
                name: "IX_StarsRatings_ProductId",
                table: "StarRating",
                newName: "IX_StarRating_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "StarRating",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StarRating",
                table: "StarRating",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StarRating_Products_ProductId",
                table: "StarRating",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
