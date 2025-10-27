using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BE;
using MPP;

namespace BLL_Negocio
{
    public class BLLReserva
    {
        private readonly MPPReserva mpp = new MPPReserva();
        private readonly BLLMesa bllMesa = new BLLMesa();
        private readonly TimeSpan DURACION = TimeSpan.FromMinutes(150); // 2h30
        private readonly TimeSpan TOL_CHECKIN_ANTES = TimeSpan.FromMinutes(15);
        private readonly TimeSpan TOL_CHECKIN_DESPUES = TimeSpan.FromMinutes(15);
        // === Helpers ===
        private static TimeSpan Hhmm(string hhmm) =>
            TimeSpan.ParseExact(hhmm, "hh\\:mm", CultureInfo.InvariantCulture);

        private static bool Solapan(DateTime aIni, DateTime aFin, DateTime bIni, DateTime bFin) =>
            aIni < bFin && bIni < aFin;

        private void ValidarNoPasado(DateTime fecha, string hora)
        {
            var inicio = fecha.Date + Hhmm(hora);
            if (inicio < DateTime.Now)
                throw new Exception("No se puede reservar en una fecha/hora pasada.");
        }

        private bool ConflictoMesa(long mesaId, DateTime fecha, string hora, long excluyeReservaId = 0)
        {
            var inicio = fecha.Date + Hhmm(hora);
            var fin = inicio + DURACION;

            var reservas = mpp.ListarTodo() ?? new List<BEReserva>();
            foreach (var r in reservas)
            {
                if (r.Id == excluyeReservaId) continue;
                if (r.MesaId != mesaId) continue;
                if (string.Equals(r.Estado, EstadosReserva.Cancelada, StringComparison.OrdinalIgnoreCase)) continue;
                if (r.Fecha.Date != fecha.Date) continue;

                var rIni = r.Fecha.Date + Hhmm(r.Hora);
                var rFin = rIni + DURACION;
                if (Solapan(inicio, fin, rIni, rFin)) return true;
            }
            return false;
        }

        // === API ===
        public long SugerirMesaId(DateTime fecha, string hora, int comensales)
        {
            if (comensales <= 0) throw new Exception("Comensales inválido.");
            ValidarNoPasado(fecha, hora);

            var disponibles = bllMesa.Disponibles(comensales); // filtra Habilitada/No bloqueada
            foreach (var m in disponibles)
                if (!ConflictoMesa(m.Id, fecha, hora)) return m.Id;

            return 0;
        }

        public bool Guardar(BEReserva r)
        {
            if (r == null) throw new Exception("Reserva nula.");
            if (r.Fecha == default) throw new Exception("Fecha obligatoria.");
            if (string.IsNullOrWhiteSpace(r.Hora)) throw new Exception("Hora obligatoria.");
            if (r.Comensales <= 0) throw new Exception("Comensales inválido.");
            if (string.IsNullOrWhiteSpace(r.ClienteNombreCompleto))
                throw new Exception("El nombre del cliente es obligatorio.");
            if (string.IsNullOrWhiteSpace(r.ClienteTelefono))
                throw new Exception("El teléfono del cliente es obligatorio.");
            // (Email opcional — si querés, podrías validar formato con Regex)

            // Normalizar estado según Alta/Edición
            var esAlta = r.Id == 0;
            if (esAlta) r.Estado = EstadosReserva.Pendiente;
            else
            {
                var actual = mpp.ListarObjeto(new BEReserva { Id = r.Id }) ?? throw new Exception("Reserva no encontrada.");
                r.Estado = actual.Estado; // el estado NO cambia acá
            }

            ValidarNoPasado(r.Fecha, r.Hora);
            if (r.MesaId > 0 && ConflictoMesa(r.MesaId, r.Fecha, r.Hora, excluyeReservaId: r.Id))
                throw new Exception("La mesa está ocupada en ese horario (ventana 2h30).");

            return mpp.Guardar(r);
        }

