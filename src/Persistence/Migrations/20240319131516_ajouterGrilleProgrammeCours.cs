using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ajouterGrilleProgrammeCours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrilleProgrammeCours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrilleProgrammeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoursId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroSessionPrevu = table.Column<byte>(type: "tinyint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrilleProgrammeCours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrilleProgrammeCours_Cours_CoursId",
                        column: x => x.CoursId,
                        principalTable: "Cours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrilleProgrammeCours_GrilleProgrammes_GrilleProgrammeId",
                        column: x => x.GrilleProgrammeId,
                        principalTable: "GrilleProgrammes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrilleProgrammeCours_CoursId",
                table: "GrilleProgrammeCours",
                column: "CoursId");

            migrationBuilder.CreateIndex(
                name: "IX_GrilleProgrammeCours_GrilleProgrammeId_CoursId",
                table: "GrilleProgrammeCours",
                columns: new[] { "GrilleProgrammeId", "CoursId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrilleProgrammeCours");
        }
    }
}
