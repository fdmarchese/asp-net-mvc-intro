using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using usando_entity_framework.Models;

namespace usando_entity_framework.Database
{
    public class AlumnosDbContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Este constructor es el que necesitamos para usar luego en la clase <see cref="Startup"/> en
        /// donde inicializamos el objeto de contexto de base de datos que luego será utilizado por toda nuestra aplicación.
        /// </summary>
        public AlumnosDbContext(DbContextOptions<AlumnosDbContext> opciones) : base(opciones) 
        {
        }

        #endregion

        // Las propiedades del contexto de datos que utilizaremos serán de tipo 
        // DbSet<MiClase> para cada una de las entidades que queremos que se persista.
        #region Propiedades

        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<MateriaAlumno> MateriaAlumnos { get; set; }
        public DbSet<Telefono> Telefonos { get; set; }
        public DbSet<TipoTelefono> TipoTelefonos { get; set; }
        public DbSet<Contacto> Contactos { get; set; }

        #endregion
    }
}
