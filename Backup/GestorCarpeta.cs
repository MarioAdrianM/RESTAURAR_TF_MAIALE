using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Backup
{
    public static class GestorCarpeta
    {
        #region "Métodos"
        public static string UbicacionBD(string nombreArchivo)
        {
            try
            {
                string carpeta = "BD";
                CrearCarpeta(carpeta);
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, carpeta, nombreArchivo);
            }
            catch (Exception ex) { throw ex; }
        }

        public static string UbicacionInformes(string nombreArchivo)
        {
            try
            {
                string carpeta = "Informes";
                CrearCarpeta(carpeta);
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, carpeta, nombreArchivo);
            }
            catch (Exception ex) { throw ex; }
        }

        public static string UbicacionPDFs(string nombreArchivo)
        {
            try
            {
                string carpeta = "PDFs";
                CrearCarpeta(carpeta);
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, carpeta, nombreArchivo);
            }
            catch (Exception ex) { throw ex; }
        }

        private static void CrearCarpeta(string nombre)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombre);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        #endregion
    }
}

