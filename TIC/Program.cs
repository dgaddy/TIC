using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TIC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TICForm());

            NotifyIcon ni = new NotifyIcon();
            ni.Visible = true;

            Client c = new Client();

            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();

                TICForm form = new TICForm();
                Application.Run();
                c.SendData();
            }
        }
    }
}
