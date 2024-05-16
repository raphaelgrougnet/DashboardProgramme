using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class correctionDesEntites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursSecondaireReussis_Etudiants_EtudiantId",
                table: "CoursSecondaireReussis");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "CoursSecondaireReussis");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CoursSecondaireReussis");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "CoursAssistes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CoursAssistes");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Cours");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Cours");

            migrationBuilder.AddColumn<int>(
                name: "Annee",
                table: "SessionEtudes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "SessionEtudes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SessionEtudes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "SessionEtudes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SessionEtudes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "SessionEtudes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "SessionEtudes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Saison",
                table: "SessionEtudes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EstBeneficiaireServicesAdaptes",
                table: "SessionAssistees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NbTotalHeures",
                table: "SessionAssistees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "NiemeSession",
                table: "SessionAssistees",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<Guid>(
                name: "EtudiantId",
                table: "CoursSecondaireReussis",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursSecondaireReussis_Etudiants_EtudiantId",
                table: "CoursSecondaireReussis",
                column: "EtudiantId",
                principalTable: "Etudiants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursSecondaireReussis_Etudiants_EtudiantId",
                table: "CoursSecondaireReussis");

            migrationBuilder.DropColumn(
                name: "Annee",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "Saison",
                table: "SessionEtudes");

            migrationBuilder.DropColumn(
                name: "EstBeneficiaireServicesAdaptes",
                table: "SessionAssistees");

            migrationBuilder.DropColumn(
                name: "NbTotalHeures",
                table: "SessionAssistees");

            migrationBuilder.DropColumn(
                name: "NiemeSession",
                table: "SessionAssistees");

            migrationBuilder.AlterColumn<Guid>(
                name: "EtudiantId",
                table: "CoursSecondaireReussis",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "CoursSecondaireReussis",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CoursSecondaireReussis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "CoursAssistes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CoursAssistes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Cours",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Cours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursSecondaireReussis_Etudiants_EtudiantId",
                table: "CoursSecondaireReussis",
                column: "EtudiantId",
                principalTable: "Etudiants",
                principalColumn: "Id");
        }
    }
}
