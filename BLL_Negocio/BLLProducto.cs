using Abstraccion;
using BE;
using MPP;
using System.Collections.Generic;
using System.Linq;

namespace BLL_Negocio
{
    public class BLLProducto : IGestor<BEProducto>
    {
        private readonly MPPProducto mpp = new MPPProducto();

        public bool CrearXML() { return mpp.CrearXML(); }
        public bool Guardar(BEProducto o) { return mpp.Guardar(o); }
        public bool Eliminar(BEProducto o) { return mpp.Eliminar(o); }
        public BEProducto ListarObjeto(BEProducto o) { return mpp.ListarObjeto(o); }
        public List<BEProducto> ListarTodo() { return mpp.ListarTodo(); }
        public long ObtenerUltimoId() { return mpp.ObtenerUltimoId(); }
        public bool VerificarExistenciaObjeto(BEProducto o) { return mpp.VerificarExistenciaObjeto(o); }

        public List<BEProducto> ListarActivos()
        {
            var l = mpp.ListarTodo();
            return l == null ? new List<BEProducto>() : l.Where(x => x.Activo).ToList();
        }
    }
}
