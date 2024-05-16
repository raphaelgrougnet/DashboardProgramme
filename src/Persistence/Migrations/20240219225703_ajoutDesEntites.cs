using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ajoutDesEntites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etudiants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HashCodePermanent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstBeneficiaireRenforcementFr = table.Column<bool>(type: "bit", nullable: false),
                    TourAdmission = table.Column<byte>(type: "tinyint", nullable: false),
                    StatutImmigration = table.Column<int>(type: "int", nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false),
                    Sanction = table.Column<int>(type: "int", nullable: false),
                    MoyenneGeneraleAuSecondaire = table.Column<float>(type: "real", nullable: false),
                    EstAssujetiAuR18 = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etudiants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Programmes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programmes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionEtudes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionEtudes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoursSecondaireReussis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeCours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EtudiantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursSecondaireReussis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursSecondaireReussis_Etudiants_EtudiantId",
                        column: x => x.EtudiantId,
                        principalTable: "Etudiants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GrilleProgrammes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgrammeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EtaleeSurNbSessions = table.Column<byte>(type: "tinyint", nullable: false),
                    AnneeMiseAJour = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrilleProgrammes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrilleProgrammes_Programmes_ProgrammeId",
                        column: x => x.ProgrammeId,
                        principalTable: "Programmes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionAssistees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EtudiantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrilleProgrammeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionEtudeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionAssistees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionAssistees_Etudiants_EtudiantId",
                        column: x => x.EtudiantId,
                        principalTable: "Etudiants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionAssistees_GrilleProgrammes_GrilleProgrammeId",
                        column: x => x.GrilleProgrammeId,
                        principalTable: "GrilleProgrammes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionAssistees_SessionEtudes_SessionEtudeId",
                        column: x => x.SessionEtudeId,
                        principalTable: "SessionEtudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursAssistes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoursId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteRecue = table.Column<int>(type: "int", nullable: false),
                    NumeroGroupe = table.Column<byte>(type: "tinyint", nullable: false),
                    SessionAssisteeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursAssistes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursAssistes_Cours_CoursId",
                        column: x => x.CoursId,
                        principalTable: "Cours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoursAssistes_SessionAssistees_SessionAssisteeId",
                        column: x => x.SessionAssisteeId,
                        principalTable: "SessionAssistees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoursAssistes_CoursId",
                table: "CoursAssistes",
                column: "CoursId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursAssistes_SessionAssisteeId",
                table: "CoursAssistes",
                column: "SessionAssisteeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursSecondaireReussis_EtudiantId",
                table: "CoursSecondaireReussis",
                column: "EtudiantId");

            migrationBuilder.CreateIndex(
                name: "IX_GrilleProgrammes_ProgrammeId",
                table: "GrilleProgrammes",
                column: "ProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAssistees_EtudiantId",
                table: "SessionAssistees",
                column: "EtudiantId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAssistees_GrilleProgrammeId",
                table: "SessionAssistees",
                column: "GrilleProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAssistees_SessionEtudeId",
                table: "SessionAssistees",
                column: "SessionEtudeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursAssistes");

            migrationBuilder.DropTable(
                name: "CoursSecondaireReussis");

            migrationBuilder.DropTable(
                name: "Cours");

            migrationBuilder.DropTable(
                name: "SessionAssistees");

            migrationBuilder.DropTable(
                name: "Etudiants");

            migrationBuilder.DropTable(
                name: "GrilleProgrammes");

            migrationBuilder.DropTable(
                name: "SessionEtudes");

            migrationBuilder.DropTable(
                name: "Programmes");
        }
    }
}
