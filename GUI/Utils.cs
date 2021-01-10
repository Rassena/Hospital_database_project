using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    class Utils
    {

        ISession session;
        RowSet results;
        Form1 f1;
        Cluster cluster;
        Row user;
        Row patient;
        RowSet list_shots;
        RowSet list_users;
        RowSet list_patients;


        //----------------------------------------communism----------------------------------------------//

        public string SHA256ToString(string s)
        {
            using (var alg = SHA256.Create())
                return string.Join(null, alg.ComputeHash(Encoding.UTF8.GetBytes(s)).Select(x => x.ToString("x2")));
        }


        //----------------------------------------Form1----------------------------------------------//
        

        public void form1_PatientB_Click(ListBox listBox1, TextBox passwordTB, TextBox loginTB)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add($"data wykonania \t Nazwa szczepionki");
        //    results = session.Execute("SELECT * FROM test_patient;");

            long login = 0;
            bool canConvert = long.TryParse(loginTB.Text, out login);

            if (canConvert == true)
            {
            //    foreach (var result in results)
                {
                //    long pesel = result.GetValue<long>("pesel");
                //    if (pesel == long.Parse(f1.loginTB.Text))
                if (1234 == long.Parse(loginTB.Text))
                    {
                        //   string tempP = result.GetValue<string>("first_name") + result.GetValue<string>("last_name");
                        string tempP = "AlaKota";
                        tempP = SHA256ToString(tempP);

                        if (SHA256ToString(passwordTB.Text) == tempP)
                        {
                            /*
                            Shot[] shots = result.GetValue<Shot[]>("shots") as Shot[];
                            if (shots != null)
                            {
                                foreach (Shot shot in shots)
                                    if (shot.done)
                                    {
                                        f1.listBox1.Items.Add($"{shot.date} \t {shot.name}");
                                    }
                            }
                            else
                            {
                                f1.listBox1.Items.Add($"Brak wykonanych szczepień");
                            }
                            */
                            listBox1.Items.Add($"Brak wykonanych szczepień");
                        }
                    }
                }
            }

        }

        public void form1_userB_Click(ListBox listBox1, TextBox passwordTB, TextBox loginTB)
        {

            listBox1.Items.Clear();
            listBox1.Items.Add($"data wykonania \t Nazwa szczepionki");

            var users = session.Execute("SELECT * FROM test_user");

            foreach (var user in users)
            {
                string login = user.GetValue<string>("login");
                if (loginTB.Text == login)
                {
                    string password = user.GetValue<string>("password");
                    if (SHA256ToString(passwordTB.Text) == password)
                    {
                        switch (user.GetValue<string>("type"))
                        {
                            case "doctor":
                                f1.Hide();
                                var form3 = new Form3(login, session);
                                form3.Show();
                                break;
                            case "nurse":
                                f1.Hide();
                                var form2 = new Form2(login, session);
                                form2.Show();
                                break;
                            case "admin":
                                f1.Hide();
                                var form4 = new Form4(login, session);
                                form4.Show();
                                break;
                        }
                    }
                }
                if (loginTB.Text == login)
                {
                    string password = SHA256ToString("admin1");
                    if (SHA256ToString(passwordTB.Text) == password)
                    {
                        listBox1.Items.Add($"zalogowano się admin");
                    }

                }
            }
        }

        public void form1_button2_Click()
        {
            Application.Exit();
        }



        //----------------------------------------Form2----------------------------------------------//


        public void form2_comboBox1Refresh(ComboBox comboBox1)
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

        public void form2_comboBox2Refresh(ComboBox comboBox1, ComboBox comboBox2)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Wybierz szcepionkę");

            if (comboBox1.Items != null)
            {
                foreach (var patient in session.Execute("SELECT * FROM test_patient;"))
                {
                    if (comboBox1.SelectedIndex != 0)
                    {
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

        public void form2_comboBox1_SelectedIndexChanged(ComboBox comboBox1, ComboBox comboBox2, ListBox listBox1)
        {
            form2_comboBox2Refresh(comboBox1, comboBox2);
            form2_listBox1Refresh(listBox1, comboBox1);
        }

        public void form2_listBox1Refresh(ListBox listBox1, ComboBox comboBox1)
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

        public void form2_button1_Click(Form2 f2)
        {
            f2.Hide();
            //      var form1 = new Form1(session);
            //    form1.Show();
        }

        public void form2_button2_Click(ComboBox comboBox1, ComboBox comboBox2, DateTimePicker dateTimePicker1, ListBox listBox1,CheckBox checkBox1, CheckBox checkBox2)
        {
            if (comboBox1.SelectedIndex != 0 & comboBox2.SelectedIndex != 0)
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
                form2_listBox1Refresh(listBox1, comboBox1);
            }
        }



        //----------------------------------------Form3----------------------------------------------//

        public void form3_comboBox1Refresh(ComboBox comboBox1)
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

        public void form3_comboBox2Refresh(ComboBox comboBox2, ComboBox comboBox3)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Wybierz szcepionkę");
            list_shots = session.Execute("SELECT * FROM test_shot;");

            if (list_shots != null)
            {
                foreach (var shot in list_shots)
                {
                    if (shot.GetValue<string>("illness") == comboBox3.SelectedItem.ToString() || comboBox3.SelectedIndex == 1)
                        if (shot.GetValue<Boolean>("obligatory"))
                            comboBox2.Items.Add($"{shot.GetValue<string>("name")} {shot.GetValue<string>("illness")} Obowiązkowe");
                        else
                            comboBox2.Items.Add($"{shot.GetValue<string>("name")} {shot.GetValue<string>("illness")} Niebowiązkowe");

                }
            }
            comboBox2.SelectedIndex = 0;
        }

        public void form3_comboBox3Refresh(ComboBox comboBox3)
        {
            comboBox3.Items.Clear();
            comboBox3.Items.Add("Wybierz chorobę");
            comboBox3.Items.Add("Wszystkie");

            list_shots = session.Execute("SELECT * FROM test_shot;");

            if (list_shots != null)
            {
                foreach (var shot in list_shots)
                {
                    if (!comboBox3.Items.Contains(shot.GetValue<string>("illness")))
                        comboBox3.Items.Add($"{shot.GetValue<string>("illness")}");
                }
            }
            comboBox3.SelectedIndex = 0;
        }

        public void form3_comboBox1_SelectedIndexChanged(ListBox listBox1, ComboBox comboBox1)
        {
            form3_listBox1Refresh( listBox1, comboBox1);
        }

        public void form3_comboBox3_SelectedIndexChanged(ComboBox comboBox2, ComboBox comboBox3)
        {
            form3_comboBox2Refresh(comboBox2, comboBox3);
        }

        public void form3_listBox1Refresh(ListBox listBox1, ComboBox comboBox1)
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

        public void form3_button1_Click()
        {
            //        this.Hide();
            //        var form1 = new Form1(session);
            //         form1.Show();
        }

        public void form3_button2_Click(ComboBox comboBox1, ComboBox comboBox2, DateTimePicker dateTimePicker1, ListBox listBox1, RadioButton radioButton1)
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

            if (shot != null & date != null & comboBox1.SelectedItem != null & add)
                session.Execute(test);
            form3_listBox1Refresh(listBox1, comboBox1);

        }

        public void form3_radioButton1_CheckedChanged(RadioButton radioButton1, RadioButton radioButton2)
        {
            if (radioButton1.Checked)
                radioButton2.Checked = false;
        }

        public void form3_radioButton2_CheckedChanged(RadioButton radioButton1, RadioButton radioButton2)
        {
            if (radioButton2.Checked)
                radioButton1.Checked = false;
        }

        public void form3_button3_Click(ComboBox comboBox2, ComboBox comboBox3)
        {
            form3_comboBox2Refresh(comboBox2, comboBox3);
        }



        //----------------------------------------Form4----------------------------------------------//

        public void form4_refresh(ComboBox comboBox1, ListBox listBox3, ListBox listBox4)
        {
            list_patients = session.Execute("SELECT * FROM test_patient;");
            list_shots = session.Execute("SELECT * FROM test_shot;");
            list_users = session.Execute("SELECT * FROM test_user");

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Wybierz użytkownika");
            comboBox1.SelectedIndex = 0;

            listBox3.Items.Clear();
            listBox4.Items.Clear();

            foreach (var patient in list_patients)
            {
                string first_name = patient.GetValue<string>("first_name");
                string last_name = patient.GetValue<string>("last_name");
                long pesel = patient.GetValue<long>("pesel");
                string gender = patient.GetValue<string>("gender");

                listBox3.Items.Add($"{pesel} {first_name} {last_name} {gender}");
            }

            foreach (var shot in list_shots)
            {
                string name = shot.GetValue<string>("name");
                string illness = shot.GetValue<string>("illness");
                string obligatory = "nieobowiązkowe";



                if (shot.GetValue<bool>("obligatory"))
                    obligatory = "obowiązkowe";
                if (shot.GetValue<bool>("available"))
                    listBox4.Items.Add($"{name} {illness} {obligatory}");

            }

            foreach (var user in list_users)
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

        public void form4_refresh_user_planned_shots(ListBox listBox2, ComboBox comboBox1)
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
       
        public void form4_refresh_user_patients_list(ComboBox comboBox1, ListBox listBox1)
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
                                foreach (long pesel in user.GetValue<long[]>("patients"))
                                {
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

        public void form4_add_patient_to_user(ComboBox comboBox1, ListBox listBox1, ListBox listBox2,ListBox listBox3)
        {
            if (comboBox1.SelectedIndex != 0 & comboBox1.SelectedIndex.ToString().Split(" ")[0] != "Admin")
            {
                if (listBox3.SelectedItem != null)
                {
                    long pesel = long.Parse(listBox3.SelectedItem.ToString().Split(" ")[0]);
                    bool add = true;
                    list_users = session.Execute("SELECT * FROM test_user");
                    foreach (var user in list_users)
                    {
                        if (user.GetValue<string>("login") == comboBox1.SelectedItem.ToString().Split(" ")[1])
                        {
                            if (user.GetValue<long[]>("patients") != null)
                            {
                                foreach (long patient in user.GetValue<long[]>("patients"))
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
            form4_refresh_user_patients_list(comboBox1, listBox1);
            form4_refresh_user_planned_shots(listBox2, comboBox1);
        }

        public void form4_del_patient_to_user(ComboBox comboBox1, ListBox listBox1, ListBox listBox2)
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
            form4_refresh_user_patients_list(comboBox1, listBox1);
            form4_refresh_user_planned_shots(listBox2, comboBox1);
        }


        public void form4_button2_Click()
        {
            var fAdd_User = new FAdd_User(session);
            fAdd_User.Show();
        }

        public void form4_button1_Click()
        {
            var fadd_Patient = new Fadd_Patient(session);
            fadd_Patient.Show();
        }

        public void form4_button5_Click(Form4 f4)
        {
            //        this.Hide();
            //       var form1 = new Form1(session);
            //       form1.Show();
        }

        public void form4_button6_Click(ComboBox comboBox1, ListBox listBox1, ListBox listBox2, ListBox listBox3, ListBox listBox4)
        {
            form4_refresh(comboBox1, listBox3, listBox4);
            form4_refresh_user_patients_list(comboBox1,listBox1);
            form4_refresh_user_planned_shots(listBox2,comboBox1);
        }

        public void form4_comboBox1_SelectedIndexChanged(ListBox listBox2, ComboBox comboBox1,ListBox listBox1)
        {
            form4_refresh_user_patients_list(comboBox1, listBox1);
            form4_refresh_user_planned_shots(listBox2, comboBox1);
        }

        public void form4_button4_Click(ComboBox comboBox1, ListBox listBox1, ListBox listBox2, ListBox listBox3)
        {
            form4_add_patient_to_user(comboBox1, listBox1, listBox2, listBox3);
        }

        public void form4_button7_Click(ComboBox comboBox1, ListBox listBox1, ListBox listBox2)
        {
            form4_del_patient_to_user(comboBox1, listBox1, listBox2);
        }

        public void form4_button3_Click()
        {
            var fadd_Shot = new FAdd_Shot(session);
            fadd_Shot.Show();
        }


        //----------------------------------------Sub_Form4_Fadd_Patient----------------------------------------------//

        public string fadd_Patient_check_first_name(TextBox textBox1)
        {
            string ret = null;
            if (textBox1.Text != null & textBox1.Text != " ")
            {
                if (textBox1.Text.Split(" ").Length == 1)
                {
                    ret = textBox1.Text;
                }
            }
            return ret;
        }

        public string fadd_Patient_check_last_name(TextBox textBox1, TextBox textBox2)
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

        public long fadd_Patient_check_pesel(TextBox textBox1, TextBox textBox3)
        {
            long ret = 0;
            bool correct = false;
            if (textBox1.Text != null & textBox1.Text != " ")
            {
                if (textBox1.Text.Split(" ").Length == 1)
                {
                    if (textBox3.Text.Length == 11)
                    {
                        if (long.TryParse(textBox3.Text, out ret))
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

        public void fadd_Patient_button1_Click(Fadd_Patient fp, TextBox textBox1, TextBox textBox2, TextBox textBox3, RadioButton radioButton2,Label label5)
        {
            string first_name = fadd_Patient_check_first_name(textBox1);
            string last_name = fadd_Patient_check_last_name(textBox1, textBox2);
            long pesel = fadd_Patient_check_pesel(textBox1, textBox3);
            string gender = "M";

            if (radioButton2.Checked)
                gender = "K";

            if (first_name != null & last_name != null & pesel > 0)
            {
                string command = $"INSERT INTO test_patient (pesel, first_name, last_name, gender) VALUES ({pesel}, '{first_name}', '{last_name}', '{gender}');";
                session.Execute(command);
                fp.Hide();
            }
            else
            {
                label5.Text = "Co najmniej jedna wartość jest błędna!";
            }

        }

        public void fadd_Patient_button2_Click(Fadd_Patient fp)
        {
            fp.Hide();
        }



        //----------------------------------------Sub_Form4_Fadd_Shot----------------------------------------------//

        public string fadd_Shot_check_name(TextBox textBox1)
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

        public string fadd_Shot_check_illness(TextBox textBox1, TextBox textBox2)
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

        public void fadd_Shot_button2_Click(FAdd_Shot fs)
        {
            fs.Close();
        }

        public void fadd_Shot_button1_Click(FAdd_Shot fs, TextBox textBox1, TextBox textBox2, RadioButton radioButton1, RadioButton radioButton3, Label label5)
        {
            string name = fadd_Shot_check_name(textBox1);
            string illness = fadd_Shot_check_illness(textBox1, textBox2);
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
                fs.Hide();
            }
            else
            {
                label5.Text = "Co najmniej jedna wartość jest błędna!";
            }
        }


        //----------------------------------------Sub_Form4_Fadd_User----------------------------------------------//


        public string fadd_User_check_login(TextBox textBox1)
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

        public string fadd_User_check_password(TextBox textBox1, TextBox textBox2)
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

        public bool fadd_User_check_password_confirm(TextBox textBox1, TextBox textBox2, TextBox textBox3)
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

        public void fadd_User_button1_Click(FAdd_User fu, TextBox textBox1, TextBox textBox2, TextBox textBox3, RadioButton radioButton1, RadioButton radioButton2, RadioButton radioButton3, Label label4)
        {
            string login = fadd_User_check_login(textBox1);
            string password = fadd_User_check_password(textBox1, textBox2);
            bool confirm = fadd_User_check_password_confirm(textBox1, textBox2, textBox3);
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
                fu.Hide();
            }
            else
            {
                label4.Text = "Co najmniej jedna wartość jest błędna!";
            }
        }

        public void fadd_User_button2_Click(FAdd_User fu)
        {
            fu.Hide();
        }




    }



}
