using Cassandra;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class Fadd_Patient : Form
    {

        Utils util;

        public Fadd_Patient(ISession session)
        {
            util = new Utils();
            InitializeComponent();
        }


        private string check_first_name()
        {
            return util.fadd_Patient_check_first_name(textBox1);
        }

        private string check_last_name()
        {
            return util.fadd_Patient_check_last_name(textBox1, textBox2);
        }

        private long check_pesel()
        {
            return util.fadd_Patient_check_pesel(textBox1, textBox3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            util.fadd_Patient_button1_Click(this, textBox1, textBox2, textBox3, radioButton2, label5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            util.fadd_Patient_button2_Click(this);
        }
    }
}
