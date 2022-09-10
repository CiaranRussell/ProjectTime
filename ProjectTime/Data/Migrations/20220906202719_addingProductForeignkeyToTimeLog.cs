using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTime.Data.Migrations
{
    public partial class addingProductForeignkeyToTimeLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "timeLog",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "timeLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_timeLog_ProjectId",
                table: "timeLog",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_timeLog_projects_ProjectId",
                table: "timeLog",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timeLog_projects_ProjectId",
                table: "timeLog");

            migrationBuilder.DropIndex(
                name: "IX_timeLog_ProjectId",
                table: "timeLog");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "timeLog");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "timeLog",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
