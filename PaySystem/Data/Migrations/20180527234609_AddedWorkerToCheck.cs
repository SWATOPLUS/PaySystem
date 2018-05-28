using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PaySystem.Data.Migrations
{
    public partial class AddedWorkerToCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Check",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Check_WorkerId",
                table: "Check",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Check_Worker_WorkerId",
                table: "Check",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Check_Worker_WorkerId",
                table: "Check");

            migrationBuilder.DropIndex(
                name: "IX_Check_WorkerId",
                table: "Check");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Check");
        }
    }
}
