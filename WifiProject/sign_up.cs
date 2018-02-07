using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace WifiProject
{
    public partial class sign_up : Form
    {
        MySqlConnection con;
        main mForm;
        public sign_up(main mForm)
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


        private void button1_Click(object sender, EventArgs e)
        {
            bool name_isNotNull = textBox1.Text != null && textBox1.Text.Length > 2 ? true : false;
            bool email_isValid = new EmailAddressAttribute().IsValid(textBox2.Text);
            bool password_isValid = ValidatePassword(textBox3.Text);

            if (!name_isNotNull) {
                MessageBox.Show("Please enter your name");
                return;
            }

            if (!email_isValid)
            {
                MessageBox.Show("Sorry your mail is not valid (example@example.example)");
                return;
            }

            if (!password_isValid)
            {
                MessageBox.Show("Sorry your password must contain upper case, lower case letter,\n has decimal digit and minimum length 8 letter");
                return;
            }

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "insert into users values ('', @name, @email, @pass, 'user')";
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@email", textBox2.Text);
            cmd.Parameters.AddWithValue("@pass", textBox3.Text);
            try
            {
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select * from users where email = @email and password = @pass";
                MySqlDataReader dr = cmd.ExecuteReader();
                user us = new user(dr, mForm);
                dr.Close();
                con.Close();
                this.mForm.Hide();
                us.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        static bool ValidatePassword(string password)
        {
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 15;

            if (password == null) throw new ArgumentNullException();

            bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c)) hasUpperCaseLetter = true;
                    else if (char.IsLower(c)) hasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) hasDecimalDigit = true;
                }
            }

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                        ;
            return isValid;

        }
    }
}
