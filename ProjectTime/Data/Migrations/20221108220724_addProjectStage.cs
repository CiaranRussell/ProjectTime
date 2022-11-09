using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTime.Data.Migrations
{
    public partial class addProjectStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectStageId",
                table: "projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "projectStage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectStage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_projects_ProjectStageId",
                table: "projects",
                column: "ProjectStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_projectStage_ProjectStageId",
                table: "projects",
                column: "ProjectStageId",
                principalTable: "projectStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_projectStage_ProjectStageId",
                table: "projects");

            migrationBuilder.DropTable(
                name: "projectStage");

            migrationBuilder.DropIndex(
                name: "IX_projects_ProjectStageId",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "ProjectStageId",
                table: "projects");
        }
    }
}
