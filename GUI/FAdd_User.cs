using Cassandra;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class FAdd_User : Form
    {
        Utils util;
        public FAdd_User(ISession session)
        {
            util = new Utils();
            InitializeComponent();

        }
        private string check_login()
        {
            return util.fadd_User_check_login(textBox1);
        }

        private string check_password()
        {
            return util.fadd_User_check_password(textBox1, textBox2);
        }

        private bool check_password_confirm()
        {
            return util.fadd_User_check_password_confirm(textBox1, textBox2, textBox3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            util.fadd_User_button1_Click(this, textBox1, textBox2, textBox3, radioButton1, radioButton2, radioButton3, label4);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
