using Microsoft.EntityFrameworkCore.Migrations;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class ReplacedEventMentorManyToManyWithEventToManyMentor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventMentors");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Mentors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_EventId",
                table: "Mentors",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Events_EventId",
                table: "Mentors",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Events_EventId",
                table: "Mentors");

            migrationBuilder.DropIndex(
                name: "IX_Mentors_EventId",
                table: "Mentors");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Mentors");

            migrationBuilder.CreateTable(
                name: "EventMentors",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    MentorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMentors", x => new { x.EventId, x.MentorId });
                    table.ForeignKey(
                        name: "FK_EventMentors_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventMentors_Mentors_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventMentors_MentorId",
                table: "EventMentors",
                column: "MentorId");
        }
    }
}
