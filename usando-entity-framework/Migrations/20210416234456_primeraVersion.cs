using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace usando_entity_framework.Migrations
{
    public partial class primeraVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alumnos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoTelefonos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTelefonos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contactos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    LinkedIn = table.Column<string>(maxLength: 100, nullable: true),
                    Twitter = table.Column<string>(maxLength: 100, nullable: true),
                    Instagram = table.Column<string>(maxLength: 100, nullable: true),
                    Facebook = table.Column<string>(maxLength: 100, nullable: true),
                    AlumnoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contactos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contactos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    Anio = table.Column<int>(nullable: false),
                    Cuatrimestre = table.Column<int>(nullable: false),
                    ProfesorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materias_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telefonos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Numero = table.Column<string>(maxLength: 50, nullable: false),
                    TipoTelefonoId = table.Column<Guid>(nullable: false),
                    AlumnoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telefonos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Telefonos_TipoTelefonos_TipoTelefonoId",
                        column: x => x.TipoTelefonoId,
                        principalTable: "TipoTelefonos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MateriaAlumnos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MateriaId = table.Column<Guid>(nullable: false),
                    AlumnoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaAlumnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MateriaAlumnos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriaAlumnos_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contactos_AlumnoId",
                table: "Contactos",
                column: "AlumnoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MateriaAlumnos_AlumnoId",
                table: "MateriaAlumnos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaAlumnos_MateriaId",
                table: "MateriaAlumnos",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_ProfesorId",
                table: "Materias",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_AlumnoId",
                table: "Telefonos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_TipoTelefonoId",
                table: "Telefonos",
                column: "TipoTelefonoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contactos");

            migrationBuilder.DropTable(
                name: "MateriaAlumnos");

            migrationBuilder.DropTable(
                name: "Telefonos");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Alumnos");

            migrationBuilder.DropTable(
                name: "TipoTelefonos");

            migrationBuilder.DropTable(
                name: "Profesores");
        }
    }
}
