using Cassandra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class Fadd_Patient : Form
    {

        Cluster cluster;
        ISession session;

        public Fadd_Patient()
        {
            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();
            session = cluster.Connect("test_keyspace");

            InitializeComponent();
        }


        private string check_first_name()
        {
            string ret = null;
            if(textBox1.Text!=null&textBox1.Text!=" ")
            {
                if(textBox1.Text.Split(" ").Length == 1)
                {
                    ret = textBox1.Text;
                }
            }
            return ret;
        }

        private string check_last_name()
        {
            string ret = null;
            if (textBox2.Text != null & textBox1.Text != " ")
            {
                if (textBox2.Text.Split(" ").Length == 1)
                {
                    ret = textBox2.Text;
                }
            }
            return ret;
        }

        private long check_pesel()
        {
            long ret = 0;
            bool correct = false;
            if (textBox1.Text != null & textBox1.Text != " ")
            {
                if (textBox1.Text.Split(" ").Length == 1)
                {
                    if (textBox3.Text.Length == 11)
                    {
                        if(long.TryParse(textBox3.Text,out ret))
                        {
                            correct = true;
                        }
                    }
                }
            }
            if (correct)
               return ret;
            else
                return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string first_name = check_first_name();
            string last_name = check_last_name();
            long pesel =check_pesel();
            string gender = "M";

            if (radioButton2.Checked)
                gender = "K";

            if (first_name != null & last_name != null & pesel > 0)
            {
                string command = $"INSERT INTO test_patient (pesel, first_name, last_name, gender) VALUES ({pesel}, '{first_name}', '{last_name}', '{gender}');";
                session.Execute(command);
                this.Hide();
            }
            else
            {
                label5.Text = "Co najmniej jedna wartość jest błędna!";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
