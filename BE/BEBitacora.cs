using System;

namespace BE
{
    public class BEBitacora : BEEntidad
    {
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }  // "admin", "maria", etc.
        public string Accion { get; set; }   // LOGIN_OK, LOGIN_FAIL, etc.
        public string Detalle { get; set; }  // texto libre
    }
}
