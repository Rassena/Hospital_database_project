using System;
using Cassandra;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form2 : Form
    {
        Cluster cluster;
        ISession session;
        RowSet results;
        RowSet list_shots;
        Row user;
        Row patient;
        Utils util;


        public Form2(string login, ISession session)
        {
            this.Text = " Pielęgniarka: " + login;

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

            comboBox1Refresh();
            comboBox2Refresh();
            listBox1Refresh();

        }

        private void comboBox1Refresh()
        {
            util.form2_comboBox1Refresh(comboBox1);
        }

        private void comboBox2Refresh()
        {
            util.form2_comboBox2Refresh(comboBox1,  comboBox2);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            util.form2_comboBox1_SelectedIndexChanged(comboBox1, comboBox2, listBox1);
        }

        private void listBox1Refresh()
        {
            util.form2_listBox1Refresh(listBox1, comboBox1);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            util.form2_button1_Click(this);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            util.form2_button2_Click(comboBox1, comboBox2, dateTimePicker1, listBox1, checkBox1, checkBox2);
        }


    }
}
