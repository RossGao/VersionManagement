using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VersionManagement.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    VersionNumber = table.Column<string>(nullable: false),
                    VersionTitle = table.Column<string>(nullable: false),
                    Department = table.Column<int>(nullable: false),
                    Creator = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ReleaseNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskTitle = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Iteration = table.Column<string>(nullable: false),
                    CommitIds = table.Column<string>(nullable: false),
                    DetailNote = table.Column<string>(nullable: true),
                    Applicant = table.Column<string>(nullable: false),
                    VersionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_Versions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "Versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Details_VersionId",
                table: "Details",
                column: "VersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Versions");
        }
    }
}
