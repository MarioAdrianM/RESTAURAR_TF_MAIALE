using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using MPP;

namespace BLL_Negocio
{
    public class BLLMesa
    {
        private readonly MPPMesa mpp = new MPPMesa();

        public bool Guardar(BEMesa m)
        {
            if (m == null) throw new Exception("Mesa nula.");
            if (m.Numero <= 0) throw new Exception("Número de mesa inválido.");
            if (m.Capacidad <= 0) throw new Exception("Capacidad inválida.");
            if (string.IsNullOrWhiteSpace(m.Sector)) throw new Exception("sector invalido.");
            var bs = new BLL_Negocio.BLLSector();
            bool ok = (bs.ListarActivos() ?? new List<BE.BESector>())
                      .Any(s => string.Equals(s.Nombre, m.Sector, StringComparison.OrdinalIgnoreCase));
            if (!ok) throw new Exception("No se puede guardar una mesa en un sector inactivo.");

            if (!m.Habilitada) m.Estado = EstadosMesa.Bloqueada;
            else if (string.IsNullOrWhiteSpace(m.Estado) || m.Estado == EstadosMesa.Bloqueada)
                m.Estado = EstadosMesa.Libre;
            return mpp.Guardar(m);
        }

        public bool Eliminar(BEMesa m) => mpp.Eliminar(m);
        public List<BEMesa> ListarTodo() => mpp.ListarTodo();
        public BEMesa ListarObjeto(BEMesa m) => mpp.ListarObjeto(m);

        // Helper 
        public List<BE.BEMesa> Disponibles(int comensales)
        {
            return ListarTodo()
                .Where(x => x.Habilitada
                    && !string.Equals(x.Estado, BE.EstadosMesa.Bloqueada, StringComparison.OrdinalIgnoreCase)
                    && x.Capacidad >= comensales)
                .OrderBy(x => x.Capacidad)
                .ThenBy(x => x.Numero)
                .ToList();
        }


        public bool TieneReservasAsociadas(long mesaId) => mpp.TieneReservasAsociadas(mesaId);

    }

}
