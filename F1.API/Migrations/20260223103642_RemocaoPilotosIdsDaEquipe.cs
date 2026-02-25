using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1.API.Migrations
{
    /// <inheritdoc />
    public partial class RemocaoPilotosIdsDaEquipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PilotosIds",
                table: "Equipes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "PilotosIds",
                table: "Equipes",
                type: "integer[]",
                nullable: false);
        }
    }
}
