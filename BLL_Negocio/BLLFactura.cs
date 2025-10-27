using System;
using System.Collections.Generic;
using BE;
using MPP;
using Seguridad;

namespace BLL_Negocio
{
    public class BLLFactura
    {
        private readonly MPPFactura mpp = new MPPFactura();
        private readonly BLLBitacora bit = new BLLBitacora();

        #region "API"
        /// <summary>
        /// Emite una factura simple (Tipo/PV correlativo) y la persiste.
        /// </summary>
        public BEFactura Emitir(long mesaId, decimal total, int puntoVenta = 1, string tipoCbte = "B")
        {
            if (mesaId <= 0) throw new ArgumentException("Mesa inválida.");
            if (total <= 0) throw new ArgumentException("Importe debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(tipoCbte)) tipoCbte = "B";

            var fac = new BEFactura
            {
                MesaId = mesaId,
                ImporteTotal = decimal.Round(total, 2),
                PuntoVenta = puntoVenta,
                TipoCbte = tipoCbte.Trim().ToUpperInvariant(),
                Fecha = DateTime.Now,
                Estado = "Emitida"
            };

            mpp.Guardar(fac);

            var usr = Sesion.UsuarioActual?.Usuario ?? "SYSTEM";
            bit.Registrar(usr, "FACTURA_EMITIDA", $"{fac} Mesa:{mesaId} Total:{fac.ImporteTotal:n2}");

            return fac;
        }

        public bool Anular(BEFactura f, string motivo = null)
        {
            if (f == null) return false;
            f.Estado = "Anulada";
            var ok = mpp.Guardar(f);
            if (ok)
            {
                var usr = Sesion.UsuarioActual?.Usuario ?? "SYSTEM";
                bit.Registrar(usr, "FACTURA_ANULADA", $"{f} {motivo ?? ""}".Trim());
            }
            return ok;
        }

        public List<BEFactura> ListarTodo() => mpp.ListarTodo();
        public BEFactura ObtenerPorId(long id) => mpp.ListarObjeto(new BEFactura { Id = id });
        #endregion
    }
}
