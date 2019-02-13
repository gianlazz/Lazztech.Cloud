using Microsoft.EntityFrameworkCore.Migrations;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class RemovedMentorInviteEventOneOneBecauseMentorHasEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "MentorInvites",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "MentorInvites",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
