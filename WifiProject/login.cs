using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WifiProject
{
    public partial class login : Form
    {
        main mForm;
        MySqlConnection con;

        public login(main mForm)
        {
            InitializeComponent();
            this.mForm = mForm;

            // initiate DB connection
            con = new MySqlConnection();
            con.ConnectionString = "server=192.168.1.4;uid=root;pwd=root;database=wifi_project";
            try
            {
                con.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            con.Close();
            this.Close();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from users where email = @email and password = @pass";
            cmd.Parameters.AddWithValue("@email", textBox2.Text);
            cmd.Parameters.AddWithValue("@pass", textBox1.Text);
            try
            {
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["type"].ToString() == "admin")
                    {
                        admin ad = new admin(dr, mForm);
                        dr.Close();
                        con.Close();
                        mForm.Hide();
                        ad.Show();
                        this.Close();
                    }
                    else
                    {
                        user us = new user(dr, mForm);
                        dr.Close();
                        con.Close();
                        mForm.Hide();
                        us.Show();
                        this.Close();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Please enter correct email and password");
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
