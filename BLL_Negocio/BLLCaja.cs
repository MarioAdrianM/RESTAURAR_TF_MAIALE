using System;
using System.Linq;
using System.Collections.Generic;
using BE;
using MPP;
using Seguridad;

namespace BLL_Negocio
{
    public class BLLCaja
    {
        private readonly MPPCaja mppCaja = new MPPCaja();
        private readonly MPPRendicion mppRend = new MPPRendicion();
        private readonly MPPPago mppPago = new MPPPago();
        private readonly BLLBitacora bit = new BLLBitacora();

        // === Apertura ===
        public BECaja Abrir(string punto, string turno, decimal fondoInicial, decimal umbralDif)
        {
            if (string.IsNullOrWhiteSpace(punto)) punto = "Caja1";
            if (string.IsNullOrWhiteSpace(turno)) turno = "Noche";
            if (fondoInicial < 0) throw new ArgumentException("Fondo inicial inválido.");
            if (umbralDif < 0) umbralDif = 0;

            // Regla: una caja abierta por (punto, turno) a la vez
            var abiertas = mppCaja.ListarTodo()
                .Where(caja => caja.Abierta
                             && string.Equals(caja.Punto, punto, StringComparison.OrdinalIgnoreCase)
                             && string.Equals(caja.Turno, turno, StringComparison.OrdinalIgnoreCase));
            if (abiertas.Any())
                throw new InvalidOperationException("Ya existe una caja abierta para ese Punto/Turno.");

            // No permitir pasado/futuro: siempre Now (no recibimos fecha por parámetro)
            var ahora = DateTime.Now;

            var c = new BECaja
            {
                Punto = punto.Trim(),
                Turno = turno.Trim(),
                FondoInicial = decimal.Round(fondoInicial, 2),
                UmbralDiferencia = decimal.Round(umbralDif, 2),
                Responsable = Sesion.UsuarioActual?.Usuario ?? "SYSTEM",
                Abierta = true,
                FechaApertura = ahora,
                
            };
            mppCaja.Guardar(c);

            bit.Registrar(c.Responsable, "APERTURA_CAJA_OK",
                $"Nombre:{c.Nombre} Fondo:{c.FondoInicial:n2} Umbral:{c.UmbralDiferencia:n2}");
            return c;
        }

