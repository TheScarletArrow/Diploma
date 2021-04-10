using System;
using System.Windows.Forms;

namespace Diploma
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AdminControlPanel());
              //Application.Run(new Auth());
              //Application.Run(new ShowTables());
        }

    }
}
