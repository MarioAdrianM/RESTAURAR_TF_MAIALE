using Backup;
using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPProveedorFamilia
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        private void CargarBD()
        {
            if (!File.Exists(ruta))
                new XDocument(new XElement("Root")).Save(ruta);

            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Proveedor_Familias") == null) BDXML.Root.Add(new XElement("Proveedor_Familias"));
            if (BDXML.Root.Element("Familias") == null) BDXML.Root.Add(new XElement("Familias"));
            if (BDXML.Root.Element("Proveedores") == null) BDXML.Root.Add(new XElement("Proveedores"));
        }
        private void GuardarBD() { BDXML.Save(ruta); }
        private XElement RootPF() { return BDXML.Root.Element("Proveedor_Familias"); }

        public void Vincular(long proveedorId, long familiaId)
        {
            CargarBD();
            var dup = RootPF().Elements("ProveedorFamilia")
                .FirstOrDefault(x => ((long?)x.Attribute("ProveedorId") ?? 0) == proveedorId
                                  && ((long?)x.Attribute("FamiliaId") ?? 0) == familiaId);
            if (dup == null)
            {
                RootPF().Add(new XElement("ProveedorFamilia",
                    new XAttribute("ProveedorId", proveedorId),
                    new XAttribute("FamiliaId", familiaId)));
                GuardarBD();
            }
        }

        public void Desvincular(long proveedorId, long familiaId)
        {
            CargarBD();
            var xe = RootPF().Elements("ProveedorFamilia")
                .FirstOrDefault(x => ((long?)x.Attribute("ProveedorId") ?? 0) == proveedorId
                                  && ((long?)x.Attribute("FamiliaId") ?? 0) == familiaId);
            if (xe != null) { xe.Remove(); GuardarBD(); }
        }

        public List<long> FamiliasIdsDeProveedor(long proveedorId)
        {
            CargarBD();
            return RootPF().Elements("ProveedorFamilia")
                .Where(x => ((long?)x.Attribute("ProveedorId") ?? 0) == proveedorId)
                .Select(x => (long)x.Attribute("FamiliaId"))
                .Distinct().ToList();
        }

        public List<long> ProveedoresIdsDeFamilia(long familiaId)
        {
            CargarBD();
            return RootPF().Elements("ProveedorFamilia")
                .Where(x => ((long?)x.Attribute("FamiliaId") ?? 0) == familiaId)
                .Select(x => (long)x.Attribute("ProveedorId"))
                .Distinct().ToList();
        }
    }
}
