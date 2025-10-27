using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.BEComposite
{
    public class BEPermiso : BEComposite
    {
        public BEPermiso(long pId, string pNombre) : base(pId, pNombre) { }

        public override void Agregar(BEComposite oBEComposite)
        {
            try { throw new Exception("Error: No se puede agregar un permiso a un permiso."); }
            catch (Exception ex) { throw ex; }
        }

        public override IList<BEComposite> ObtenerHijos()
        {
            try { throw new Exception("Error: Un permiso no posee hijos."); }
            catch (Exception ex) { throw ex; }
        }
    }
}