        public BECaja ObtenerCajaAbierta(string punto, string turno)
        {
            return mppCaja.ListarTodo()
                .Where(c => c.Abierta)
                .OrderByDescending(c => c.FechaApertura)
                .FirstOrDefault(c =>
                    string.Equals(c.Punto, punto ?? "Caja1", StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(c.Turno, turno ?? "Noche", StringComparison.OrdinalIgnoreCase));
        }

        public List<BECaja> ListarAbiertas()
        {
            return mppCaja.ListarTodo()
                          .Where(c => c.Abierta)
                          .OrderByDescending(c => c.FechaApertura)
                          .ToList(); // BECaja.Nombre se calcula solo
        }

        // === Rendiciones ===
        public (decimal ef, decimal tj, decimal qr) CalcularEsperado(string mozoUsuario, DateTime desde)
        {
            var pagos = mppPago.ListarTodo()
                .Where(p => p.Fecha >= desde &&
                            string.Equals(p.Estado, "EnCustodiaMozo", StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(p.Cobrador ?? "", mozoUsuario ?? "", StringComparison.OrdinalIgnoreCase));

            decimal ef = pagos.Where(p => p.Medio == "Efectivo").Sum(p => p.Monto);
            decimal tj = pagos.Where(p => p.Medio == "Tarjeta").Sum(p => p.Monto);
            decimal qr = pagos.Where(p => p.Medio == "QR").Sum(p => p.Monto);
            return (ef, tj, qr);
        }

        // Resumen de mozos con pendientes para una caja abierta
        public List<(string Mozo, decimal EspEf, decimal EspTj, decimal EspQr, decimal Total)> MozosConPendientes(long cajaId)
        {
            var caja = mppCaja.ListarObjeto(new BECaja { Id = cajaId });
            if (caja == null || !caja.Abierta) return new List<(string, decimal, decimal, decimal, decimal)>();

            var pagos = mppPago.ListarTodo()
                .Where(p => p.Fecha >= caja.FechaApertura && p.Estado == "EnCustodiaMozo");

            var q = pagos
                .GroupBy(p => p.Cobrador ?? "")
                .Select(g => (
                    Mozo: g.Key,
                    EspEf: g.Where(p => p.Medio == "Efectivo").Sum(p => p.Monto),
                    EspTj: g.Where(p => p.Medio == "Tarjeta").Sum(p => p.Monto),
                    EspQr: g.Where(p => p.Medio == "QR").Sum(p => p.Monto)
                ))
                .Select(x => (x.Mozo, x.EspEf, x.EspTj, x.EspQr, x.EspEf + x.EspTj + x.EspQr))
                .OrderByDescending(x => x.Item5)
                .ToList();

            return q;
        }

        public BERendicion RecibirRendicion(long cajaId, string mozoUsuario,
            decimal entEf, decimal entTj, decimal entQr, string obs = null)
        {
            var caja = mppCaja.ListarObjeto(new BECaja { Id = cajaId });
            if (caja == null || !caja.Abierta) throw new InvalidOperationException("Caja no abierta.");

            var (espEf, espTj, espQr) = CalcularEsperado(mozoUsuario, caja.FechaApertura);
            var dif = (entEf - espEf) + (entTj - espTj) + (entQr - espQr);
            var estado = Math.Abs(dif) <= caja.UmbralDiferencia ? "OK" : "ConDiferencias";

            if (estado == "ConDiferencias" && string.IsNullOrWhiteSpace(obs))
                throw new InvalidOperationException(
                    $"La diferencia ({dif:n2}) supera el umbral ({caja.UmbralDiferencia:n2}). Ingresá una observación.");

            var r = new BERendicion
            {
                CajaId = caja.Id,
                MozoUsuario = mozoUsuario ?? "",
                EspEfectivo = decimal.Round(espEf, 2),
                EspTarjeta = decimal.Round(espTj, 2),
                EspQR = decimal.Round(espQr, 2),
                EntEfectivo = decimal.Round(entEf, 2),
                EntTarjeta = decimal.Round(entTj, 2),
                EntQR = decimal.Round(entQr, 2),
                DiferenciaTotal = decimal.Round(dif, 2),
                Estado = estado,
                Observacion = obs ?? ""
            };
            mppRend.Guardar(r);

            // Marcar pagos del mozo como rendidos
            var pagos = mppPago.ListarTodo()
                .Where(p => p.Fecha >= caja.FechaApertura &&
                            string.Equals(p.Estado, "EnCustodiaMozo", StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(p.Cobrador ?? "", mozoUsuario ?? "", StringComparison.OrdinalIgnoreCase))
                .ToList();
            foreach (var p in pagos) { p.Estado = "Rendido"; mppPago.Guardar(p); }

            var usr = Sesion.UsuarioActual?.Usuario ?? "SYSTEM";
            bit.Registrar(usr, estado == "OK" ? "RENDICION_OK" : "RENDICION_CON_DIFERENCIAS",
                $"Caja:{caja.Nombre} Mozo:{mozoUsuario} Esp Ef:{espEf:n2}/Tj:{espTj:n2}/QR:{espQr:n2} Ent Ef:{entEf:n2}/Tj:{entTj:n2}/QR:{entQr:n2} Dif:{dif:n2}");
            return r;
        }

        // Cierre de caja (turno)
        public void CerrarCaja(long cajaId)
        {
            var caja = mppCaja.ListarObjeto(new BECaja { Id = cajaId });
            if (caja == null || !caja.Abierta) throw new InvalidOperationException("Caja no abierta.");

            // Conciliar pagos ya rendidos
            var pagosRendidos = mppPago.ListarTodo()
                .Where(p => p.Fecha >= caja.FechaApertura && p.Estado == "Rendido")
                .ToList();
            foreach (var p in pagosRendidos) { p.Estado = "Conciliado"; mppPago.Guardar(p); }

            caja.Abierta = false;
            mppCaja.Guardar(caja);

            var usr = Sesion.UsuarioActual?.Usuario ?? "SYSTEM";
            bit.Registrar(usr, "CIERRE_CAJA_OK", $"Caja:{caja.Nombre} Punto:{caja.Punto} Turno:{caja.Turno}");
        }
        public class MozoPendienteRow
        {
            public string Mozo { get; set; }
            public decimal EspEf { get; set; }
            public decimal EspTj { get; set; }
            public decimal EspQr { get; set; }
            public decimal Total => EspEf + EspTj + EspQr;
        }
    }
}
