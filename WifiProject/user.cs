using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NativeWifi;
using MySql.Data.MySqlClient;

namespace WifiProject
{
    struct location
    {
        public int x, y;
    };

    public partial class user : Form
    {
        int link_index;
        string[] urls = new string[2];
        string str = "";
        private Timer t;
        location location1;
        Random random;
        

        main mForm;
        MySqlDataReader current_user;
        //--------------------------------------- mahmode ali -----------------//
        // Global Variable initiation
        Result result = new Result();  // Location in (x, y, z)
        List<rAP> APs = new List<rAP>();    // List For read All Access Point In This Area
        List<myPoint> myPoints = new List<myPoint>();   // List For My Access Point Information
        List<APmatched> listMatched = new List<APmatched>();    // List For Matched Access Point
        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        MySqlCommandBuilder CB;
        DataSet ds;
        MySqlDataReader dr;

        #region get x, y and z API
        void ShowInfi()
        {
            #region Read All Access Points

            APs.Clear();
            WlanClient client = new WlanClient();
            // Wlan = new WlanClient();

            try
            {
                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {
                    Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                    foreach (Wlan.WlanBssEntry network in wlanBssEntries)
                    {
                        //MessageBox.Show(network.hostTimestamp.ToString());
                        int rss = network.rssi;
                        //     MessageBox.Show(rss.ToString());
                        byte[] macAddr = network.dot11Bssid;
                        string tMac = "";
                        for (int i = 0; i < macAddr.Length; i++)
                        {

                            tMac += macAddr[i].ToString("x2").PadLeft(2, '0').ToUpper();
                        }
                        
                        //Culculate the distance
                        double p1m = -18.5;   //1.8
                        double pr = rss;
                        double p1m_pr = p1m - pr;
                        double n = 4.8;   //4.8
                        double pwor = (p1m_pr / (10 * n));
                        double d = Math.Pow(10, pwor);
                        // Display The Sorted List In DataGrad View

                        APs.Add(new rAP(System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString(),
                            tMac.ToString(),
                            network.linkQuality,
                            network.dot11BssType.ToString(),
                            rss,
                            d));
                    }
                    //Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

        }
        Result CalcPosition(List<rAP> list)
        {
            // Show All Information About All Read Accesses Point
            ShowInfi();
            // Sort The List Of Access Point Based in RSSID
            list.Sort();
            // Match Access Point With My Point, That Sorted in Database
            if (list.Count >= 3)
            {
                // get AP records from DB
                myPoints.Clear();
                cmd = new MySqlCommand("select * from ap", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    myPoints.Add(new myPoint(Convert.ToDouble(dr["x"]), Convert.ToDouble(dr["y"]),
                        dr["mac"].ToString(), dr["ssid"].ToString(), dr["name"].ToString(),
                        Convert.ToDouble(dr["rssidat1m"]), Convert.ToDouble(dr["maxrssid"]),
                        Convert.ToDouble(dr["noise"])));
                }
                // clear matched list of AP
                listMatched.Clear();
                foreach (rAP ap in list)
                {
                    foreach (myPoint myap in myPoints)
                    {
                        if (ap.MAC == myap.mac)
                        {
                            //Culculate the distance
                            double p1m = myap.rssidat1m;   //18.5
                            double pr = ap.RSSID;
                            double p1m_pr = p1m - pr;
                            double n = myap.noise;   //4.8
                            double pwor = (p1m_pr / (10 * n));
                            double d = Math.Pow(10, pwor);
                            ap.distance = d;

                            listMatched.Add(new APmatched(ap.SSID, myap.name, ap.MAC, ap.distance, myap.x, myap.y, myap.rssidat1m, myap.noise));
                        }
                    }

                }
                dr.Close();

            }
            else { MessageBox.Show("not enouph, at lest 3 access point \n to calculate the location"); return null; }

            // Calculate the Result
            if (listMatched.Count >= 3)
            {
                // Sort The Matched List by name
                listMatched.Sort();

                // Variable Initiation
                // Possible My ListMatched Stor 5 Access Point In This Case I Select Three Only EX: a0, a1, b0 from sorted matched point list
                var r1 = listMatched[0].distance;
                var x1 = listMatched[0].x;
                var y1 = listMatched[0].y;
                int aPositionInCoodinat = listMatched[0].name[1] - 48;
                int aNumber = listMatched[0].name[2] - 48;

                var r2 = listMatched[1].distance;
                var d = listMatched[1].x;
                var y2 = listMatched[1].y;

                // set initial values
                double r3 = 0;
                double i = 0;
                double j = 0;
                int bPositionInCoodinat = 0;
                int bNumber = 0;
                if (!listMatched[2].name.Contains("a"))
                {
                    r3 = listMatched[2].distance;   //
                    i = listMatched[2].x;
                    j = listMatched[2].y;
                    bPositionInCoodinat = listMatched[2].name[1] - 48;
                    bNumber = listMatched[2].name[2] - 48;
                }
                else if (listMatched.Count >= 4 && !listMatched[2].name.Contains("a"))
                {
                    r3 = listMatched[3].distance;
                    i = listMatched[3].x;
                    j = listMatched[3].y;
                    bPositionInCoodinat = listMatched[0].name[1] - 48;
                    bNumber = listMatched[2].name[2] - 48;
                }


                result.x = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(d, 2)) / (2 * d);
                result.y = ((Math.Pow(r1, 2) - Math.Pow(r3, 2) + Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * j)) - ((i * result.x) / j);
                result.z = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(result.x, 2) + Math.Pow(result.y, 2));

                result.x += 25 * Convert.ToInt32(aPositionInCoodinat);
                result.y += 25 * Convert.ToInt32(bPositionInCoodinat);
                if (bNumber < aNumber)
                {
                    result.y += 25;
                }


            }
            else { MessageBox.Show("not enouph, at lest 3 access point Matched with access point stored in my database"); }

            // return the result x,y and z
            return result;



        }
        //-------------------------- only use this function to get x,y and z -----//
        Result get_XYZ()
        {
            return CalcPosition(APs);
        }

