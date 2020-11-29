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
    public partial class Form3 : Form
    {
        Cluster cluster;
        ISession session;
        RowSet results;
        RowSet list_shots;
        Row user;
        Row patient;

        public Form3(string login)
        {
            this.Text = login + " Doktor";

            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();
            session = cluster.Connect("test_keyspace");

            session.UserDefinedTypes.Define(
               UdtMap.For<Shot>()
                  .Map(a => a.date, "date")
                  .Map(a => a.name, "name")
                  .Map(a => a.obligatory, "obligatory")
                  .Map(a => a.done, "done")
            );

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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void comboBox1Refresh()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Wybierz Patcjenta");


            if (user.GetValue<long[]>("patients") != null)
            {
                foreach (long pesel in (user.GetValue<long[]>("patients")))
                {
                    foreach (var patient in session.Execute("SELECT * FROM test_patient;"))
                    {
                        if (pesel == patient.GetValue<long>("pesel"))
                        {
                            comboBox1.Items.Add($"{pesel} {patient.GetValue<string>("last_name")} {patient.GetValue<string>("first_name")}");
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox2Refresh()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Wybierz szcepionkę");

            if (list_shots != null)
            {
                foreach (var shot in list_shots)
                {
                    if (shot.GetValue<Boolean>("obligatory"))
                        comboBox2.Items.Add($"{shot.GetValue<string>("name")} {shot.GetValue<string>("illness")} Obowiązkowe");
                    else
                        comboBox2.Items.Add($"{shot.GetValue<string>("name")} {shot.GetValue<string>("illness")} Nie bowiązkowe");

                }
            }
            comboBox2.SelectedIndex = 0;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1Refresh();
        }

        private void listBox1Refresh()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("data nazwa wykonano Obowiązkowe");

            if (comboBox1.SelectedIndex != 0)
            {
                string[] str = comboBox1.Text.Split(" ");
                long pesel = long.Parse(str[0]);

                results = session.Execute("SELECT * FROM test_patient;");
                foreach (var pat in results)
                {
                    if (pesel == pat.GetValue<long>("pesel"))
                    {
                        patient = pat;
                    }
                }

                Shot[] shots = patient.GetValue<Shot[]>("shots");

                if (shots != null)
                {
                    foreach (Shot shot in patient.GetValue<Shot[]>("shots"))
                    {
                        string ob = "NIE";
                        string dn = "NIE";
                        if (shot.obligatory)
                            ob = "TAK";
                        if (shot.done)
                            dn = "TAK";

                        listBox1.Items.Add($"{shot.date} {shot.name} {dn} {ob}");
                    }
                }
                else
                {
                    listBox1.Items.Add($"Brak wykonanych szczepionek");
                }
            }
        }


        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form1 = new Form1();
            form1.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string shot = ((string)comboBox2.SelectedItem).Split(" ")[0];
            string date = dateTimePicker1.Value.ToString("yyy-MM-dd");
            string pesel = comboBox1.Text.Split(" ")[0];
            Boolean add = true;

            string test = $"UPDATE test_patient SET shots = shots +[{{ name: '{shot}' ,date : '{date}',done: {radioButton1.Checked}, obligatory: {false}}}] WHERE pesel = {pesel};";

            foreach (Object ob in listBox1.Items)
            {
                string comp = ob.ToString().Split(" ")[1];

                if (shot.Equals(comp))
                    add = false;
            }

            if (shot != null & date != null & comboBox1.SelectedItem != null&add)
                session.Execute(test);
            listBox1Refresh();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                radioButton2.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                radioButton1.Checked = false;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
