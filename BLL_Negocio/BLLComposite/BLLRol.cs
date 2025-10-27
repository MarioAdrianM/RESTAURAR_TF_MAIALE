using BE.BEComposite;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL_Negocio.BLLComposite
{
    public class BLLRol
    {
        const string ADMIN_ROLE_NAME = "ADMIN";

        private readonly MPPRol mpp = new MPPRol();
        public bool Guardar(BERol r)
        {
            if (r == null) throw new Exception("Rol inválido.");

            if (!string.IsNullOrWhiteSpace(r.Nombre) &&
                r.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) &&
                r.Id > 0)
                throw new Exception("El rol 'ADMIN' no puede ser modificado.");
            return mpp.Guardar(r);
        }
        public bool Eliminar(BERol r) 
        {
            if (r == null) throw new Exception("Rol inválido.");

            if (!string.IsNullOrWhiteSpace(r.Nombre) &&
                r.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) &&
                r.Id > 0)
                throw new Exception("El rol 'ADMIN' no puede ser modificado.");
            return mpp.Eliminar(r);
        }
        public bool VerificarExistenciaObjeto(BERol r) => mpp.VerificarExistenciaObjeto(r);
        public List<BERol> ListarTodo() => mpp.ListarTodo();
        public BERol ListarObjeto(BERol r) => mpp.ListarObjeto(r);

        public bool AsociarRolaPermiso(BERol r, BEPermiso p) => mpp.AsociarRolPermiso(r, p);
        public bool DesasociarRolaPermiso(BERol r, BEPermiso p) => mpp.DesasociarRolPermiso(r, p);
        public bool AsociarRolARol(BERol padre, BERol hijo) => mpp.AsociarRolRol(padre, hijo);
        public bool DesasociarRolARol(BERol padre, BERol hijo) => mpp.DesasociarRolRol(padre, hijo);
    }
}
