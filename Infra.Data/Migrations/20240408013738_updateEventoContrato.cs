using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateEventoContrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_ContratoId",
                table: "Eventos",
                column: "ContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Contratos_ContratoId",
                table: "Eventos",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Contratos_ContratoId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_ContratoId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "Eventos");
        }
    }
}
