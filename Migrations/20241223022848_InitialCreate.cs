using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentCompetitionAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Username = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionAwards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    StudentId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CompetitionName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    AwardLevel = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    AwardDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionAwards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionAwards_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionAwards_StudentId",
                table: "CompetitionAwards",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionAwards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
