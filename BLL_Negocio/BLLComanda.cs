using Abstraccion;
using BE;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_Negocio
{
    public class BLLComanda : IGestor<BEComanda>
    {
        private readonly MPPComanda mpp = new MPPComanda();
        private readonly BLLProducto bllProducto = new BLLProducto();

        // ----- IGestor (cabecera) -----
        public bool CrearXML() { return mpp.CrearXML(); }
        public bool Guardar(BEComanda o) { return mpp.Guardar(o); }
        public bool Eliminar(BEComanda o) { return mpp.Eliminar(o); }
        public BEComanda ListarObjeto(BEComanda o) { return mpp.ListarObjeto(o); }
        public List<BEComanda> ListarTodo() { return mpp.ListarTodo(); }
        public long ObtenerUltimoId() { return mpp.ObtenerUltimoId(); }
        public bool VerificarExistenciaObjeto(BEComanda o) { return mpp.VerificarExistenciaObjeto(o); }

        // ----- Reglas de negocio -----
        public BEComanda Abrir(long mesaId, string mozoUsuario)
        {
            if (mesaId <= 0) throw new Exception("Mesa inválida.");
            if (string.IsNullOrWhiteSpace(mozoUsuario)) mozoUsuario = "";

            var abierta = mpp.ObtenerAbiertaPorMesa(mesaId);
            if (abierta != null) return abierta;

            return mpp.Abrir(mesaId, mozoUsuario);
        }
        // Devuelve la última comanda CERRADA con factura solicitada para esa mesa
        public BEComanda ObtenerParaFacturarPorMesa(long mesaId)
        {
            var todas = ListarTodo();
            return todas
                .Where(c => c.MesaId == mesaId
                         && c.Estado == EstadosComanda.Cerrada
                         && c.FacturaSolicitada)
                .OrderByDescending(c => c.FechaApertura)
                .FirstOrDefault();
        }

        public BEComanda ObtenerAbiertaPorMesa(long mesaId)
        {
            return mpp.ObtenerAbiertaPorMesa(mesaId);
        }

        public BEComanda AgregarItem(long comandaId, long productoId, decimal cantidad)
        {
            if (cantidad <= 0) throw new Exception("Cantidad debe ser mayor a cero.");

            var prod = bllProducto.ListarObjeto(new BEProducto { Id = productoId });
            if (prod == null || !prod.Activo) throw new Exception("Producto no disponible.");

            // snapshot
            var item = new BEItemComanda
            {
                ProductoId = prod.Id,
                Descripcion = prod.Nombre,
                PrecioUnitario = prod.PrecioVenta,
                Cantidad = cantidad,
                Estado = EstadosItemComanda.Pendiente
            };

            var comanda = ListarObjeto(new BEComanda { Id = comandaId });
            if (comanda == null) throw new Exception("Comanda no encontrada.");
            if (comanda.Estado != EstadosComanda.Abierta) throw new Exception("La comanda no está abierta.");

            return mpp.AgregarItem(comandaId, item);
        }

        public BEComanda ModificarCantidadItem(long comandaId, long itemId, decimal nuevaCantidad)
        {
            if (nuevaCantidad <= 0) throw new Exception("Cantidad debe ser mayor a cero.");

            var comanda = ListarObjeto(new BEComanda { Id = comandaId });
            if (comanda == null) throw new Exception("Comanda no encontrada.");
            if (comanda.Estado != EstadosComanda.Abierta) throw new Exception("La comanda no está abierta.");

            return mpp.ModificarCantidadItem(comandaId, itemId, nuevaCantidad);
        }

        public BEComanda AnularItem(long comandaId, long itemId, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo)) motivo = "Anulación";

            var comanda = ListarObjeto(new BEComanda { Id = comandaId });
            if (comanda == null) throw new Exception("Comanda no encontrada.");
            // permitimos anular en abierta o cerrada antes de facturar (según política)
            if (comanda.Estado == EstadosComanda.Facturada) throw new Exception("La comanda ya fue facturada.");

            return mpp.CambiarEstadoItem(comandaId, itemId, EstadosItemComanda.Anulado, motivo);
        }

        public BEComanda CambiarEstadoItemCocina(long comandaId, long itemId, string nuevoEstado)
        {
            if (nuevoEstado != EstadosItemComanda.Pendiente &&
                nuevoEstado != EstadosItemComanda.EnPrep &&
                nuevoEstado != EstadosItemComanda.Listo)
                throw new Exception("Estado de cocina inválido.");

            var comanda = ListarObjeto(new BEComanda { Id = comandaId });
            if (comanda == null) throw new Exception("Comanda no encontrada.");
            if (comanda.Estado == EstadosComanda.Facturada) throw new Exception("La comanda ya fue facturada.");

            return mpp.CambiarEstadoItem(comandaId, itemId, nuevoEstado, null);
        }

        public BEComanda PedirFactura(long comandaId)
        {
            var c = ListarObjeto(new BEComanda { Id = comandaId });
            if (c == null) throw new Exception("Comanda no encontrada.");
            if (c.Estado != EstadosComanda.Abierta) throw new Exception("La comanda no está abierta.");
            if (c.Items == null || c.Items.Count == 0 || c.Total <= 0)
                throw new Exception("No se puede pedir factura con comanda vacía.");

            // Cerramos y marcamos solicitud a Caja
            return mpp.Cerrar(comandaId, true);
        }

        public BEComanda MarcarFacturada(long comandaId)
        {
            // Lo llamará Caja al emitir factura (antes de crear la BEFactura, si querés)
            return mpp.MarcarFacturada(comandaId);
        }
    }
}
