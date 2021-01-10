using Cassandra;
using System;

using System.Windows.Forms;


namespace GUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main()
        {


            //Zamieniæ to na mysql -- bardzo analogicznie

            Cluster cluster = Cluster.Builder()
                .AddContactPoint("192.168.100.99")
                .Build();
            ISession session = cluster.Connect("test_keyspace");


            session.UserDefinedTypes.Define(
                UdtMap.For<Shot>()
                      .Map(a => a.id, "id")
                      .Map(a => a.name, "name")
                      .Map(a => a.accessible, "accessible")
                      .Map(a => a.refund, "refund")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Patient>()
                      .Map(a => a.pesel, "pesel")
                      .Map(a => a.first_name, "first_name")
                      .Map(a => a.last_name, "last_name")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Nurse>()
                      .Map(a => a.pesel, "pesel")
                      .Map(a => a.first_name, "first_name")
                      .Map(a => a.last_name, "last_name")
                      .Map(a => a.login, "login")
                      .Map(a => a.password, "password")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Doctor>()
                      .Map(a => a.pesel, "pesel")
                      .Map(a => a.first_name, "first_name")
                      .Map(a => a.last_name, "last_name")
                      .Map(a => a.login, "login")
                      .Map(a => a.password, "password")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Ilness>()
                      .Map(a => a.id, "id")
                      .Map(a => a.name, "name")
                );
            //Zamieniæ to na mysql -- bardzo analogicznie






            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
