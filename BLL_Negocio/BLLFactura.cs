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
        // Caja: emite factura a partir de una comanda cerrada con solicitud
        public BEFactura EmitirDesdeComanda(long comandaId, int puntoVenta = 1, string tipoCbte = "B")
        {
            var bllCom = new BLL_Negocio.BLLComanda();
            var c = bllCom.ListarObjeto(new BE.BEComanda { Id = comandaId });
            if (c == null) throw new InvalidOperationException("Comanda no encontrada.");
            if (c.Estado == BE.EstadosComanda.Facturada) throw new InvalidOperationException("La comanda ya fue facturada.");
            if (c.Estado != BE.EstadosComanda.Cerrada || !c.FacturaSolicitada)
                throw new InvalidOperationException("La comanda debe estar CERRADA y con factura solicitada.");

            if (c.Total <= 0) throw new InvalidOperationException("Total inválido.");

            // usa tu método actual Emitir(...)
            var fac = Emitir(c.MesaId, c.Total, puntoVenta, tipoCbte);

            // marca la comanda como facturada
            bllCom.MarcarFacturada(c.Id);

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
