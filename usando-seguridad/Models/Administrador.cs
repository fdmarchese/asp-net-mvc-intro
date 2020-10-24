
namespace usando_seguridad.Models
{
    public class Administrador : Usuario
    {
        public override Rol Rol => Rol.Administrador;
    }
}
