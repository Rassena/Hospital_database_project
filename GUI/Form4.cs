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
    public partial class Form4 : Form
    {

        Cluster cluster;
        ISession session;
        RowSet list_shots;
        RowSet list_users;
        RowSet list_patients;



        public Form4(string login, ISession session)
        {
            this.Text = " Admin: " + login;

            this.session = session;

            InitializeComponent();
            refresh();
        }


        private void refresh()
        {
            list_patients = session.Execute("SELECT * FROM test_patient;");
            list_shots = session.Execute("SELECT * FROM test_shot;");
            list_users = session.Execute("SELECT * FROM test_user");

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Wybierz użytkownika");
            comboBox1.SelectedIndex = 0;

            listBox3.Items.Clear();
            listBox4.Items.Clear();

            foreach(var patient in list_patients)
            {
                string first_name = patient.GetValue<string>("first_name");
                string last_name = patient.GetValue<string>("last_name");
                long pesel= patient.GetValue<long>("pesel");
                string gender = patient.GetValue<string>("gender");

                listBox3.Items.Add($"{pesel} {first_name} {last_name} {gender}");
            }

            foreach(var shot in list_shots)
            {
                string name = shot.GetValue<string>("name");
                string illness = shot.GetValue<string>("illness");
                string obligatory = "nieobowiązkowe";



                if (shot.GetValue<bool>("obligatory"))
                    obligatory = "obowiązkowe";
                if (shot.GetValue<bool>("available"))
                    listBox4.Items.Add($"{name} {illness} {obligatory}");

            }

            foreach(var user in list_users)
            {
                string login = user.GetValue<string>("login");
                string type = "unkow";

                switch (user.GetValue<string>("type"))
                {
                    case "admin":
                        type = "Admin";
                        break;
                    case "nurse":
                        type = "Pielęgniarka";
                        break;
                    case "doctor":
                        type = "Doktor";
                        break;
                }
                comboBox1.Items.Add($"{type} {login}");
            }
        }


        private void refresh_user_planned_shots()
        {
            listBox2.Items.Clear();

            if (comboBox1.SelectedIndex != 0)
            {
                if (comboBox1.Text.Split(" ")[0] != "Admin")
                {
                    list_users = session.Execute("SELECT * FROM test_user");
                    foreach (var user in list_users)
                    {
                        if (user.GetValue<string>("login") == comboBox1.Text.Split(" ")[1])
                        {
                            if (user.GetValue<long[]>("patients") != null)
                            {
                                foreach (long pesel in user.GetValue<long[]>("patients"))
                                {
                                    list_patients = session.Execute("SELECT * FROM test_patient;");
                                    foreach (var patient in list_patients)
                                    {
                                        if (pesel == patient.GetValue<long>("pesel"))
                                        {
                                            Shot[] shots = patient.GetValue<Shot[]>("shots");
                                            if (shots != null)
                                            {
                                                foreach (Shot shot in shots)
                                                {
                                                    if (!shot.done)
                                                    {
                                                        listBox2.Items.Add($"{shot.date} {shot.name} {pesel}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void refresh_user_patients_list()
        {

            listBox1.Items.Clear();

            if (comboBox1.SelectedIndex != 0)
            {
                if (comboBox1.Text.Split(" ")[0] != "Admin")
                {
                    list_users = session.Execute("SELECT * FROM test_user");
                    foreach (var user in list_users)
                    {
                        if (user.GetValue<string>("login") == comboBox1.Text.Split(" ")[1])
                        {
                            if (user.GetValue<long[]>("patients") != null)
                            {
                                foreach (long pesel in user.GetValue<long[]>("patients")){
                                    list_patients = session.Execute("SELECT * FROM test_patient;");
                                    foreach (var patient in list_patients)
                                    {
                                        if (pesel == patient.GetValue<long>("pesel"))
                                        {
                                            string first_name = patient.GetValue<string>("first_name");
                                            string last_name = patient.GetValue<string>("last_name");
                                            string gender = patient.GetValue<string>("gender");
                                            listBox1.Items.Add($"{pesel} {first_name} {last_name} {gender}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void add_patient_to_user()
        {
            if (comboBox1.SelectedIndex!=0 & comboBox1.SelectedIndex.ToString().Split(" ")[0] != "Admin")
            {
                if (listBox3.SelectedItem != null)
                {
                    long pesel =long.Parse(listBox3.SelectedItem.ToString().Split(" ")[0]);
                    bool add = true;
                    list_users = session.Execute("SELECT * FROM test_user");
                    foreach(var user in list_users)
                    {
                        if (user.GetValue<string>("login")== comboBox1.SelectedItem.ToString().Split(" ")[1])
                        {
                            if (user.GetValue<long[]>("patients") != null)
                            {
                                foreach(long patient in user.GetValue<long[]>("patients"))
                                {
                                    if (pesel == patient)
                                    {
                                        add = false;
                                    }
                                }
                            }
                        }
                    }
                    if (add)
                    {
                        string command = $"UPDATE test_user SET patients = patients +  [{pesel}] WHERE login = '{comboBox1.SelectedItem.ToString().Split(" ")[1]}';";
                        session.Execute(command);
                    }
                }
            }
            refresh_user_patients_list();
            refresh_user_planned_shots();
        }

        private void del_patient_to_user()
        {
            if (comboBox1.SelectedIndex != 0 & comboBox1.SelectedIndex.ToString().Split(" ")[0] != "Admin")
            {
                if (listBox1.SelectedItem != null)
                {
                    string pesel = listBox1.SelectedItem.ToString().Split(" ")[0];
                    string command = $"UPDATE test_user SET patients = patients -  [{pesel}] WHERE login = '{comboBox1.SelectedItem.ToString().Split(" ")[1]}';";
                    session.Execute(command);
                }
            }
            refresh_user_patients_list();
            refresh_user_planned_shots();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var fAdd_User = new FAdd_User(session);
            fAdd_User.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fadd_Patient = new Fadd_Patient(session);
            fadd_Patient.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form1 = new Form1(session);
            form1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            refresh();
            refresh_user_patients_list();
            refresh_user_planned_shots();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_user_patients_list();
            refresh_user_planned_shots();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            add_patient_to_user();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            del_patient_to_user();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var fadd_Shot = new FAdd_Shot(session);
            fadd_Shot.Show();
        }
    }
}
