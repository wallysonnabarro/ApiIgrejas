using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fichas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FichasConectados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriboId = table.Column<int>(type: "int", nullable: false),
                    Lider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<int>(type: "int", nullable: false),
                    EstadoCivil = table.Column<int>(type: "int", nullable: false),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContatoEmergencial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Crianca = table.Column<bool>(type: "bit", nullable: false),
                    Cuidados = table.Column<bool>(type: "bit", nullable: false),
                    Idade = table.Column<int>(type: "int", nullable: false),
                    DescricaoCuidados = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichasConectados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichasConectados_TribosEquipes_TriboId",
                        column: x => x.TriboId,
                        principalTable: "TribosEquipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FichasLider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriboId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichasLider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichasLider_TribosEquipes_TriboId",
                        column: x => x.TriboId,
                        principalTable: "TribosEquipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FichasConectados_TriboId",
                table: "FichasConectados",
                column: "TriboId");

            migrationBuilder.CreateIndex(
                name: "IX_FichasLider_TriboId",
                table: "FichasLider",
                column: "TriboId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FichasConectados");

            migrationBuilder.DropTable(
                name: "FichasLider");
        }
    }
}
