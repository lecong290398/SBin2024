using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DTKH2024.SbinSolution.Migrations
{
    /// <inheritdoc />
    public partial class Update_ProductPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionName",
                table: "ProductPromotions");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductPromotions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromotionCode",
                table: "ProductPromotions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductPromotions");

            migrationBuilder.DropColumn(
                name: "PromotionCode",
                table: "ProductPromotions");

            migrationBuilder.AddColumn<string>(
                name: "PromotionName",
                table: "ProductPromotions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
