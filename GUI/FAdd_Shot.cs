using Cassandra;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class FAdd_Shot : Form
    {
        Utils util;
        public FAdd_Shot(ISession session)
        {

            util = new Utils();

            InitializeComponent();
        }

        private string check_name()
        {
            return util.fadd_Shot_check_name(textBox1);
        }

        private string check_illness()
        {
            return util.fadd_Shot_check_illness(textBox1, textBox2);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            util.fadd_Shot_button2_Click(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            util.fadd_Shot_button1_Click(this, textBox1, textBox2, radioButton1, radioButton3, label5);
        }
    }
}
