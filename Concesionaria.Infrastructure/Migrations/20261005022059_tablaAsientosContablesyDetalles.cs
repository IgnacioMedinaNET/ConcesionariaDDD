using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tablaAsientosContablesyDetalles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsientosContables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Origen = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsientosContables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsientosContables_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesAsientoContable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AsientoContableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AsientoContableId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CuentaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Debe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Haber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesAsientoContable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesAsientoContable_AsientosContables_AsientoContableId",
                        column: x => x.AsientoContableId,
                        principalTable: "AsientosContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesAsientoContable_AsientosContables_AsientoContableId1",
                        column: x => x.AsientoContableId1,
                        principalTable: "AsientosContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetallesAsientoContable_Cuentas_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuentas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsientosContables_EmpresaId",
                table: "AsientosContables",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAsientoContable_AsientoContableId",
                table: "DetallesAsientoContable",
                column: "AsientoContableId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAsientoContable_AsientoContableId1",
                table: "DetallesAsientoContable",
                column: "AsientoContableId1");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAsientoContable_CuentaId",
                table: "DetallesAsientoContable",
                column: "CuentaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesAsientoContable");

            migrationBuilder.DropTable(
                name: "AsientosContables");
        }
    }
}
