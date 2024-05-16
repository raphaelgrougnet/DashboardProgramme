using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ajouterIndexUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SessionAssistees_EtudiantId",
                table: "SessionAssistees");

            migrationBuilder.DropIndex(
                name: "IX_MemberProgrammes_MemberId",
                table: "MemberProgrammes");

            migrationBuilder.DropIndex(
                name: "IX_GrilleProgrammes_ProgrammeId",
                table: "GrilleProgrammes");

            migrationBuilder.DropIndex(
                name: "IX_CoursAssistes_SessionAssisteeId",
                table: "CoursAssistes");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "SessionEtudes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Programmes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HashCodePermanent",
                table: "Etudiants",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CodeCours",
                table: "CoursSecondaireReussis",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Cours",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEtudes_Annee_Saison",
                table: "SessionEtudes",
                columns: new[] { "Annee", "Saison" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionEtudes_Ordre",
                table: "SessionEtudes",
                column: "Ordre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionEtudes_Slug",
                table: "SessionEtudes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionAssistees_EtudiantId_SessionEtudeId",
                table: "SessionAssistees",
                columns: new[] { "EtudiantId", "SessionEtudeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programmes_Numero",
                table: "Programmes",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberProgrammes_MemberId_ProgrammeId",
                table: "MemberProgrammes",
                columns: new[] { "MemberId", "ProgrammeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrilleProgrammes_ProgrammeId_EtaleeSurNbSessions_AnneeMiseAJour",
                table: "GrilleProgrammes",
                columns: new[] { "ProgrammeId", "EtaleeSurNbSessions", "AnneeMiseAJour" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Etudiants_HashCodePermanent",
                table: "Etudiants",
                column: "HashCodePermanent",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoursSecondaireReussis_CodeCours",
                table: "CoursSecondaireReussis",
                column: "CodeCours");

            migrationBuilder.CreateIndex(
                name: "IX_CoursSecondaireReussis_CodeCours_EtudiantId",
                table: "CoursSecondaireReussis",
                columns: new[] { "CodeCours", "EtudiantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoursAssistes_SessionAssisteeId_CoursId",
                table: "CoursAssistes",
                columns: new[] { "SessionAssisteeId", "CoursId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cours_Code",
                table: "Cours",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SessionEtudes_Annee_Saison",
                table: "SessionEtudes");

            migrationBuilder.DropIndex(
                name: "IX_SessionEtudes_Ordre",
                table: "SessionEtudes");

            migrationBuilder.DropIndex(
                name: "IX_SessionEtudes_Slug",
                table: "SessionEtudes");

            migrationBuilder.DropIndex(
                name: "IX_SessionAssistees_EtudiantId_SessionEtudeId",
                table: "SessionAssistees");

            migrationBuilder.DropIndex(
                name: "IX_Programmes_Numero",
                table: "Programmes");

            migrationBuilder.DropIndex(
                name: "IX_MemberProgrammes_MemberId_ProgrammeId",
                table: "MemberProgrammes");

            migrationBuilder.DropIndex(
                name: "IX_GrilleProgrammes_ProgrammeId_EtaleeSurNbSessions_AnneeMiseAJour",
                table: "GrilleProgrammes");

            migrationBuilder.DropIndex(
                name: "IX_Etudiants_HashCodePermanent",
                table: "Etudiants");

            migrationBuilder.DropIndex(
                name: "IX_CoursSecondaireReussis_CodeCours",
                table: "CoursSecondaireReussis");

            migrationBuilder.DropIndex(
                name: "IX_CoursSecondaireReussis_CodeCours_EtudiantId",
                table: "CoursSecondaireReussis");

            migrationBuilder.DropIndex(
                name: "IX_CoursAssistes_SessionAssisteeId_CoursId",
                table: "CoursAssistes");

            migrationBuilder.DropIndex(
                name: "IX_Cours_Code",
                table: "Cours");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "SessionEtudes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Programmes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "HashCodePermanent",
                table: "Etudiants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CodeCours",
                table: "CoursSecondaireReussis",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Cours",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAssistees_EtudiantId",
                table: "SessionAssistees",
                column: "EtudiantId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProgrammes_MemberId",
                table: "MemberProgrammes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GrilleProgrammes_ProgrammeId",
                table: "GrilleProgrammes",
                column: "ProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursAssistes_SessionAssisteeId",
                table: "CoursAssistes",
                column: "SessionAssisteeId");
        }
    }
}
