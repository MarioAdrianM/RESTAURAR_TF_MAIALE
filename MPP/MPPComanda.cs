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
    public class MPPComanda : IGestor<BEComanda>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        private void CargarBD()
        {
            if (!File.Exists(ruta))
                new XDocument(new XElement("Root")).Save(ruta);

            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Comandas") == null) BDXML.Root.Add(new XElement("Comandas"));
            if (BDXML.Root.Element("Productos") == null) BDXML.Root.Add(new XElement("Productos")); // para consistencia
        }
        private void GuardarBD() { BDXML.Save(ruta); }
        private XElement RootComandas() { return BDXML.Root.Element("Comandas"); }

        // ------ IGestor (ABM de la cabecera de comanda) ------
        public bool CrearXML() { CargarBD(); GuardarBD(); return true; }

        public bool Guardar(BEComanda c)
        {
            CargarBD();
            if (c.Id == 0)
            {
                c.Id = ObtenerUltimoId() + 1;
                var xe = Serializar(c);
                RootComandas().Add(xe);
            }
            else
            {
                var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == c.Id);
                if (xe == null) throw new Exception("Comanda no encontrada.");
                xe.ReplaceWith(Serializar(c));
            }
            GuardarBD();
            return true;
        }

        public bool Eliminar(BEComanda c)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == c.Id);
            if (xe != null) { xe.Remove(); GuardarBD(); return true; }
            return false;
        }

        public BEComanda ListarObjeto(BEComanda c)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == c.Id);
            return xe == null ? null : Deserializar(xe);
        }

        public List<BEComanda> ListarTodo()
        {
            CargarBD();
            return RootComandas().Elements("Comanda").Select(Deserializar)
                .OrderByDescending(c => c.FechaApertura)
                .ToList();
        }

        public long ObtenerUltimoId()
        {
            CargarBD();
            return RootComandas().Elements("Comanda")
                .Select(x => (long?)x.Attribute("Id") ?? 0)
                .DefaultIfEmpty(0).Max();
        }

        public bool VerificarExistenciaObjeto(BEComanda c)
        {
            if (c == null) return false;
            CargarBD();

            if (c.Id > 0)
                return RootComandas().Elements("Comanda").Any(x => ((long?)x.Attribute("Id") ?? 0) == c.Id);

            // clave natural (abierta por mesa): MesaId + Estado Abierta
            if (c.MesaId > 0)
            {
                return RootComandas().Elements("Comanda").Any(x =>
                    ((long?)x.Attribute("MesaId") ?? 0) == c.MesaId &&
                    ((string)x.Attribute("Estado") ?? "") == EstadosComanda.Abierta);
            }
            return false;
        }

        // ------ Operaciones específicas de dominio ------
        public BEComanda ObtenerAbiertaPorMesa(long mesaId)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda")
                .FirstOrDefault(x =>
                    ((long?)x.Attribute("MesaId") ?? 0) == mesaId &&
                    ((string)x.Attribute("Estado") ?? "") == EstadosComanda.Abierta);
            return xe == null ? null : Deserializar(xe);
        }

        public BEComanda Abrir(long mesaId, string mozoUsuario)
        {
            CargarBD();
            var abierta = ObtenerAbiertaPorMesa(mesaId);
            if (abierta != null) return abierta;

            var c = new BEComanda
            {
                Id = ObtenerUltimoId() + 1,
                MesaId = mesaId,
                MozoUsuario = mozoUsuario ?? "",
                FechaApertura = DateTime.Now,
                Estado = EstadosComanda.Abierta,
                FacturaSolicitada = false,
                Total = 0m
            };
            RootComandas().Add(Serializar(c));
            GuardarBD();
            return c;
        }

        public BEComanda AgregarItem(long comandaId, BEItemComanda item)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == comandaId);
            if (xe == null) throw new Exception("Comanda no encontrada.");

            var xItems = xe.Element("Items");
            if (xItems == null) { xItems = new XElement("Items"); xe.Add(xItems); }

            long nextItemId = xItems.Elements("Item").Select(x => (long?)x.Attribute("ItemId") ?? 0).DefaultIfEmpty(0).Max() + 1;
            item.ItemId = nextItemId;

            xItems.Add(SerializarItem(item));

            // recalcular total y persistir
            RecalcularTotal(xe);
            GuardarBD();
            return Deserializar(xe);
        }

        public BEComanda ModificarCantidadItem(long comandaId, long itemId, decimal nuevaCantidad)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == comandaId);
            if (xe == null) throw new Exception("Comanda no encontrada.");

            var xi = xe.Element("Items")?.Elements("Item").FirstOrDefault(x => ((long?)x.Attribute("ItemId") ?? 0) == itemId);
            if (xi == null) throw new Exception("Item no encontrado.");

            xi.SetAttributeValue("Cantidad", nuevaCantidad);

            RecalcularTotal(xe);
            GuardarBD();
            return Deserializar(xe);
        }

        public BEComanda CambiarEstadoItem(long comandaId, long itemId, string nuevoEstado, string motivo = null)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == comandaId);
            if (xe == null) throw new Exception("Comanda no encontrada.");

            var xi = xe.Element("Items")?.Elements("Item").FirstOrDefault(x => ((long?)x.Attribute("ItemId") ?? 0) == itemId);
            if (xi == null) throw new Exception("Item no encontrado.");

            xi.SetAttributeValue("Estado", nuevoEstado ?? EstadosItemComanda.Pendiente);
            if (!string.IsNullOrWhiteSpace(motivo))
                xi.SetAttributeValue("MotivoAnulacion", motivo);

            RecalcularTotal(xe);
            GuardarBD();
            return Deserializar(xe);
        }

        public BEComanda Cerrar(long comandaId, bool solicitarFactura)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == comandaId);
            if (xe == null) throw new Exception("Comanda no encontrada.");

            xe.SetAttributeValue("Estado", EstadosComanda.Cerrada);
            xe.SetAttributeValue("FacturaSolicitada", solicitarFactura);
            xe.SetAttributeValue("FechaCierre", DateTime.Now);

            GuardarBD();
            return Deserializar(xe);
        }

        public BEComanda MarcarFacturada(long comandaId)
        {
            CargarBD();
            var xe = RootComandas().Elements("Comanda").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == comandaId);
            if (xe == null) throw new Exception("Comanda no encontrada.");

            xe.SetAttributeValue("Estado", EstadosComanda.Facturada);
            xe.SetAttributeValue("FechaCierre", DateTime.Now);
            GuardarBD();
            return Deserializar(xe);
        }

        // ------ Serialización ------
        private XElement Serializar(BEComanda c)
        {
            var xe = new XElement("Comanda",
                new XAttribute("Id", c.Id),
                new XAttribute("MesaId", c.MesaId),
                new XAttribute("MozoUsuario", c.MozoUsuario ?? ""),
                new XAttribute("FechaApertura", c.FechaApertura.ToString("o")),
                new XAttribute("Estado", c.Estado ?? EstadosComanda.Abierta),
                new XAttribute("FacturaSolicitada", c.FacturaSolicitada),
                new XAttribute("Total", c.Total)
            );
            if (c.FechaCierre.HasValue) xe.Add(new XAttribute("FechaCierre", c.FechaCierre.Value.ToString("o")));

            var xItems = new XElement("Items");
            foreach (var it in c.Items)
                xItems.Add(SerializarItem(it));
            xe.Add(xItems);

            return xe;
        }

        private XElement SerializarItem(BEItemComanda i)
        {
            var xi = new XElement("Item",
                new XAttribute("ItemId", i.ItemId),
                new XAttribute("ProductoId", i.ProductoId),
                new XAttribute("Descripcion", i.Descripcion ?? ""),
                new XAttribute("PrecioUnitario", i.PrecioUnitario),
                new XAttribute("Cantidad", i.Cantidad),
                new XAttribute("Estado", i.Estado ?? EstadosItemComanda.Pendiente)
            );
            if (!string.IsNullOrWhiteSpace(i.MotivoAnulacion))
                xi.Add(new XAttribute("MotivoAnulacion", i.MotivoAnulacion));
            return xi;
        }

        private BEComanda Deserializar(XElement xe)
        {
            var c = new BEComanda
            {
                Id = (long)xe.Attribute("Id"),
                MesaId = (long)xe.Attribute("MesaId"),
                MozoUsuario = (string)xe.Attribute("MozoUsuario") ?? "",
                FechaApertura = DateTime.Parse((string)xe.Attribute("FechaApertura")),
                Estado = (string)xe.Attribute("Estado") ?? EstadosComanda.Abierta,
                FacturaSolicitada = (bool?)xe.Attribute("FacturaSolicitada") ?? false,
                Total = (decimal)xe.Attribute("Total")
            };
            var attCierre = xe.Attribute("FechaCierre");
            if (attCierre != null) c.FechaCierre = DateTime.Parse((string)attCierre);

            var items = new List<BEItemComanda>();
            var xItems = xe.Element("Items");
            if (xItems != null)
            {
                foreach (var xi in xItems.Elements("Item"))
                {
                    var it = new BEItemComanda
                    {
                        ItemId = (long)xi.Attribute("ItemId"),
                        ProductoId = (long)xi.Attribute("ProductoId"),
                        Descripcion = (string)xi.Attribute("Descripcion") ?? "",
                        PrecioUnitario = (decimal)xi.Attribute("PrecioUnitario"),
                        Cantidad = (decimal)xi.Attribute("Cantidad"),
                        Estado = (string)xi.Attribute("Estado") ?? EstadosItemComanda.Pendiente
                    };
                    var attMotivo = xi.Attribute("MotivoAnulacion");
                    if (attMotivo != null) it.MotivoAnulacion = (string)attMotivo;
                    items.Add(it);
                }
            }
            c.Items = items;
            return c;
        }

        private void RecalcularTotal(XElement xeComanda)
        {
            decimal total = 0m;
            var xItems = xeComanda.Element("Items");
            if (xItems != null)
            {
                foreach (var xi in xItems.Elements("Item"))
                {
                    var estado = (string)xi.Attribute("Estado") ?? EstadosItemComanda.Pendiente;
                    if (estado == EstadosItemComanda.Anulado) continue;

                    decimal p = (decimal)xi.Attribute("PrecioUnitario");
                    decimal q = (decimal)xi.Attribute("Cantidad");
                    total += Math.Round(p * q, 2);
                }
            }
            xeComanda.SetAttributeValue("Total", total);
        }
    }
}
