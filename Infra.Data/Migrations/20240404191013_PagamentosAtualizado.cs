using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class PagamentosAtualizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cancelado",
                table: "Pagamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DtTransferencia",
                table: "Pagamentos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObsTransferencia",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Transferido",
                table: "Pagamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelado",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "DtTransferencia",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "ObsTransferencia",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "Transferido",
                table: "Pagamentos");
        }
    }
}
