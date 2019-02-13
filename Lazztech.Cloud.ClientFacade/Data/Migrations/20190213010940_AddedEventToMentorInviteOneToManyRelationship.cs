using Microsoft.EntityFrameworkCore.Migrations;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class AddedEventToMentorInviteOneToManyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "MentorInvites",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MentorInvites_EventId",
                table: "MentorInvites",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites");

            migrationBuilder.DropIndex(
                name: "IX_MentorInvites_EventId",
                table: "MentorInvites");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "MentorInvites");
        }
    }
}
