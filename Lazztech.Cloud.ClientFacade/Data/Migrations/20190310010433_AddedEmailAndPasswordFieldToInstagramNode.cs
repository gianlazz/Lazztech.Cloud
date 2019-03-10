using Microsoft.EntityFrameworkCore.Migrations;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class AddedEmailAndPasswordFieldToInstagramNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "InstagramNodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "InstagramNodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "InstagramNodes");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "InstagramNodes");
        }
    }
}
