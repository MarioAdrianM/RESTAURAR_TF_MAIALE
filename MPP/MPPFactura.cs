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
    public class MPPFactura : IGestor<BEFactura>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        #region "Infra"
        public bool CrearXML()
        {
            try
            {
                if (!File.Exists(ruta))
                {
                    // Root mínimo. Otros MPPs agregarán sus nodos si faltan.
                    BDXML = new XDocument(new XElement("Root", new XElement("Facturas")));
                    BDXML.Save(ruta);
                    return true;
                }
                else
                {
                    BDXML = XDocument.Load(ruta);
                    AseguroRoot("Facturas");
                    BDXML.Save(ruta);
                    return true;
                }
            }
            catch { return false; }
        }
        private void AseguroRoot(string name)
        {
            if (BDXML.Root.Element(name) == null)
                BDXML.Root.Add(new XElement(name));
        }
        private XElement RootFacturas() => BDXML.Root.Element("Facturas");
        #endregion

        #region "IGestor"
        public long ObtenerUltimoId()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            var ids = RootFacturas().Elements("factura").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public List<BEFactura> ListarTodo()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return RootFacturas().Elements("factura")
                .Select(FromXml).OrderBy(f => f.Id).ToList();
        }

        public BEFactura ListarObjeto(BEFactura f)
        {
            if (f == null) return null;
            CrearXML(); BDXML = XDocument.Load(ruta);
            var x = RootFacturas().Elements("factura")
                .FirstOrDefault(e => (long)e.Attribute("Id") == f.Id);
            return x == null ? null : FromXml(x);
        }

        public bool VerificarExistenciaObjeto(BEFactura f)
        {
            // Evitamos duplicados de (PuntoVenta, TipoCbte, Numero) para distinto Id
            CrearXML(); BDXML = XDocument.Load(ruta);
            return RootFacturas().Elements("factura")
                .Any(x =>
                    (int)x.Element("PuntoVenta") == f.PuntoVenta &&
                    string.Equals((string)x.Element("TipoCbte"), f.TipoCbte, StringComparison.OrdinalIgnoreCase) &&
                    (long)x.Element("Numero") == f.Numero &&
                    (long)x.Attribute("Id") != f.Id);
        }

        public bool Guardar(BEFactura f)
        {
            if (f == null) return false;
            CrearXML(); BDXML = XDocument.Load(ruta);

            if (f.Id == 0) f.Id = ObtenerUltimoId() + 1;

            // Si no viene Numero, genero correlativo por PV+Tipo
            if (f.Numero == 0)
                f.Numero = SiguienteNumero(f.PuntoVenta, f.TipoCbte);

            // Upsert
            var exist = RootFacturas().Elements("factura")
                .FirstOrDefault(x => (long)x.Attribute("Id") == f.Id);

            var nodo = ToXml(f);
            if (exist == null)
                RootFacturas().Add(nodo);
            else
                exist.ReplaceWith(nodo);

            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BEFactura f)
        {
            if (f == null || f.Id <= 0) return false;
            CrearXML(); BDXML = XDocument.Load(ruta);

            var x = RootFacturas().Elements("factura")
                .FirstOrDefault(e => (long)e.Attribute("Id") == f.Id);
            if (x == null) return false;

            // Borrado físico (para mantener el patrón de otros MPPs).
            x.Remove();
            BDXML.Save(ruta);
            return true;
        }
        #endregion

        #region "Helpers"
        private long SiguienteNumero(int pv, string tipo)
        {
            var max = RootFacturas().Elements("factura")
                .Where(x =>
                    (int)x.Element("PuntoVenta") == pv &&
                    string.Equals((string)x.Element("TipoCbte"), tipo ?? "B", StringComparison.OrdinalIgnoreCase))
                .Select(x => (long)x.Element("Numero"))
                .DefaultIfEmpty(0)
                .Max();
            return max + 1;
        }

        private static BEFactura FromXml(XElement x) => new BEFactura
        {
            Id = (long)x.Attribute("Id"),
            Fecha = DateTime.Parse((string)x.Element("Fecha")),
            MesaId = (long)x.Element("MesaId"),
            PuntoVenta = (int)x.Element("PuntoVenta"),
            TipoCbte = (string)x.Element("TipoCbte"),
            Numero = (long)x.Element("Numero"),
            ImporteTotal = (decimal)x.Element("ImporteTotal"),
            Estado = (string)x.Element("Estado")
        };

        private static XElement ToXml(BEFactura f) =>
            new XElement("factura",
                new XAttribute("Id", f.Id),
                new XElement("Fecha", f.Fecha.ToString("s")),
                new XElement("MesaId", f.MesaId),
                new XElement("PuntoVenta", f.PuntoVenta),
                new XElement("TipoCbte", f.TipoCbte ?? "B"),
                new XElement("Numero", f.Numero),
                new XElement("ImporteTotal", f.ImporteTotal),
                new XElement("Estado", f.Estado ?? "Emitida")
            );
        #endregion
    }
}
