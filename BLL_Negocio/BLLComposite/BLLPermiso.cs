using BE.BEComposite;
using MPP;
using System.Collections.Generic;

namespace BLL_Negocio.BLLComposite
{
    public class BLLPermiso
    {
        private readonly MPPPermiso mpp = new MPPPermiso();
        public bool Guardar(BEPermiso p) => mpp.Guardar(p);
        public bool Eliminar(BEPermiso p) => mpp.Eliminar(p);
        public bool VerificarExistenciaObjeto(BEPermiso p) => mpp.VerificarExistenciaObjeto(p);
        public List<BEPermiso> ListarTodo() => mpp.ListarTodo();
        public BEPermiso ListarObjeto(BEPermiso p) => mpp.ListarObjeto(p);
    }
}
