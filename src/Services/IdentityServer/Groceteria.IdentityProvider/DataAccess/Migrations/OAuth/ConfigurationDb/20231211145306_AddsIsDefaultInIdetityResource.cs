using Microsoft.EntityFrameworkCore.Migrations;

namespace Groceteria.IdentityProvider.DataAccess.Migrations.OAuth.ConfigurationDb
{
    public partial class AddsIsDefaultInIdetityResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IdentityResources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "IdentityResources",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IdentityResources");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "IdentityResources");
        }
    }
}
