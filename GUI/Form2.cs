using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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


        public Form2(string login, ISession session)
        {
            this.Text = " Pielęgniarka: " + login;

            this.session = session;

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

            if (comboBox1.Items != null)
            {
                foreach (var patient in session.Execute("SELECT * FROM test_patient;"))
                {
                    if(comboBox1.SelectedIndex != 0){
                        long pesel = long.Parse(comboBox1.Text.Split(" ")[0]);
                        if (pesel == patient.GetValue<long>("pesel"))
                        {
                            Shot[] shots = patient.GetValue<Shot[]>("shots");
                            if (shots != null)
                            {

                                foreach (Shot shot in shots)
                                {
                                    string ob = "Nieobowiązkowe";
                                    if (shot.obligatory)
                                    {
                                        ob = "Obowiązkowe";
                                    }
                                    if (!shot.done)
                                        comboBox2.Items.Add($"{shot.date} {shot.name} {ob}");
                                }
                            }

                        }
                    }
                }
            }
            comboBox2.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2Refresh();
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
            var form1 = new Form1(session);
            form1.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex!=0&comboBox2.SelectedIndex!=0)
            {
                string name = ((string)comboBox2.SelectedItem).Split(" ")[1];
                string date = dateTimePicker1.Value.ToString("yyy-MM-dd");
                long pesel = long.Parse(comboBox1.Text.Split(" ")[0]);
                int position = -1;
                results = session.Execute("SELECT * FROM test_patient;");
                foreach (var result in results)
                {
                    if (result.GetValue<long>("pesel") == pesel)
                    {
                        if (result.GetValue<Shot[]>("shots") != null)
                        {
                            Shot[] shots = (result.GetValue<Shot[]>("shots"));
                            for (int i = 0; i < result.GetValue<Shot[]>("shots").Length; i++)
                            {
                                if (shots[i].name == name)
                                {
                                    position = i;
                                }
                            }
                        }
                    }
                }


                if (name != null & date != null & position != -1)
                {
                    string test = $"UPDATE test_patient SET shots[{position}] = {{ name: '{name}' ,date : '{date}',done: {checkBox1.Checked}, obligatory: {checkBox2.Checked}}} WHERE pesel = {pesel};";
                    Console.WriteLine(test);
                    session.Execute(test);
                }
                listBox1Refresh();
            }
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

    }
}
