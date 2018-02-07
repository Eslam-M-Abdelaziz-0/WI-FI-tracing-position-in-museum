using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
namespace WifiProject
{
    public partial class Navigation : Form
    {

        private string url;

        public Navigation()
        {
            InitializeComponent();
        }

        public Navigation(string url)
        {
            InitializeComponent();

            this.url = url;
            webBrowser1.Navigate(url);
            txt_url.Text = url; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(txt_url.Text);
        }
    }
}
