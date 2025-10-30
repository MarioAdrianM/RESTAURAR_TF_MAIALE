using Abstraccion;
using BE;
using MPP;
using System.Collections.Generic;
using System.Linq;

namespace BLL_Negocio
{
    public class BLLFamilia : IGestor<BEFamilia>
    {
        private readonly MPPFamilia mpp = new MPPFamilia();

        public bool CrearXML() { return mpp.CrearXML(); }
        public bool Guardar(BEFamilia o) { return mpp.Guardar(o); }
        public bool Eliminar(BEFamilia o) { return mpp.Eliminar(o); }
        public BEFamilia ListarObjeto(BEFamilia o) { return mpp.ListarObjeto(o); }
        public List<BEFamilia> ListarTodo() { return mpp.ListarTodo(); }
        public long ObtenerUltimoId() { return mpp.ObtenerUltimoId(); }
        public bool VerificarExistenciaObjeto(BEFamilia o) { return mpp.VerificarExistenciaObjeto(o); }

        public List<BEFamilia> ListarActivas()
        {
            var l = mpp.ListarTodo();
            return l == null ? new List<BEFamilia>() : l.Where(x => x.Activa).ToList();
        }
    }
}
