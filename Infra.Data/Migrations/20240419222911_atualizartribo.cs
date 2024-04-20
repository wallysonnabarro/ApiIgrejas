using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class atualizartribo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "TribosEquipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TribosEquipes_ContratoId",
                table: "TribosEquipes",
                column: "ContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TribosEquipes_Contratos_ContratoId",
                table: "TribosEquipes",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TribosEquipes_Contratos_ContratoId",
                table: "TribosEquipes");

            migrationBuilder.DropIndex(
                name: "IX_TribosEquipes_ContratoId",
                table: "TribosEquipes");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "TribosEquipes");
        }
    }
}