        public bool GuardarConSugerencia(BEReserva r)
        {
            if (r == null) throw new Exception("Reserva nula.");
            ValidarNoPasado(r.Fecha, r.Hora);

            if (r.MesaId == 0)
            {
                var mesaId = SugerirMesaId(r.Fecha, r.Hora, r.Comensales);
                if (mesaId == 0) throw new Exception("No hay mesas disponibles para esa fecha y hora.");
                r.MesaId = mesaId;
            }
            else
            {
                if (ConflictoMesa(r.MesaId, r.Fecha, r.Hora, excluyeReservaId: r.Id))
                    throw new Exception("La mesa está ocupada en ese horario (ventana 2h30).");
            }

            return Guardar(r);
        }

        public bool Eliminar(BEReserva r) => mpp.Eliminar(r);
        public List<BEReserva> ListarTodo() => mpp.ListarTodo();
        public BEReserva ListarObjeto(BEReserva r) => mpp.ListarObjeto(r);

        // Cambios de estado (y reflejo en Mesa)
        public bool CambiarEstado(long reservaId, string nuevoEstado)
        {
            var r = ListarObjeto(new BEReserva { Id = reservaId }) ?? throw new Exception("Reserva no encontrada.");

            switch (nuevoEstado)
            {
                case EstadosReserva.Confirmada:
                    ValidarNoPasado(r.Fecha, r.Hora);
                    r.Estado = EstadosReserva.Confirmada;
                    break;

                case EstadosReserva.CheckIn:
                    {
                        var ahora = DateTime.Now;
                        var inicio = r.Fecha.Date + Hhmm(r.Hora);
                        var rangoIni = inicio - TOL_CHECKIN_ANTES;
                        var rangoFin = inicio + TOL_CHECKIN_DESPUES;

                        if (ahora < rangoIni || ahora > rangoFin)
                            throw new Exception(
                                $"Check-In fuera de rango. Permitido entre {rangoIni:dd/MM/yyyy HH:mm} y {rangoFin:dd/MM/yyyy HH:mm}.");

                        r.Estado = EstadosReserva.CheckIn;

                        // marcar la mesa como Ocupada
                        var mesa = bllMesa.ListarObjeto(new BEMesa { Id = r.MesaId });
                        if (mesa != null)
                        {
                            mesa.Estado = EstadosMesa.Ocupada;
                            bllMesa.Guardar(mesa);
                        }
                        break;
                    }

                case EstadosReserva.Cancelada:
                    if (string.Equals(r.Estado, EstadosReserva.CheckIn, StringComparison.OrdinalIgnoreCase))
                        throw new Exception("No se puede cancelar una reserva en Check-In.");
                    r.Estado = EstadosReserva.Cancelada;

                    // si estaba ocupada por esta reserva y no hay comanda, liberar
                    var m2 = bllMesa.ListarObjeto(new BEMesa { Id = r.MesaId });
                    if (m2 != null && m2.Habilitada) { m2.Estado = EstadosMesa.Libre; bllMesa.Guardar(m2); }
                    break;

                default:
                    throw new Exception("Estado no válido.");
            }

            return mpp.Guardar(r);
        }
        public List<BE.BEMesa> MesasDisponibles(DateTime fecha, string hora, int comensales, string sector = null)
        {
            if (comensales <= 0) throw new Exception("Comensales inválido.");
            ValidarNoPasado(fecha, hora);

            var candidatas = bllMesa.Disponibles(comensales) ?? new List<BE.BEMesa>();
            // Quitar mesas de sectores inactivos
            var activos = new BLL_Negocio.BLLSector().ListarActivos()
                           ?.Select(a => a.Nombre.Trim().ToLowerInvariant())
                           .ToHashSet() ?? new HashSet<string>();

            candidatas = candidatas
                .Where(m => activos.Contains((m.Sector ?? "").Trim().ToLowerInvariant()))
                .ToList();

            if (!string.IsNullOrWhiteSpace(sector) &&
                !string.Equals(sector, "Todos", StringComparison.OrdinalIgnoreCase))
            {
                candidatas = candidatas
                    .Where(m => string.Equals(m.Sector?.Trim(), sector.Trim(), StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var libres = new List<BE.BEMesa>();
            foreach (var m in candidatas)
                if (!ConflictoMesa(m.Id, fecha, hora))
                    libres.Add(m);

            return libres.OrderBy(m => m.Capacidad).ThenBy(m => m.Numero).ToList();
        }

    }
}
