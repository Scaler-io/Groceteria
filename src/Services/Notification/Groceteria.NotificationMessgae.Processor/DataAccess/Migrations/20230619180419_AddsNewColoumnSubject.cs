using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Groceteria.NotificationMessgae.Processor.DataAccess.Migrations
{
    public partial class AddsNewColoumnSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "NotificationHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "NotificationHistory");
        }
    }
}