        #endregion

        public user()
        {
            InitializeComponent();
            Discover_panel.Dock = DockStyle.Fill;
            //pan2page.Dock = DockStyle.Fill;
            //pan3page.Dock = DockStyle.Fill;
            //pan4page.Dock = DockStyle.Fill;
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

            //////----------------------- Example ----------------//
            //result = get_XYZ();
            //MessageBox.Show("X : " + result.x);
            //MessageBox.Show("y : " + result.y);
            //MessageBox.Show("z : " + result.z);
            //------------------------------------------------//
        }
        //---------------------------------- The End Mahmod -----------------------------------//

        public user(MySqlDataReader user, main mForm)
        {
            InitializeComponent();
            Discover_panel.Dock = DockStyle.Fill;
            //pan2page.Dock = DockStyle.Fill;
            //pan3page.Dock = DockStyle.Fill;
            //pan4page.Dock = DockStyle.Fill;
            this.mForm = mForm;
            this.current_user = user;
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


            current_user.Read();
            label1.Text = current_user["name"].ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            con.Close();
            this.mForm.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            con.Close();
            mForm.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pan1.Show();
            pan2.Hide();
            pan3.Hide();
            pan4.Hide();
            Discover_panel.Show();
            //pan2page.Hide();
            //pan3page.Hide();
            //pan4page.Hide();

            t.Enabled = true;
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            random = new Random();

            Discover_panel.Dock = DockStyle.Fill;
            //pan2page.Dock = DockStyle.Fill;
            //pan3page.Dock = DockStyle.Fill;
            //pan4page.Dock = DockStyle.Fill;


            pan1.Show();
            pan2.Hide();
            pan3.Hide();
            pan4.Hide();



        }

        private void Discover_panel_Paint(object sender, PaintEventArgs e)
        {
            t = new Timer();
            t.Interval = 4000;
            t.Enabled = false;
            t.Tick += new EventHandler(t_tick);
        }


        private void t_tick(Object sender, EventArgs e)
        {

            //Result res = CalcPosition(APs);
            //if(res != null)
            //{
            //    textBox1.Text = res.x.ToString();
            //    textBox2.Text = res.y.ToString();
            //    textBox3.Text = res.z.ToString();
            //}




            location1.x = random.Next(0, 15);
            location1.y = random.Next(0, 10);

            if (location1.x < 5)
                location1.x = 0;
            else if (location1.x >= 5 && location1.x < 10)
                location1.x = 5;
            else if (location1.x >= 10 && location1.x < 15)
                location1.x = 10;
            else
                location1.x = 15;

            if (location1.y < 5)
                location1.y = 0;
            else
                location1.y = 10;

           // location1.x = 0;location1.y = 0;

            //MessageBox.Show("X : " + location1.x + "Y : " + location1.y);

            int index = comboBox1.SelectedIndex;

            if (index == 0)
            {
                label_artical.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                link_index = 1;
            }

            else 
            {
                
                label_artical.RightToLeft = System.Windows.Forms.RightToLeft.No;
                index = 5;
                link_index = 0;
            }

            MySqlCommand cmd = new MySqlCommand("p3", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("new_x", location1.x);
            cmd.Parameters.AddWithValue("new_y", location1.y);
            cmd.Parameters.AddWithValue("i", index);

            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            pictureBox7.Image = Image.FromFile(@"Photo\"+ dr[0].ToString());
            str = dr[1].ToString();
            string links = dr[2].ToString();
            //urls = links.Split('$');
            //lbl_link.Text = urls[link_index];  
            lbl_link.Text = links;
                        
            dr.Close();

            string str1 = "        ";
            label_artical.Text = "       ";

            for (int i = 0,j=0; i < str.Length; i++)
            {
                j++;
                str1 += str[i];
                if (j > 100 && str[i] == ' ')
                {
                    j = 0;
                    str1 = str1 + "\n";
                }
            }

            label_artical.Text = str1;
        }

        private void lbl_link_DoubleClick(object sender, EventArgs e)
        {
            Navigation nav = new Navigation(lbl_link.Text);
            nav.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e) // form maximized
        {
            WindowState = FormWindowState.Maximized;
        }

        private void pictureBox4_Click(object sender, EventArgs e) // form normal
        {
            WindowState = FormWindowState.Normal;
        }

        private void pictureBox5_Click(object sender, EventArgs e) // form minimized
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pan1.Show(); // Access Point
            pan2.Hide();
            pan3.Hide();
            pan4.Hide();
            Discover_panel.Hide();
            //pan2page.Hide();
            //pan3page.Hide();
            //pan4page.Hide();
        }
    }
}
