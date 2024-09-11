using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DTKH2024.SbinSolution.Migrations
{
    /// <inheritdoc />
    public partial class ReUpdate_ProductPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryPromotionId",
                table: "ProductPromotions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPromotions_CategoryPromotionId",
                table: "ProductPromotions",
                column: "CategoryPromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPromotions_CategoryPromotions_CategoryPromotionId",
                table: "ProductPromotions",
                column: "CategoryPromotionId",
                principalTable: "CategoryPromotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPromotions_CategoryPromotions_CategoryPromotionId",
                table: "ProductPromotions");

            migrationBuilder.DropIndex(
                name: "IX_ProductPromotions_CategoryPromotionId",
                table: "ProductPromotions");

            migrationBuilder.DropColumn(
                name: "CategoryPromotionId",
                table: "ProductPromotions");
        }
    }
}
