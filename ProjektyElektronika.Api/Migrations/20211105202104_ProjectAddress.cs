using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjektyElektronika.Api.Migrations
{
    public partial class ProjectAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Projects");
        }
    }
}
