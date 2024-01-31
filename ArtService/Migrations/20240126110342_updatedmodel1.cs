using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtService.Migrations
{
    /// <inheritdoc />
    public partial class updatedmodel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HighestBid",
                table: "Arts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighestBid",
                table: "Arts");
        }
    }
}
