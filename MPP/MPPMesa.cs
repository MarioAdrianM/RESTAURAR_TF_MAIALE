using Abstraccion;
using Backup;
using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPMesa : IGestor<BEMesa>
    {
        public MPPMesa() { CrearXML(); }

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
                        new XElement("Mesas") // ← NUEVO
                    )
                );
                BDXML.Save(ruta);
            }
            else
            {
                BDXML = XDocument.Load(ruta);
                if (BDXML.Root.Element("Mesas") == null)
                {
                    BDXML.Root.Add(new XElement("Mesas"));
                    BDXML.Save(ruta);
                }
            }
            return true;
        }

        public long ObtenerUltimoId()
        {
            CrearXML();
            var ids = BDXML.Root.Element("Mesas").Elements("mesa").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public bool VerificarExistenciaObjeto(BEMesa m)
        {
            CrearXML();
            if (m == null) return false;

            // Modificación: ¿hay otra mesa con mismo Número?
            if (m.Id > 0)
                return BDXML.Root.Element("Mesas").Elements("mesa")
                        .Any(x => (int)x.Element("Numero") == m.Numero &&
                                  (long)x.Attribute("Id") != m.Id);

            // Alta: ¿existe ese Número?
            return BDXML.Root.Element("Mesas").Elements("mesa")
                     .Any(x => (int)x.Element("Numero") == m.Numero);
        }

        public bool Guardar(BEMesa m)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);

            if (m.Id == 0)
            {
                if (VerificarExistenciaObjeto(m)) throw new Exception("Ya existe una mesa con ese número.");
                m.Id = ObtenerUltimoId() + 1;
                BDXML.Root.Element("Mesas").Add(Serializar(m));
            }
            else
            {
                if (VerificarExistenciaObjeto(m)) throw new Exception("Ya existe una mesa con ese número.");
                var q = BDXML.Root.Element("Mesas").Elements("mesa").Where(x => (long)x.Attribute("Id") == m.Id);
                if (!q.Any()) throw new Exception("Mesa no encontrada.");
                foreach (var n in q)
                {
                    n.Element("Numero").Value = m.Numero.ToString();
                    n.Element("Capacidad").Value = m.Capacidad.ToString();
                    n.Element("Sector").Value = m.Sector ?? "";
                    n.Element("Estado").Value = m.Estado;
                    n.Element("Habilitada").Value = m.Habilitada ? "true" : "false";
                    n.Element("Observaciones").Value = m.Observaciones ?? "";
                }
            }
            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BEMesa m)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);

            
            bool tieneReservas = BDXML.Root.Element("Reservas")?
                .Elements("reserva")
                .Any(x => (long?)x.Element("MesaId") == m.Id &&
                          !string.Equals((string)x.Element("Estado"), "Cancelada", StringComparison.OrdinalIgnoreCase))
                ?? false;

            if (tieneReservas)
                throw new Exception("No se puede eliminar: tiene reservas asociadas. Deshabilítela (Bloqueada).");

            BDXML.Root.Element("Mesas").Elements("mesa")
                .Where(x => (long)x.Attribute("Id") == m.Id).Remove();
            BDXML.Save(ruta);
            return true;
        }

        public List<BEMesa> ListarTodo()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return BDXML.Root.Element("Mesas").Elements("mesa").Select(Deserializar).ToList();
        }

        public BEMesa ListarObjeto(BEMesa filtro)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            XElement x = null;
            if (filtro.Id > 0)
                x = BDXML.Root.Element("Mesas").Elements("mesa").FirstOrDefault(n => (long)n.Attribute("Id") == filtro.Id);
            return x == null ? null : Deserializar(x);
        }

        public bool TieneReservasAsociadas(long mesaId)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return BDXML.Root.Element("Reservas")?
                .Elements("reserva")
                .Any(x => (long?)x.Element("MesaId") == mesaId &&
                          !string.Equals((string)x.Element("Estado"), "Cancelada", StringComparison.OrdinalIgnoreCase))
                ?? false;
        }

        // Helpers
        private static XElement Serializar(BEMesa m) =>
            new XElement("mesa",
                new XAttribute("Id", m.Id),
                new XElement("Numero", m.Numero),
                new XElement("Capacidad", m.Capacidad),
                new XElement("Sector", m.Sector ?? ""),
                new XElement("Estado", m.Estado ?? BE.EstadosMesa.Libre),
                new XElement("Habilitada", m.Habilitada ? "true" : "false"),
                new XElement("Observaciones", m.Observaciones ?? "")
            );

        private static BEMesa Deserializar(XElement x) =>
            new BEMesa
            {
                Id = (long)x.Attribute("Id"),
                Numero = (int)x.Element("Numero"),
                Capacidad = (int)x.Element("Capacidad"),
                Sector = (string)x.Element("Sector"),
                Estado = (string)x.Element("Estado"),
                Habilitada = (bool?)x.Element("Habilitada") ?? true,
                Observaciones = (string)x.Element("Observaciones")
            };
    }
}
