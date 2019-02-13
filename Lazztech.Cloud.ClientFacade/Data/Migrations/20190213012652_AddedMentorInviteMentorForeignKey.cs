using Microsoft.EntityFrameworkCore.Migrations;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class AddedMentorInviteMentorForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites");

            migrationBuilder.AlterColumn<int>(
                name: "MentorId",
                table: "MentorInvites",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites");

            migrationBuilder.AlterColumn<int>(
                name: "MentorId",
                table: "MentorInvites",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
