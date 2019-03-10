using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lazztech.Cloud.ClientFacade.Migrations
{
    public partial class AddedSelectedContentHashTagsAndPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstagramHashtags",
                columns: table => new
                {
                    InstagramHashtagId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    FocusGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramHashtags", x => x.InstagramHashtagId);
                    table.ForeignKey(
                        name: "FK_InstagramHashtags_FocusGroups_FocusGroupId",
                        column: x => x.FocusGroupId,
                        principalTable: "FocusGroups",
                        principalColumn: "FocusGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstagramPosts",
                columns: table => new
                {
                    InstagramPostId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Posted = table.Column<bool>(nullable: false),
                    DateTimeForPost = table.Column<DateTime>(nullable: true),
                    LinkToContent = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    PostUrl = table.Column<string>(nullable: true),
                    InstagramNodeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramPosts", x => x.InstagramPostId);
                    table.ForeignKey(
                        name: "FK_InstagramPosts_InstagramNodes_InstagramNodeId",
                        column: x => x.InstagramNodeId,
                        principalTable: "InstagramNodes",
                        principalColumn: "InstagramNodeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SelectedInstaContents",
                columns: table => new
                {
                    SelectedInstaContentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DateTimeWhenSelected = table.Column<DateTime>(nullable: false),
                    LinkToContent = table.Column<string>(nullable: true),
                    FocusGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedInstaContents", x => x.SelectedInstaContentId);
                    table.ForeignKey(
                        name: "FK_SelectedInstaContents_FocusGroups_FocusGroupId",
                        column: x => x.FocusGroupId,
                        principalTable: "FocusGroups",
                        principalColumn: "FocusGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstagramHashtags_FocusGroupId",
                table: "InstagramHashtags",
                column: "FocusGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramPosts_InstagramNodeId",
                table: "InstagramPosts",
                column: "InstagramNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedInstaContents_FocusGroupId",
                table: "SelectedInstaContents",
                column: "FocusGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstagramHashtags");

            migrationBuilder.DropTable(
                name: "InstagramPosts");

            migrationBuilder.DropTable(
                name: "SelectedInstaContents");
        }
    }
}
