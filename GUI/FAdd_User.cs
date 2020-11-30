using Cassandra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class FAdd_User : Form
    {
        Cluster cluster;
        ISession session;
        public FAdd_User(ISession session)
        {

            this.session = session;

            InitializeComponent();

        }
        private string check_login()
        {
            string ret = "";
            if (textBox1.Text != null & textBox1.Text != " ")
            {
                if (textBox1.Text.Split(" ").Length == 1)
                {
                    ret = textBox1.Text;
                }
            }
            return ret;
        }

        private string check_password()
        {
            string ret = "";
            if (textBox2.Text != null & textBox1.Text != " ")
            {
                if (textBox2.Text.Length >= 8)
                {
                    ret = textBox2.Text;
                }
            }
            return ret;
        }

        private bool check_password_confirm()
        {
            bool ret = false;
            if (textBox2.Text != null & textBox1.Text != " ")
            {
                if (textBox2.Text == textBox3.Text)
                {
                    ret = true;
                }
            }
            return ret;
        }

        private static string SHA256ToString(string s)
        {
            using (var alg = SHA256.Create())
                return string.Join(null, alg.ComputeHash(Encoding.UTF8.GetBytes(s)).Select(x => x.ToString("x2")));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = check_login();
            string password = check_password();
            bool confirm = check_password_confirm();
            string type = null;

            if (radioButton1.Checked)
            {
                type = "admin";
            }
            if (radioButton2.Checked)
            {
                type = "doctor";
            }
            if (radioButton3.Checked)
            {
                type = "nurse";
            }

            if (login != "" & password != "" & confirm & type != "")
            {
                string command = $"INSERT INTO test_user (login, password, type) VALUES ('{login}', '{SHA256ToString(password)}', '{type}');";
                session.Execute(command);
                this.Hide();
            }
            else
            {
                label4.Text = "Co najmniej jedna wartość jest błędna!";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
