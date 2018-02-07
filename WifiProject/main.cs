using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WifiProject
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login lg = new login(this);
            lg.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sign_up si = new sign_up(this);
            si.ShowDialog();
        }
    }
}
