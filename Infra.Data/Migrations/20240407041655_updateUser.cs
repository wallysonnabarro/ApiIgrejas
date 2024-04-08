using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichasConectados_Siaos_SiaoId",
                table: "FichasConectados");

            migrationBuilder.DropForeignKey(
                name: "FK_FichasLider_Siaos_SiaoId",
                table: "FichasLider");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Siaos_SiaoId",
                table: "Pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Siaos");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "SiaoId",
                table: "Pagamentos",
                newName: "EventoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamentos_SiaoId",
                table: "Pagamentos",
                newName: "IX_Pagamentos_EventoId");

            migrationBuilder.RenameColumn(
                name: "SiaoId",
                table: "FichasLider",
                newName: "EventoId");

            migrationBuilder.RenameIndex(
                name: "IX_FichasLider_SiaoId",
                table: "FichasLider",
                newName: "IX_FichasLider_EventoId");

            migrationBuilder.RenameColumn(
                name: "SiaoId",
                table: "FichasConectados",
                newName: "EventoId");

            migrationBuilder.RenameIndex(
                name: "IX_FichasConectados_SiaoId",
                table: "FichasConectados",
                newName: "IX_FichasConectados_EventoId");

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordenadores = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Termino = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FichasConectados_Eventos_EventoId",
                table: "FichasConectados",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FichasLider_Eventos_EventoId",
                table: "FichasLider",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Eventos_EventoId",
                table: "Pagamentos",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichasConectados_Eventos_EventoId",
                table: "FichasConectados");

            migrationBuilder.DropForeignKey(
                name: "FK_FichasLider_Eventos_EventoId",
                table: "FichasLider");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Eventos_EventoId",
                table: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "EventoId",
                table: "Pagamentos",
                newName: "SiaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamentos_EventoId",
                table: "Pagamentos",
                newName: "IX_Pagamentos_SiaoId");

            migrationBuilder.RenameColumn(
                name: "EventoId",
                table: "FichasLider",
                newName: "SiaoId");

            migrationBuilder.RenameIndex(
                name: "IX_FichasLider_EventoId",
                table: "FichasLider",
                newName: "IX_FichasLider_SiaoId");

            migrationBuilder.RenameColumn(
                name: "EventoId",
                table: "FichasConectados",
                newName: "SiaoId");

            migrationBuilder.RenameIndex(
                name: "IX_FichasConectados_EventoId",
                table: "FichasConectados",
                newName: "IX_FichasConectados_SiaoId");

            migrationBuilder.CreateTable(
                name: "Siaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Coordenadores = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Evento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Termino = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siaos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Siaos_SiaoId",
                table: "Pagamentos",
                column: "SiaoId",
                principalTable: "Siaos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
