using Abstraccion;
using Backup;
using BE;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPPago : IGestor<BEPago>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        public bool CrearXML()
        {
            if (!File.Exists(ruta))
            {
                BDXML = new XDocument(new XElement("Root",
                    new XElement("Pagos")
                ));
                BDXML.Save(ruta);
                return true;
            }
            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Pagos") == null)
            {
                BDXML.Root.Add(new XElement("Pagos"));
                BDXML.Save(ruta);
            }
            return true;
        }

        private XElement Root() { return BDXML.Root.Element("Pagos"); }

        public long ObtenerUltimoId()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            var ids = Root().Elements("pago").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public List<BEPago> ListarTodo()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return Root().Elements("pago").Select(FromXml).OrderBy(p => p.Id).ToList();
        }

        public BEPago ListarObjeto(BEPago o) // por Id
        {
            if (o == null) return null;
            CrearXML(); BDXML = XDocument.Load(ruta);
            var x = Root().Elements("pago").FirstOrDefault(e => (long)e.Attribute("Id") == o.Id);
            return x == null ? null : FromXml(x);
        }

        public bool VerificarExistenciaObjeto(BEPago o) { return false; } // no usamos

        public bool Guardar(BEPago p)
        {
            if (p.Id == 0) p.Id = ObtenerUltimoId() + 1;
            CrearXML(); BDXML = XDocument.Load(ruta);

            var xExist = Root().Elements("pago").FirstOrDefault(e => (long)e.Attribute("Id") == p.Id);
            var xNew = ToXml(p);
            if (xExist == null) Root().Add(xNew);
            else xExist.ReplaceWith(xNew);

            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BEPago p)
        {
            if (p == null || p.Id <= 0) return false;
            CrearXML(); BDXML = XDocument.Load(ruta);
            var x = Root().Elements("pago").FirstOrDefault(e => (long)e.Attribute("Id") == p.Id);
            if (x == null) return false;
            x.Remove(); BDXML.Save(ruta);
            return true;
        }

        private static BEPago FromXml(XElement x) => new BEPago
        {
            Id = (long)x.Attribute("Id"),
            Fecha = System.DateTime.Parse((string)x.Element("Fecha")),
            FacturaId = (long)x.Element("FacturaId"),
            MesaId = (long)x.Element("MesaId"),
            Medio = (string)x.Element("Medio"),
            Monto = (decimal)x.Element("Monto"),
            Autorizacion = (string)x.Element("Autorizacion"),
            Vuelto = (decimal)x.Element("Vuelto"),
            Estado = (string)x.Element("Estado"),
            Cobrador = (string)x.Element("Cobrador") ?? ""

        };

        private static XElement ToXml(BEPago p) =>
            new XElement("pago",
                new XAttribute("Id", p.Id),
                new XElement("Fecha", p.Fecha.ToString("s")),
                new XElement("FacturaId", p.FacturaId),
                new XElement("MesaId", p.MesaId),
                new XElement("Medio", p.Medio ?? ""),
                new XElement("Monto", p.Monto),
                new XElement("Autorizacion", p.Autorizacion ?? ""),
                new XElement("Vuelto", p.Vuelto),
                new XElement("Estado", p.Estado ?? "EnCustodiaMozo"),
                new XElement("Cobrador", p.Cobrador ?? "")

            );
    }

}
