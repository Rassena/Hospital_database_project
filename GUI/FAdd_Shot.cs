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
    public partial class FAdd_Shot : Form
    {
        Cluster cluster;
        ISession session;
        public FAdd_Shot(ISession session)
        {

            this.session = session;

            InitializeComponent();
        }

        private string check_name()
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

        private string check_illness()
        {
            string ret = "";
            if (textBox2.Text != null & textBox1.Text != " ")
            {
                if (textBox2.Text.Split(" ").Length == 1)
                {
                    ret = textBox2.Text;
                }
            }
            return ret;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = check_name();
            string illness = check_illness();
            bool obligatory = false;
            bool available = false;

            if (radioButton1.Checked)
                available = true;
            if (radioButton3.Checked)
                obligatory = true;

            if (name != "" & illness != "")
            {
                string command = $"INSERT INTO test_shot (name, illness, available, obligatory) VALUES ('{name}', '{illness}', {available}, {obligatory});";
                session.Execute(command);
                this.Hide();
            }
            else
            {
                label5.Text = "Co najmniej jedna wartość jest błędna!";
            }
        }
    }
}
