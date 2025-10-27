using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Seguridad
{
    public static class Encriptacion
    {
        #region "Métodos"
        public static string EncriptarPassword(string pPassword)
        {
            try
            {
                byte[] encriptar = Encoding.Unicode.GetBytes(pPassword);
                return Convert.ToBase64String(encriptar);
            }
            catch (CryptographicException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }

        public static string DesencriptarPassword(string pPasswordEncriptado)
        {
            try
            {
                byte[] desencriptar = Convert.FromBase64String(pPasswordEncriptado);
                return Encoding.Unicode.GetString(desencriptar);
            }
            catch (CryptographicException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }
        public static bool CompararPlanoConCifrado(string pPassword, string pPasswordEncriptado)
        {
            return EncriptarPassword(pPassword ?? string.Empty) == (pPasswordEncriptado ?? string.Empty);
        }
        #endregion
    }
}
