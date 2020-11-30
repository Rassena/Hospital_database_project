using System;
using Cassandra;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace GUI
{
    public partial class Form1 : Form
    {

        ISession session;
        RowSet results;

        public Form1(ISession session)
        {

            this.session = session;

            InitializeComponent();
            listBox1.Items.Add($"data wykonania \t Nazwa szczepionki");
        }

        private static string SHA256ToString(string s)
        {
            using (var alg = SHA256.Create())
                return string.Join(null, alg.ComputeHash(Encoding.UTF8.GetBytes(s)).Select(x => x.ToString("x2")));
        }


        private void PatientB_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add($"data wykonania \t Nazwa szczepionki");
            results = session.Execute("SELECT * FROM test_patient;");

            long login = 0;
            bool canConvert = long.TryParse(loginTB.Text, out login);

            if (canConvert == true)
            {
                foreach (var result in results)
                {
                    long pesel = result.GetValue<long>("pesel");
                    if (pesel == long.Parse(loginTB.Text))
                    {
                        string tempP = result.GetValue<string>("first_name") + result.GetValue<string>("last_name");

                        tempP = SHA256ToString(tempP);

                        if (SHA256ToString(passwordTB.Text) == tempP)
                        {
                            Shot[] shots = result.GetValue<Shot[]>("shots") as Shot[];
                            if (shots != null)
                            {
                                foreach (Shot shot in shots)
                                    if (shot.done)
                                    {
                                        listBox1.Items.Add($"{shot.date} \t {shot.name}");
                                    }
                            }
                            else
                            {
                                listBox1.Items.Add($"Brak wykonanych szczepień");
                            }
                        }
                    }
                }
            }

        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private void userB_Click(object sender, EventArgs e)
        {

            listBox1.Items.Clear();
            listBox1.Items.Add($"data wykonania \t Nazwa szczepionki");

            var users = session.Execute("SELECT * FROM test_user");

            foreach (var user in users)
            {
                string login = user.GetValue<string>("login");
                if (loginTB.Text == login)
                {
                    string password = user.GetValue<string>("password");
                    if (SHA256ToString(passwordTB.Text) == password)
                    {
                        switch (user.GetValue<string>("type")){
                            case "doctor":
                                this.Hide();
                                var form3 = new Form3(login,session);
                                form3.Show();
                                break;
                            case "nurse":
                                this.Hide();
                                var form2 = new Form2(login,session);
                                form2.Show();
                                break;
                            case "admin":
                                this.Hide();
                                var form4 = new Form4(login,session);
                                form4.Show();
                                break;
                        }
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
