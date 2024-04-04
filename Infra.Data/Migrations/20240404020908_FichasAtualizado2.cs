using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class FichasAtualizado2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "FichasLider",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FichasLider_AreaId",
                table: "FichasLider",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_FichasLider_AreasSet_AreaId",
                table: "FichasLider",
                column: "AreaId",
                principalTable: "AreasSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichasLider_AreasSet_AreaId",
                table: "FichasLider");

            migrationBuilder.DropIndex(
                name: "IX_FichasLider_AreaId",
                table: "FichasLider");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "FichasLider");
        }
    }
}
