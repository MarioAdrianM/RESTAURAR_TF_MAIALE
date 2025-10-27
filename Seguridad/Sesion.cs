using BE;

namespace Seguridad
{
    public static class Sesion
    {
        public static BE.BEUsuario UsuarioActual { get; set; }
        public static bool EstaLogueado => UsuarioActual != null;
        public static void Cerrar() => UsuarioActual = null;
    }
}
