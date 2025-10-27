using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.BEComposite
{
    // Componente base del patrón Composite (Rol/Permiso)
    public abstract class BEComposite : BEEntidad
    {
        #region "Propiedades"
        public string Nombre { get; set; }
        #endregion

        #region "Constructor"
        protected BEComposite(long pId, string pNombre)
        {
            Id = pId;
            Nombre = pNombre;
        }
        #endregion

        #region "Métodos"
        public abstract void Agregar(BEComposite oBEComposite);
        public abstract IList<BEComposite> ObtenerHijos();
        #endregion
    }
}

