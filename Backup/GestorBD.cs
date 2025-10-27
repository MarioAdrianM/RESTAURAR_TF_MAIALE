// Backup/GestorBD.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Backup
{
    public class GestorBD
    {
        // Crea backup plano de BD\BD.xml en la carpeta \Backup (se crea si no existe)
        public void CrearBackUp()
        {
            try
            {
                // Aseguro carpetas
                string rutaBD = GestorCarpeta.UbicacionBD("BD.xml");
                string carpetaBackup = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup");
                Directory.CreateDirectory(carpetaBackup);

                // Nombre y ruta destino
                string nombreBackup = $"BD_Backup_{DateTime.Now:dd-MM-yyyy HH-mm-ss}.xml";
                string rutaBackup = Path.Combine(carpetaBackup, nombreBackup);

                // Verificaciones
                if (!File.Exists(rutaBD))
                    throw new Exception("Error: No se encontró BD\\BD.xml.");

                if (File.Exists(rutaBackup))
                    throw new Exception("Error: El backup que intenta guardar ya existe.");

                // Copia
                File.Copy(rutaBD, rutaBackup);
            }
            catch (XmlException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }

        // Restaura BD\BD.xml a partir de un archivo existente en \Backup
        public void CrearRestore(string nombreBackup)
        {
            try
            {
                string rutaBD = GestorCarpeta.UbicacionBD("BD.xml");
                string carpetaBackup = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup");
                Directory.CreateDirectory(carpetaBackup);

                string rutaBackup = Path.Combine(carpetaBackup, nombreBackup);

                if (!File.Exists(rutaBackup))
                    throw new Exception("Error: No se encontró el archivo de backup seleccionado en \\Backup.");

                // Sobrescribe BD.xml con el backup elegido
                File.Copy(rutaBackup, rutaBD, true);
            }
            catch (XmlException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }

        // Lista de backups disponibles en \Backup (solo .xml)
        public List<string> ListarBackups()
        {
            try
            {
                string carpetaBackup = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup");
                Directory.CreateDirectory(carpetaBackup);

                return Directory.GetFiles(carpetaBackup, "*.xml", SearchOption.TopDirectoryOnly)
                                .Select(Path.GetFileName)
                                .OrderByDescending(n => n) // los más nuevos arriba
                                .ToList();
            }
            catch (XmlException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }
    }
}
