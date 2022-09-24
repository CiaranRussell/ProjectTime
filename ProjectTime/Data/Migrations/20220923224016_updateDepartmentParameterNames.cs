using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTime.Data.Migrations
{
    public partial class updateDepartmentParameterNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "departments",
                newName: "ModifiedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "departments",
                newName: "CreatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedByUserId",
                table: "departments",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "departments",
                newName: "CreatedBy");
        }
    }
}
