using Abstraccion;
using Backup;
using BE;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPCaja : IGestor<BECaja>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        public bool CrearXML()
        {
            if (!File.Exists(ruta))
                new XDocument(new XElement("Root", new XElement("Cajas"))).Save(ruta);

            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Cajas") == null)
            {
                BDXML.Root.Add(new XElement("Cajas"));
                BDXML.Save(ruta);
            }
            return true;
        }
        private XElement Root() { return BDXML.Root.Element("Cajas"); }

        public long ObtenerUltimoId()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            var ids = Root().Elements("caja").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public List<BECaja> ListarTodo()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return Root().Elements("caja").Select(FromXml).OrderBy(c => c.Id).ToList();
        }

        public BECaja ListarObjeto(BECaja o)
        {
            if (o == null) return null;
            CrearXML(); BDXML = XDocument.Load(ruta);
            var x = Root().Elements("caja").FirstOrDefault(e => (long)e.Attribute("Id") == o.Id);
            return x == null ? null : FromXml(x);
        }

        public bool VerificarExistenciaObjeto(BECaja o) { return false; }

        public bool Guardar(BECaja c)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            if (c.Id == 0) c.Id = ObtenerUltimoId() + 1;

            var xExist = Root().Elements("caja").FirstOrDefault(e => (long)e.Attribute("Id") == c.Id);
            var xNew = ToXml(c);
            if (xExist == null) Root().Add(xNew); else xExist.ReplaceWith(xNew);

            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BECaja c)
        {
            if (c == null || c.Id <= 0) return false;
            CrearXML(); BDXML = XDocument.Load(ruta);
            var x = Root().Elements("caja").FirstOrDefault(e => (long)e.Attribute("Id") == c.Id);
            if (x == null) return false;
            x.Remove(); BDXML.Save(ruta);
            return true;
        }

        private static BECaja FromXml(XElement x) => new BECaja
        {
            Id = (long)x.Attribute("Id"),
            
            FechaApertura = System.DateTime.Parse((string)x.Element("FechaApertura")),
            Punto = (string)x.Element("Punto"),
            Turno = (string)x.Element("Turno"),
            Responsable = (string)x.Element("Responsable"),
            FondoInicial = (decimal)x.Element("FondoInicial"),
            UmbralDiferencia = (decimal)x.Element("UmbralDiferencia"),
            Abierta = (bool)x.Element("Abierta")
        };

        private static XElement ToXml(BECaja c) =>
            new XElement("caja",
                new XAttribute("Id", c.Id),
                new XElement("FechaApertura", c.FechaApertura.ToString("s")),
                new XElement("Punto", c.Punto ?? "Caja1"),
                
                new XElement("Turno", c.Turno ?? ""),
                new XElement("Responsable", c.Responsable ?? ""),
                new XElement("FondoInicial", c.FondoInicial),
                new XElement("UmbralDiferencia", c.UmbralDiferencia),
                new XElement("Abierta", c.Abierta)
            );
    }
}
