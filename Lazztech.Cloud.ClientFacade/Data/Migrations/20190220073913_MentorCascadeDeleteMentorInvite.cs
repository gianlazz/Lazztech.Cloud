using Microsoft.EntityFrameworkCore.Migrations;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class MentorCascadeDeleteMentorInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorInvites_Mentors_MentorId",
                table: "MentorInvites");

            migrationBuilder.DropIndex(
                name: "IX_MentorInvites_MentorId",
                table: "MentorInvites");

            migrationBuilder.AddColumn<int>(
                name: "MentorInviteId",
                table: "Mentors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorInvites_MentorId",
                table: "MentorInvites",
                column: "MentorId",
                unique: true);

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

            migrationBuilder.DropIndex(
                name: "IX_MentorInvites_MentorId",
                table: "MentorInvites");

            migrationBuilder.DropColumn(
                name: "MentorInviteId",
                table: "Mentors");

            migrationBuilder.CreateIndex(
                name: "IX_MentorInvites_MentorId",
                table: "MentorInvites",
                column: "MentorId");

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
