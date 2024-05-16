using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ajouterAliasDansProgramme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EstAliasDeId",
                table: "Programmes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programmes_EstAliasDeId",
                table: "Programmes",
                column: "EstAliasDeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programmes_Programmes_EstAliasDeId",
                table: "Programmes",
                column: "EstAliasDeId",
                principalTable: "Programmes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programmes_Programmes_EstAliasDeId",
                table: "Programmes");

            migrationBuilder.DropIndex(
                name: "IX_Programmes_EstAliasDeId",
                table: "Programmes");

            migrationBuilder.DropColumn(
                name: "EstAliasDeId",
                table: "Programmes");
        }
    }
}
