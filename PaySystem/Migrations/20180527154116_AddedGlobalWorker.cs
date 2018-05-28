﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PaySystem.Migrations
{
    public partial class AddedGlobalWorker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Worker",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                DateOfBirth = table.Column<DateTime>(nullable: false),
                UpdateTimeDateTime = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(nullable: true),
                Biography = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Worker", x => x.Id);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Worker");
        }
    }
}
