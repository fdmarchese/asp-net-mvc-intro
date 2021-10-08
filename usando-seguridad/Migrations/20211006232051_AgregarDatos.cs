using System;
using Microsoft.EntityFrameworkCore.Migrations;
using usando_seguridad.Models;

namespace usando_seguridad.Migrations
{
    public partial class AgregarDatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var BancoId = Guid.NewGuid();

            migrationBuilder.InsertData(
                "Bancos",
                new string[]
                {
                    nameof(Banco.Id),
                    nameof(Banco.Nombre)
                },
                new object[]
                {
                    BancoId,
                    "Banco Santander"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
