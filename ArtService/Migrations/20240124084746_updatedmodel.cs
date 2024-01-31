using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtService.Migrations
{
    /// <inheritdoc />
    public partial class updatedmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Arts",
                newName: "SellerId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Arts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Arts",
                newName: "UserId");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Arts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
