using System;
using System.Collections;
using Cassandra;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace GUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
