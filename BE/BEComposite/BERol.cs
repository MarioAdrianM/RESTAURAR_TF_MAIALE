using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.BEComposite
{
    public class BERol : BEComposite
    {
        public List<BEComposite> listaPermisos { get; private set; }

        public BERol(long pId, string pNombre) : base(pId, pNombre)
        {
            listaPermisos = new List<BEComposite>();
        }

        public override void Agregar(BEComposite oBEComposite)
        {
            try { listaPermisos.Add(oBEComposite); }
            catch (Exception ex) { throw ex; }
        }

        public override IList<BEComposite> ObtenerHijos()
        {
            try { return listaPermisos; }
            catch (Exception ex) { throw ex; }
        }
    }
}

