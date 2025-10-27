using Abstraccion;
using Backup;
using BE;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPRendicion : IGestor<BERendicion>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        public bool CrearXML()
        {
            if (!File.Exists(ruta))
                new XDocument(new XElement("Root", new XElement("Rendiciones"))).Save(ruta);

            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Rendiciones") == null)
            {
                BDXML.Root.Add(new XElement("Rendiciones"));
                BDXML.Save(ruta);
            }
            return true;
        }
        private XElement Root() { return BDXML.Root.Element("Rendiciones"); }

        public long ObtenerUltimoId()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            var ids = Root().Elements("rendicion").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public List<BERendicion> ListarTodo()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return Root().Elements("rendicion").Select(FromXml).OrderBy(r => r.Id).ToList();
        }

        public BERendicion ListarObjeto(BERendicion o)
        {
            if (o == null) return null;
            CrearXML(); BDXML = XDocument.Load(ruta);
            var x = Root().Elements("rendicion").FirstOrDefault(e => (long)e.Attribute("Id") == o.Id);
            return x == null ? null : FromXml(x);
        }

        public bool VerificarExistenciaObjeto(BERendicion o) { return false; }

        public bool Guardar(BERendicion r)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            if (r.Id == 0) r.Id = ObtenerUltimoId() + 1;

            var xExist = Root().Elements("rendicion").FirstOrDefault(e => (long)e.Attribute("Id") == r.Id);
            var xNew = ToXml(r);
            if (xExist == null) Root().Add(xNew); else xExist.ReplaceWith(xNew);

            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BERendicion r)
        {
            if (r == null || r.Id <= 0) return false;
            CrearXML(); BDXML = XDocument.Load(ruta);
            var x = Root().Elements("rendicion").FirstOrDefault(e => (long)e.Attribute("Id") == r.Id);
            if (x == null) return false;
            x.Remove(); BDXML.Save(ruta);
            return true;
        }

        private static BERendicion FromXml(XElement x) => new BERendicion
        {
            Id = (long)x.Attribute("Id"),
            CajaId = (long)x.Element("CajaId"),
            Fecha = System.DateTime.Parse((string)x.Element("Fecha")),
            MozoUsuario = (string)x.Element("MozoUsuario"),
            EspEfectivo = (decimal)x.Element("EspEfectivo"),
            EspTarjeta = (decimal)x.Element("EspTarjeta"),
            EspQR = (decimal)x.Element("EspQR"),
            EntEfectivo = (decimal)x.Element("EntEfectivo"),
            EntTarjeta = (decimal)x.Element("EntTarjeta"),
            EntQR = (decimal)x.Element("EntQR"),
            DiferenciaTotal = (decimal)x.Element("DiferenciaTotal"),
            Estado = (string)x.Element("Estado"),
            Observacion = (string)x.Element("Observacion")
        };

        private static XElement ToXml(BERendicion r) =>
            new XElement("rendicion",
                new XAttribute("Id", r.Id),
                new XElement("CajaId", r.CajaId),
                new XElement("Fecha", r.Fecha.ToString("s")),
                new XElement("MozoUsuario", r.MozoUsuario ?? ""),
                new XElement("EspEfectivo", r.EspEfectivo),
                new XElement("EspTarjeta", r.EspTarjeta),
                new XElement("EspQR", r.EspQR),
                new XElement("EntEfectivo", r.EntEfectivo),
                new XElement("EntTarjeta", r.EntTarjeta),
                new XElement("EntQR", r.EntQR),
                new XElement("DiferenciaTotal", r.DiferenciaTotal),
                new XElement("Estado", r.Estado ?? "OK"),
                new XElement("Observacion", r.Observacion ?? "")
            );
    }
}
