using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAreaContrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "AreasSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AreasSet_ContratoId",
                table: "AreasSet",
                column: "ContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AreasSet_Contratos_ContratoId",
                table: "AreasSet",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AreasSet_Contratos_ContratoId",
                table: "AreasSet");

            migrationBuilder.DropIndex(
                name: "IX_AreasSet_ContratoId",
                table: "AreasSet");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "AreasSet");
        }
    }
}
