using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE.BEComposite;

namespace BE
{
    public class BEUsuario : BEEntidad
    {
        #region "Propiedades"
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Usuario { get; set; }
        public string Password { get; set; }         // Se guardará encriptado en XML
        public bool Activo { get; set; }
        public bool Bloqueado { get; set; }
        public List<BERol> listaRoles { get; set; } = new List<BERol>();
        public bool DebeCambiarPassword { get; set; } = false;
        public List<BEPermiso> listaPermisos { get; set; } = new List<BEPermiso>();
        #endregion

        #region "Métodos"
        public override string ToString() => Id.ToString().Trim();
        #endregion
    }
}
