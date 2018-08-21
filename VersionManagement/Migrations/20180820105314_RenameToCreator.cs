using Microsoft.EntityFrameworkCore.Migrations;

namespace VersionManagement.Migrations
{
    public partial class RenameToCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Applicant",
            //    table: "Versions");

            migrationBuilder.AlterColumn<string>(
                name: "VersionTitle",
                table: "Versions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "Versions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Versions",
                nullable: false);
                //defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Versions");

            migrationBuilder.AlterColumn<string>(
                name: "VersionTitle",
                table: "Versions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "Versions",
                nullable: true,
                oldClrType: typeof(string));

            //migrationBuilder.AddColumn<string>(
            //    name: "Applicant",
            //    table: "Versions",
            //    nullable: true);
        }
    }
}
