using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using MPP;

namespace BLL_Negocio
{
    public class BLLSector
    {
        private readonly MPPSector mpp = new MPPSector();

        public bool Guardar(BESector s)
        {
            if (s == null) throw new Exception("Sector nulo.");
            if (string.IsNullOrWhiteSpace(s.Nombre)) throw new Exception("Nombre obligatorio.");
            s.Nombre = s.Nombre.Trim();
            return mpp.Guardar(s);
        }

        public bool Eliminar(BESector s) => mpp.Eliminar(s);
        public List<BESector> ListarTodo() => mpp.ListarTodo();

        public List<BESector> ListarActivos() =>
            (mpp.ListarTodo() ?? new List<BESector>()).Where(x => x.Activo).OrderBy(x => x.Nombre).ToList();

        public BESector ListarObjeto(BESector s) => mpp.ListarObjeto(s);

        public bool Activar(long id, bool activo)
        {
            var s = ListarObjeto(new BESector { Id = id }) ?? throw new Exception("Sector no encontrado.");
            s.Activo = activo;
            return Guardar(s);
        }
    }
}
