using Cassandra;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form3 : Form
    {
        Cluster cluster;
        ISession session;
        RowSet results;
        RowSet list_shots;
        Row user;
        Row patient;
        Utils util;

        public Form3(string login, ISession session)
        {
            this.Text = login + " Doktor";

            this.session = session;
            util = new Utils();

            results = session.Execute("SELECT * FROM test_patient;");
            list_shots = session.Execute("SELECT * FROM test_shot;");
            var users = session.Execute("SELECT * FROM test_user");

            foreach (var usr in users)
            {
                if (login == usr.GetValue<string>("login"))
                {
                    user = usr;
                }
            }

            InitializeComponent();


            comboBox3Refresh();
            comboBox1Refresh();
            comboBox2Refresh();
            listBox1Refresh();

        }
        private void comboBox1Refresh()
        {
            util.form3_comboBox1Refresh(comboBox1);
        }

        private void comboBox2Refresh()
        {
            util.form3_comboBox2Refresh(comboBox2, comboBox3);
        }


        private void comboBox3Refresh()
        {
            util.form3_comboBox3Refresh(comboBox3);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1Refresh();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2Refresh();
        }

        private void listBox1Refresh()
        {
            util.form3_listBox1Refresh(listBox1, comboBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            util.form3_button1_Click();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            util.form3_button2_Click(comboBox1, comboBox2, dateTimePicker1,listBox1, radioButton1);
        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            util.form3_radioButton1_CheckedChanged(radioButton1, radioButton2);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            util.form3_radioButton2_CheckedChanged(radioButton1, radioButton2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            util.form3_button3_Click(comboBox2, comboBox3);
        }
    }
}
