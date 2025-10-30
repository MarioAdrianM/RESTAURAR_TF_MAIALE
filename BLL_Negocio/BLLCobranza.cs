using System;
using System.Linq;
using BE;
using MPP;
using Seguridad;

namespace BLL_Negocio
{
    public class BLLCobranza
    {
        private readonly MPPPago mppPago = new MPPPago();
        private readonly BLLFactura bllFactura = new BLLFactura();
        private readonly BLLBitacora bit = new BLLBitacora();
        private readonly MPPFactura mppFactura = new MPPFactura(); // para marcar cobradas

        public BEFactura UltimaFacturaEmitidaPorMesa(long mesaId)
        {
            var list = bllFactura.ListarTodo();
            return list
                .Where(f => f.MesaId == mesaId && string.Equals(f.Estado, "Emitida", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(f => f.Fecha)
                .ThenByDescending(f => f.Id)
                .FirstOrDefault();
        }

        public BEPago RegistrarPago(long facturaId, string medio, decimal importe, string autorizacion = null)
        {
            if (facturaId <= 0) throw new ArgumentException("Factura inválida.");
            var f = bllFactura.ObtenerPorId(facturaId);
            if (f == null) throw new InvalidOperationException("Factura no encontrada.");
            if (!string.Equals(f.Estado, "Emitida", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("La factura no está disponible para cobro.");

            medio = (medio ?? "").Trim();
            if (medio != "Efectivo" && medio != "Tarjeta" && medio != "QR")
                throw new ArgumentException("Medio de pago inválido.");

            if (importe <= 0) throw new ArgumentException("Importe debe ser mayor a cero.");

            decimal vuelto = 0m;
            if (medio == "Efectivo")
            {
                if (importe < f.ImporteTotal)
                    throw new InvalidOperationException("El importe recibido es insuficiente.");
                vuelto = decimal.Round(importe - f.ImporteTotal, 2);
            }
            else
            {
                // Tarjeta/QR → requerimos autorización y forzamos importe exacto
                if (string.IsNullOrWhiteSpace(autorizacion))
                    throw new InvalidOperationException("Falta autorización/código.");
                if (importe != f.ImporteTotal)
                    throw new InvalidOperationException("El importe debe ser exacto para pagos electrónicos.");
            }

            var pago = new BEPago
            {
                FacturaId = f.Id,
                MesaId = f.MesaId,
                Medio = medio,
                Monto = decimal.Round(importe, 2),
                Autorizacion = autorizacion,
                Vuelto = vuelto,
                Estado = "EnCustodiaMozo"
            };
            pago.Cobrador = Seguridad.Sesion.UsuarioActual?.Usuario ?? "DESCONOCIDO";

            mppPago.Guardar(pago);

            // Marcamos la factura como COBRADA
            f.Estado = "Cobrada";
            mppFactura.Guardar(f);
            // ==== Liberar mesa al cobrar la factura ====
            try
            {
                // Busco la factura recién cobrada para conocer la MesaId
                var bllFac = new BLLFactura();
                var fac = bllFac.ObtenerPorId(facturaId);
                if (fac != null && fac.MesaId > 0)
                {
                    var bllMesa = new BLLMesa();
                    var mesa = bllMesa.ListarObjeto(new BEMesa { Id = fac.MesaId });
                    if (mesa != null)
                    {
                        mesa.Estado = EstadosMesa.Libre;   // ← liberar la mesa
                        bllMesa.Guardar(mesa);
                    }
                }
            }
            catch
            {
                // no romper el flujo de cobro si falla la liberación
            }


            var usr = Sesion.UsuarioActual?.Usuario ?? "SYSTEM";
            bit.Registrar(usr, "PAGO_REGISTRADO", $"{medio} Fact:{f} Importe:{importe:n2} Vuelto:{vuelto:n2}");

            return pago;
        }
        public System.Collections.Generic.List<long> MesasPendientes()
        {
            return bllFactura.ListarTodo()
                .Where(f => f.Estado == "Emitida")
                .Select(f => f.MesaId)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public System.Collections.Generic.List<BEFactura> FacturasPendientesPorMesa(long mesaId)
        {
            return bllFactura.ListarTodo()
                .Where(f => f.MesaId == mesaId && f.Estado == "Emitida")
                .OrderByDescending(f => f.Fecha).ThenByDescending(f => f.Id)
                .ToList();
        }

    }
}
