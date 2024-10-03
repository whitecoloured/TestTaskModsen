using Microsoft.EntityFrameworkCore.Migrations;

namespace EventsWebAPI.Migrations
{
    public partial class AddEventPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventPlace",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventPlace",
                table: "Events");
        }
    }
}
