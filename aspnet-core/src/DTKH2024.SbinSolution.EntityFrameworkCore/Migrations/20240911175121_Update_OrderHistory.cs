using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DTKH2024.SbinSolution.Migrations
{
    /// <inheritdoc />
    public partial class Update_OrderHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGive",
                table: "OrderHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGive",
                table: "OrderHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
