using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1.API.Migrations
{
    /// <inheritdoc />
    public partial class AjustesModelagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrandesPremios_Pilotos_VencedorId",
                table: "GrandesPremios");

            migrationBuilder.DropForeignKey(
                name: "FK_Pilotos_Equipes_EquipeId",
                table: "Pilotos");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pilotos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nacionalidade",
                table: "Pilotos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "GrandesPremios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Localizacao",
                table: "GrandesPremios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Equipes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<List<int>>(
                name: "PilotosIds",
                table: "Equipes",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_GrandesPremios_Pilotos_VencedorId",
                table: "GrandesPremios",
                column: "VencedorId",
                principalTable: "Pilotos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Pilotos_Equipes_EquipeId",
                table: "Pilotos",
                column: "EquipeId",
                principalTable: "Equipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrandesPremios_Pilotos_VencedorId",
                table: "GrandesPremios");

            migrationBuilder.DropForeignKey(
                name: "FK_Pilotos_Equipes_EquipeId",
                table: "Pilotos");

            migrationBuilder.DropColumn(
                name: "PilotosIds",
                table: "Equipes");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pilotos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nacionalidade",
                table: "Pilotos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "GrandesPremios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Localizacao",
                table: "GrandesPremios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Equipes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_GrandesPremios_Pilotos_VencedorId",
                table: "GrandesPremios",
                column: "VencedorId",
                principalTable: "Pilotos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pilotos_Equipes_EquipeId",
                table: "Pilotos",
                column: "EquipeId",
                principalTable: "Equipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
