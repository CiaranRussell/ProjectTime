using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTime.Data.Migrations
{
    public partial class addingTimeLogToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "timeLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectUserId = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_timeLog_projectUsers_ProjectUserId",
                        column: x => x.ProjectUserId,
                        principalTable: "projectUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_timeLog_ProjectUserId",
                table: "timeLog",
                column: "ProjectUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "timeLog");
        }
    }
}
