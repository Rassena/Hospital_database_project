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
                      .Map(a => a.id, "Chorobaid")
                      .Map(a => a.name, "nazwa")
                      .Map(a => a.accessible, "dostêpna")
                      .Map(a => a.refund, "refundacjaid")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Vaccination>()
                      .Map(a => a.id, "id")
                      .Map(a => a.name, "Szczepionkanazwa")
                      .Map(a => a.pesel_p, "PacjientPesel")
                      .Map(a => a.pesel_n, "Pielêgniarkapesel")
                      .Map(a => a.pesel_d, "Doktorpesel")
                      .Map(a => a.date, "data_wykonania")
                      .Map(a => a.obligatory, "obowi¹zkowowe")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<High_risk>()
                      .Map(a => a.id, "Chorobaid")
                      .Map(a => a.pesel_p, "PacjientPesel")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Patient>()
                      .Map(a => a.pesel, "Pesel")
                      .Map(a => a.first_name, "imie")
                      .Map(a => a.last_name, "nazwisko")
                      .Map(a => a.login, "login")
                      .Map(a => a.password, "haslo")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Nurse>()
                      .Map(a => a.pesel, "pesel")
                      .Map(a => a.first_name, "imie")
                      .Map(a => a.last_name, "nazwisko")
                      .Map(a => a.login, "login")
                      .Map(a => a.password, "haslo")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Doctor>()
                      .Map(a => a.pesel, "pesel")
                      .Map(a => a.first_name, "imie")
                      .Map(a => a.last_name, "nazwisko")
                      .Map(a => a.login, "login")
                      .Map(a => a.password, "haslo")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Ilness>()
                      .Map(a => a.id, "id")
                      .Map(a => a.name, "nazwa")
                );

            session.UserDefinedTypes.Define(
                UdtMap.For<Refund>()
                      .Map(a => a.id, "id")
                      .Map(a => a.type_r, "typ_refundacji ")
                );

            //Zamieniæ to na mysql -- bardzo analogicznie






            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
