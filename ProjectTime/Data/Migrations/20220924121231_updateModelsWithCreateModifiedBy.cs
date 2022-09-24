using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTime.Data.Migrations
{
    public partial class updateModelsWithCreateModifiedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDateTime",
                table: "timeLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "projectUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByUserId",
                table: "projectUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDateTime",
                table: "projectUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByUserId",
                table: "projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDateTime",
                table: "projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "nonWorkingDays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByUserId",
                table: "nonWorkingDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDateTime",
                table: "nonWorkingDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifyDateTime",
                table: "timeLog");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "projectUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "projectUsers");

            migrationBuilder.DropColumn(
                name: "ModifyDateTime",
                table: "projectUsers");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "ModifyDateTime",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "nonWorkingDays");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "nonWorkingDays");

            migrationBuilder.DropColumn(
                name: "ModifyDateTime",
                table: "nonWorkingDays");
        }
    }
}
