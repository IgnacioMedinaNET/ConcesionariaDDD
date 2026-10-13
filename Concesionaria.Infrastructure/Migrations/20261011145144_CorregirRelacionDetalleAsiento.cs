using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorregirRelacionDetalleAsiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesAsientoContable_AsientosContables_AsientoContableId1",
                table: "DetallesAsientoContable");

            migrationBuilder.DropIndex(
                name: "IX_DetallesAsientoContable_AsientoContableId1",
                table: "DetallesAsientoContable");

            migrationBuilder.DropColumn(
                name: "AsientoContableId1",
                table: "DetallesAsientoContable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AsientoContableId1",
                table: "DetallesAsientoContable",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAsientoContable_AsientoContableId1",
                table: "DetallesAsientoContable",
                column: "AsientoContableId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesAsientoContable_AsientosContables_AsientoContableId1",
                table: "DetallesAsientoContable",
                column: "AsientoContableId1",
                principalTable: "AsientosContables",
                principalColumn: "Id");
        }
    }
}
