using Abstraccion;
using Backup;
using BE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPReserva : IGestor<BEReserva>
    {
        public MPPReserva() { CrearXML(); }

        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        public bool CrearXML()
        {
            if (!File.Exists(ruta))
            {
                BDXML = new XDocument(
                    new XElement("Root",
                        new XElement("Usuarios"),
                        new XElement("Roles"),
                        new XElement("Permisos"),
                        new XElement("Usuario_Roles"),
                        new XElement("Usuario_Permisos"),
                        new XElement("Rol_Rol"),
                        new XElement("Rol_Permisos"),
                        new XElement("Bitacora"),
                        new XElement("Mesas"),
                        new XElement("Reservas") // ← NUEVO
                    )
                );
                BDXML.Save(ruta);
            }
            else
            {
                BDXML = XDocument.Load(ruta);
                if (BDXML.Root.Element("Reservas") == null)
                {
                    BDXML.Root.Add(new XElement("Reservas"));
                    BDXML.Save(ruta);
                }
            }
            return true;
        }

        public long ObtenerUltimoId()
        {
            CrearXML();
            var ids = BDXML.Root.Element("Reservas").Elements("reserva").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public bool VerificarExistenciaObjeto(BEReserva r)
        {
            CrearXML();
            if (r == null) return false;

            if (r.Id > 0)
                return BDXML.Root.Element("Reservas").Elements("reserva")
                        .Any(x => (long)x.Attribute("Id") == r.Id);

            // Duplicado lógico exacto (misma mesa/fecha/hora)
            return BDXML.Root.Element("Reservas").Elements("reserva").Any(x =>
                (long?)x.Element("MesaId") == r.MesaId &&
                DateTime.Parse((string)x.Element("Fecha")).Date == r.Fecha.Date &&
                string.Equals((string)x.Element("Hora"), r.Hora, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals((string)x.Element("Estado"), BE.EstadosReserva.Cancelada, StringComparison.OrdinalIgnoreCase));
        }

        public bool Guardar(BEReserva r)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);

            if (r.Id == 0)
            {
                if (VerificarExistenciaObjeto(r)) throw new Exception("Ya existe una reserva igual.");
                r.Id = ObtenerUltimoId() + 1;
                BDXML.Root.Element("Reservas").Add(Serializar(r));
            }
            else
            {
                var q = BDXML.Root.Element("Reservas").Elements("reserva").Where(x => (long)x.Attribute("Id") == r.Id);
                if (!q.Any()) throw new Exception("Reserva no encontrada.");

                foreach (var n in q)
                {
                    n.Element("Fecha").Value = r.Fecha.ToString("yyyy-MM-dd");
                    n.Element("Hora").Value = r.Hora;
                    n.Element("Comensales").Value = r.Comensales.ToString();
                    n.Element("MesaId").Value = r.MesaId.ToString();
                    n.Element("Estado").Value = r.Estado;
                    n.Element("Observaciones").Value = r.Observaciones ?? "";
                    n.Element("ClienteNombreCompleto").Value = r.ClienteNombreCompleto ?? "";
                    n.Element("ClienteTelefono").Value = r.ClienteTelefono ?? "";
                    n.Element("ClienteEmail").Value = r.ClienteEmail ?? "";

                }
            }
            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BEReserva r)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            BDXML.Root.Element("Reservas").Elements("reserva")
                .Where(x => (long)x.Attribute("Id") == r.Id)
                .Remove();
            BDXML.Save(ruta);
            return true;
        }

        public List<BEReserva> ListarTodo()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return BDXML.Root.Element("Reservas").Elements("reserva").Select(Deserializar).ToList();
        }

        public BEReserva ListarObjeto(BEReserva filtro)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            XElement x = null;
            if (filtro.Id > 0)
                x = BDXML.Root.Element("Reservas").Elements("reserva")
                        .FirstOrDefault(n => (long)n.Attribute("Id") == filtro.Id);
            return x == null ? null : Deserializar(x);
        }

        // helpers
        private static XElement Serializar(BEReserva r) =>
            new XElement("reserva",
                new XAttribute("Id", r.Id),
                new XElement("Fecha", r.Fecha.ToString("yyyy-MM-dd")),
                new XElement("Hora", r.Hora),
                new XElement("Comensales", r.Comensales),
                new XElement("MesaId", r.MesaId),
                new XElement("Estado", r.Estado ?? BE.EstadosReserva.Pendiente),
                new XElement("Observaciones", r.Observaciones ?? ""),
                new XElement("ClienteNombreCompleto", r.ClienteNombreCompleto ?? ""),
                new XElement("ClienteTelefono", r.ClienteTelefono ?? ""),
                new XElement("ClienteEmail", r.ClienteEmail ?? "")

            );

        private static BEReserva Deserializar(XElement x) =>
            new BEReserva
            {
                Id = (long)x.Attribute("Id"),
                Fecha = DateTime.Parse((string)x.Element("Fecha")),
                Hora = (string)x.Element("Hora"),
                Comensales = (int)x.Element("Comensales"),
                MesaId = (long)x.Element("MesaId"),
                Estado = (string)x.Element("Estado"),
                Observaciones = (string)x.Element("Observaciones"),
                ClienteNombreCompleto = (string)x.Element("ClienteNombreCompleto"),
                ClienteTelefono = (string)x.Element("ClienteTelefono"),
                ClienteEmail = (string)x.Element("ClienteEmail")

            };
    }
}
