using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Groceteria.NotificationMessgae.Processor.DataAccess.Migrations
{
    public partial class ChangesNameFromMessageTypeToNotificationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageType",
                table: "NotificationHistory",
                newName: "NotificationType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationType",
                table: "NotificationHistory",
                newName: "MessageType");
        }
    }
}
