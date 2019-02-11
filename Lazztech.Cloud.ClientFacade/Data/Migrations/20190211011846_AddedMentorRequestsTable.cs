using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class AddedMentorRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MentorRequests",
                columns: table => new
                {
                    MentorRequestId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DateTimeOfRequest = table.Column<DateTime>(nullable: false),
                    MentorId = table.Column<int>(nullable: false),
                    UniqueRequesteeId = table.Column<string>(nullable: true),
                    RequestAccepted = table.Column<bool>(nullable: false),
                    DateTimeWhenProcessed = table.Column<DateTime>(nullable: true),
                    OutboundSmsSmsId = table.Column<int>(nullable: true),
                    InboundSmsSmsId = table.Column<int>(nullable: true),
                    MentoringDuration = table.Column<TimeSpan>(nullable: false),
                    RequestTimeout = table.Column<TimeSpan>(nullable: false),
                    TimedOut = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorRequests", x => x.MentorRequestId);
                    table.ForeignKey(
                        name: "FK_MentorRequests_SmsMessages_InboundSmsSmsId",
                        column: x => x.InboundSmsSmsId,
                        principalTable: "SmsMessages",
                        principalColumn: "SmsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorRequests_Mentors_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorRequests_SmsMessages_OutboundSmsSmsId",
                        column: x => x.OutboundSmsSmsId,
                        principalTable: "SmsMessages",
                        principalColumn: "SmsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MentorRequests_InboundSmsSmsId",
                table: "MentorRequests",
                column: "InboundSmsSmsId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorRequests_MentorId",
                table: "MentorRequests",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorRequests_OutboundSmsSmsId",
                table: "MentorRequests",
                column: "OutboundSmsSmsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorRequests");
        }
    }
}
