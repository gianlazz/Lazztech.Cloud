using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class AddedInitialMarketingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    CampaignId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.CampaignId);
                });

            migrationBuilder.CreateTable(
                name: "FocusGroups",
                columns: table => new
                {
                    FocusGroupId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    DateStarted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocusGroups", x => x.FocusGroupId);
                });

            migrationBuilder.CreateTable(
                name: "InstagramNodes",
                columns: table => new
                {
                    InstagramNodeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserName = table.Column<string>(nullable: true),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    FocusGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramNodes", x => x.InstagramNodeId);
                    table.ForeignKey(
                        name: "FK_InstagramNodes_FocusGroups_FocusGroupId",
                        column: x => x.FocusGroupId,
                        principalTable: "FocusGroups",
                        principalColumn: "FocusGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstagramNodes_FocusGroupId",
                table: "InstagramNodes",
                column: "FocusGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "InstagramNodes");

            migrationBuilder.DropTable(
                name: "FocusGroups");
        }
    }
}
