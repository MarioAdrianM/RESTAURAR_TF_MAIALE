using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Seguridad;
namespace Restaurar_v1._0
{
    internal static class Program
    {
    
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Bootstrap.SembrarAdmin();
            
            Application.Run(new FormLogin());
        }
    }
}
