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

            Cluster cluster = Cluster.Builder()
                .AddContactPoint("192.168.100.99")
                .Build();
            ISession session = cluster.Connect("test_keyspace");
            session.UserDefinedTypes.Define(
                UdtMap.For<Shot>()
                      .Map(a => a.date, "date")
                      .Map(a => a.name, "name")
                      .Map(a => a.obligatory, "obligatory")
                      .Map(a => a.done, "done")
                );

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(session));
        }
    }
}
