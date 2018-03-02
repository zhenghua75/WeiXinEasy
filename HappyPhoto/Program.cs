using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HappyPhoto
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmCreatPrintCode());
            //Application.Run(new frmGetPrintPhoto());
            //Application.Run(new frmPrintCodeService());
            //Application.Run(new frmModelTest());
            Application.Run(new frmVYIGOPrint());
            
        }
    }
}
