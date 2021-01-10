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
        Utils util;

        public Form1()
        {
            util = new Utils();

            InitializeComponent();
            listBox1.Items.Add($"data wykonania \t Nazwa szczepionki");

        }

        private void PatientB_Click(object sender, EventArgs e)
        {
            util.form1_PatientB_Click(listBox1, passwordTB, loginTB);
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private void userB_Click(object sender, EventArgs e)
        {
            util.form1_userB_Click(listBox1, passwordTB, loginTB);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            util.form1_button2_Click();
        }

    }
}
