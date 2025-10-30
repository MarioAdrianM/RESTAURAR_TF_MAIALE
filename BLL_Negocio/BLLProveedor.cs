using Abstraccion;
using BE;
using MPP;
using System.Collections.Generic;
using System.Linq;

namespace BLL_Negocio
{
    public class BLLProveedor : IGestor<BEProveedor>
    {
        private readonly MPPProveedor mpp = new MPPProveedor();
        private readonly MPPProveedorFamilia mppRel = new MPPProveedorFamilia();
        private readonly MPPFamilia mppFam = new MPPFamilia();

        public bool CrearXML() { return mpp.CrearXML(); }
        public bool Guardar(BEProveedor o) { return mpp.Guardar(o); }
        public bool Eliminar(BEProveedor o) { return mpp.Eliminar(o); }
        public BEProveedor ListarObjeto(BEProveedor o) { return mpp.ListarObjeto(o); }
        public List<BEProveedor> ListarTodo() { return mpp.ListarTodo(); }
        public long ObtenerUltimoId() { return mpp.ObtenerUltimoId(); }
        public bool VerificarExistenciaObjeto(BEProveedor o) { return mpp.VerificarExistenciaObjeto(o); }

        public List<BEProveedor> ListarActivos()
        {
            var l = mpp.ListarTodo();
            return l == null ? new List<BEProveedor>() : l.Where(x => x.Activo).ToList();
        }

        // Vinculaciones Proveedor–Familias
        public void VincularFamilia(long proveedorId, long familiaId) { mppRel.Vincular(proveedorId, familiaId); }
        public void DesvincularFamilia(long proveedorId, long familiaId) { mppRel.Desvincular(proveedorId, familiaId); }

        public List<BEFamilia> FamiliasDeProveedor(long proveedorId)
        {
            var ids = mppRel.FamiliasIdsDeProveedor(proveedorId);
            var todas = mppFam.ListarTodo();
            return todas.Where(f => ids.Contains(f.Id)).ToList();
        }
    }
}
