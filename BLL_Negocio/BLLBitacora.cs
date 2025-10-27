using BE;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL_Negocio
{
    public class BLLBitacora
    {
        private readonly MPPBitacora mpp = new MPPBitacora();

        public void Registrar(string usuario, string accion, string detalle = null)
        {
            mpp.Guardar(new BEBitacora
            {
                Fecha = DateTime.Now,
                Usuario = usuario ?? "",
                Accion = accion ?? "",
                Detalle = detalle ?? ""
            });
        }

        public List<BEBitacora> Listar() => mpp.ListarTodo();
    }
}
