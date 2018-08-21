using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VersionManagement.Migrations
{
    public partial class AddDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VersionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskTitle = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Iteration = table.Column<string>(nullable: false),
                    CommitIds = table.Column<string>(nullable: false),
                    DetailNote = table.Column<string>(nullable: true),
                    Applicant = table.Column<string>(nullable: false),
                    VersionInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersionDetail_Versions_VersionInfoId",
                        column: x => x.VersionInfoId,
                        principalTable: "Versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VersionDetail_VersionInfoId",
                table: "VersionDetail",
                column: "VersionInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VersionDetail");
        }
    }
}
