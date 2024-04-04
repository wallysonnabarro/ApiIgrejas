using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class FichasAtualizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiaoId",
                table: "FichasLider",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiaoId",
                table: "FichasConectados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FichasLider_SiaoId",
                table: "FichasLider",
                column: "SiaoId");

            migrationBuilder.CreateIndex(
                name: "IX_FichasConectados_SiaoId",
                table: "FichasConectados",
                column: "SiaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FichasConectados_Siaos_SiaoId",
                table: "FichasConectados",
                column: "SiaoId",
                principalTable: "Siaos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FichasLider_Siaos_SiaoId",
                table: "FichasLider",
                column: "SiaoId",
                principalTable: "Siaos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichasConectados_Siaos_SiaoId",
                table: "FichasConectados");

            migrationBuilder.DropForeignKey(
                name: "FK_FichasLider_Siaos_SiaoId",
                table: "FichasLider");

            migrationBuilder.DropIndex(
                name: "IX_FichasLider_SiaoId",
                table: "FichasLider");

            migrationBuilder.DropIndex(
                name: "IX_FichasConectados_SiaoId",
                table: "FichasConectados");

            migrationBuilder.DropColumn(
                name: "SiaoId",
                table: "FichasLider");

            migrationBuilder.DropColumn(
                name: "SiaoId",
                table: "FichasConectados");
        }
    }
}
