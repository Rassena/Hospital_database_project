using Cassandra;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form4 : Form
    {

        Cluster cluster;
        ISession session;
        RowSet list_shots;
        RowSet list_users;
        RowSet list_patients;
        Utils util;



        public Form4(string login, ISession session)
        {
            this.Text = " Admin: " + login;

            this.session = session;
            util = new Utils();

            InitializeComponent();
            refresh();
        }


        private void refresh()
        {
            util.form4_refresh(comboBox1, listBox3, listBox4);
        }


        private void refresh_user_planned_shots()
        {
            util.form4_refresh_user_planned_shots(listBox2, comboBox1);
        }
        private void refresh_user_patients_list()
        {
            util.form4_refresh_user_patients_list(comboBox1, listBox1);
        }

        private void add_patient_to_user()
        {
            util.form4_add_patient_to_user(comboBox1, listBox1, listBox2, listBox3);
        }

        private void del_patient_to_user()
        {
            util.form4_del_patient_to_user(comboBox1, listBox1, listBox2);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            util.form4_button2_Click();    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vutil.form4_button1_Click();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            util.form4_button5_Click(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            util.form4_button6_Click(comboBox1, listBox1, listBox2, listBox3, listBox4);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            util.form4_comboBox1_SelectedIndexChanged(listBox2, comboBox1, listBox1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            util.form4_button4_Click(comboBox1, listBox1, listBox2, listBox3);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            util.form4_button7_Click(comboBox1, listBox1, listBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            util.form4_button3_Click();
        }
    }
}
