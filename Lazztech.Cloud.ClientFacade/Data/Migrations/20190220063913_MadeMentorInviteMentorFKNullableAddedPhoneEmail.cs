using Microsoft.EntityFrameworkCore.Migrations;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class MadeMentorInviteMentorFKNullableAddedPhoneEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites");

            migrationBuilder.AlterColumn<int>(
                name: "MentorId",
                table: "MentorInvites",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "MentorInvites",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MentorInvites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "MentorInvites",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Events_EventId",
                table: "MentorInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "MentorInvites");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "MentorInvites");

            migrationBuilder.AlterColumn<int>(
                name: "MentorId",
                table: "MentorInvites",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
