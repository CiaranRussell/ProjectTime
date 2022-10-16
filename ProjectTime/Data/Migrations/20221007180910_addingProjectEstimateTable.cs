using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTime.Data.Migrations
{
    public partial class addingProjectEstimateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projectEstimates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectUserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectEstimates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectEstimates_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projectEstimates_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projectEstimates_projectUsers_ProjectUserId",
                        column: x => x.ProjectUserId,
                        principalTable: "projectUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_projectEstimates_DepartmentId",
                table: "projectEstimates",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_projectEstimates_ProjectId",
                table: "projectEstimates",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_projectEstimates_ProjectUserId",
                table: "projectEstimates",
                column: "ProjectUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projectEstimates");
        }
    }
}
