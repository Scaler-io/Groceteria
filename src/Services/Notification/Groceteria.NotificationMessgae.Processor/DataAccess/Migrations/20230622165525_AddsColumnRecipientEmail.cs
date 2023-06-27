using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Groceteria.NotificationMessgae.Processor.DataAccess.Migrations
{
    public partial class AddsColumnRecipientEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationType",
                table: "NotificationHistory",
                newName: "RecipientEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecipientEmail",
                table: "NotificationHistory",
                newName: "NotificationType");
        }
    }
}
