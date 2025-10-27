using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraccion
{
    public interface IGestor<T> where T : IEntidad
    {
        #region "Métodos"
        bool Guardar(T objeto);
        bool Eliminar(T objeto);
        bool VerificarExistenciaObjeto(T objeto);
        List<T> ListarTodo();
        T ListarObjeto(T objeto);
        bool CrearXML();
        long ObtenerUltimoId();
        #endregion
    }
}
