using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class AddedMentorInvitesTableWithOneToOneRelationshipToMentor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MentorInvites",
                columns: table => new
                {
                    MentorInviteId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DateTimeWhenCreated = table.Column<DateTime>(nullable: false),
                    MentorId = table.Column<int>(nullable: true),
                    DateTimeWhenViewed = table.Column<DateTime>(nullable: true),
                    Accepted = table.Column<bool>(nullable: false),
                    InviteLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorInvites", x => x.MentorInviteId);
                    table.ForeignKey(
                        name: "FK_MentorInvites_Mentors_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MentorInvites_MentorId",
                table: "MentorInvites",
                column: "MentorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorInvites");
        }
    }
}
