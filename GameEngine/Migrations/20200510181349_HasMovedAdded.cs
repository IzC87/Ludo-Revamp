using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class HasMovedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMoved",
                table: "Players",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasMoved",
                table: "Players");
        }
    }
}
