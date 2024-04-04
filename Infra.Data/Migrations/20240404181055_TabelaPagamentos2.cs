using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class TabelaPagamentos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dinheiro = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Debito = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credito = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreditoParcelado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Parcelas = table.Column<int>(type: "int", nullable: false),
                    Pix = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Desistente = table.Column<int>(type: "int", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Receber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Descontar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FichaConsumidorId = table.Column<int>(type: "int", nullable: true),
                    VoluntarioId = table.Column<int>(type: "int", nullable: true),
                    SiaoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_FichasConectados_FichaConsumidorId",
                        column: x => x.FichaConsumidorId,
                        principalTable: "FichasConectados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pagamentos_FichasLider_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "FichasLider",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pagamentos_Siaos_SiaoId",
                        column: x => x.SiaoId,
                        principalTable: "Siaos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_FichaConsumidorId",
                table: "Pagamentos",
                column: "FichaConsumidorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_SiaoId",
                table: "Pagamentos",
                column: "SiaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_UsuarioId",
                table: "Pagamentos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_VoluntarioId",
                table: "Pagamentos",
                column: "VoluntarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
