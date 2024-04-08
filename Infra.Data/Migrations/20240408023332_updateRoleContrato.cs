using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateRoleContrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ContratoId",
                table: "Roles",
                column: "ContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Contratos_ContratoId",
                table: "Roles",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Contratos_ContratoId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ContratoId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "Roles");
        }
    }
}
