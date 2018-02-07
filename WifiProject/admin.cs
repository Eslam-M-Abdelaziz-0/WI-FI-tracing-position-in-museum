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
using NativeWifi;

namespace WifiProject
{
    public partial class admin : Form
    {
        private int x = 0;
        private int y = 0;
        

        string str;
        string path = "";
        string img_name = "";

        main mForm;
        MySqlDataReader user;
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

        public admin(MySqlDataReader dr, main mForm)
        {
            InitializeComponent();
            this.mForm = mForm;
            this.user = dr;
            // initialize panels
            pan1Page.Dock = DockStyle.Fill;
            pan2page.Dock = DockStyle.Fill;
            pan3page.Dock = DockStyle.Fill;
            pan4page.Dock = DockStyle.Fill;

            pan1.Show();
            pan2.Hide();
            pan3.Hide();
            pan4.Hide();
            pan2page.Hide();
            pan3page.Hide();
            pan4page.Hide();

            // initiate DB connection
            con = new MySqlConnection();
            con.ConnectionString = "server=192.168.1.4;uid=root;pwd=root;database=wifi_project";
            try
            {
                con.Open();
                cmd = new MySqlCommand("select * from ap",con);
                da = new MySqlDataAdapter(cmd);
                CB = new MySqlCommandBuilder(da);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView3.DataSource = ds.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            ShowInfi(dataGridView2);
            ShowInfi(dataGridView4);
        }

        public admin()
        {
            InitializeComponent();
            pan1.Show();
            pan2.Hide();
            pan3.Hide();
            pan4.Hide();
            pan2page.Hide();
            pan3page.Hide();
            pan4page.Hide();
            // initialize panels
            pan1Page.Dock = DockStyle.Fill;
            pan2page.Dock = DockStyle.Fill;
            pan3page.Dock = DockStyle.Fill;
            pan4page.Dock = DockStyle.Fill;
            // initiate DB connection
            con = new MySqlConnection();
            con.ConnectionString = "server=192.168.1.4;uid=root;pwd=root;database=wifi_project";
            try
            {
                con.Open();
                cmd = new MySqlCommand("select * from ap", con);
                da = new MySqlDataAdapter(cmd);
                CB = new MySqlCommandBuilder(da);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView3.DataSource = ds.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            ShowInfi(dataGridView2);
            ShowInfi(dataGridView4);
        }

        private void pictureBox2_Click(object sender, EventArgs e) // application close
        {
            con.Close();
            this.mForm.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e) // form maximize
        {
            WindowState = FormWindowState.Maximized;
        }

        private void pictureBox4_Click(object sender, EventArgs e) // from normal
        {
            WindowState = FormWindowState.Normal;
        }

        private void pictureBox5_Click(object sender, EventArgs e) // form minimize
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e) // open pan1page
        {
            pan1.Show(); // Access Point
            pan2.Hide();
            pan3.Hide();
            pan4.Hide();
            pan1Page.Show();
            pan2page.Hide();
            pan3page.Hide();
            pan4page.Hide();
        }

        private void button2_Click(object sender, EventArgs e) // open pan2page
        {
            pan1.Hide();
            pan2.Show(); // Draw AP Network
            pan3.Hide();
            pan4.Hide();
            pan1Page.Hide();
            pan2page.Show();
            pan3page.Hide();
            pan4page.Hide();

            //t.Enabled = false;


        }

        private void button3_Click(object sender, EventArgs e) // open pan3page
        {
            pan1.Hide();
            pan2.Hide();
            pan3.Show(); // Test Ap
            pan4.Hide();
            pan1Page.Hide();
            pan2page.Hide();
            pan3page.Show();
            pan4page.Hide();


            //t.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e) // open pan4page
        {
            pan1.Hide();
            pan2.Hide();
            pan3.Hide();
            pan4.Show(); // Manage Data
            pan1Page.Hide();
            pan2page.Hide();
            pan3page.Hide();
            pan4page.Show();

            //t.Enabled = true;
        }

        private void pictureBox6_Click(object sender, EventArgs e) // logout
        {
            con.Close();
            mForm.Show();
            this.Close();
        }

        private void admin_FormClosing(object sender, FormClosingEventArgs e) // form close
        {
            con.Close();
        }

        private void button5_Click(object sender, EventArgs e) // update AP records in pan1page
        {
            da.Update(ds);
        }

        private void button6_Click(object sender, EventArgs e) // read all AP to put in pan1page  and pan3page
        {
            ShowInfi(dataGridView2);
            ShowInfi(dataGridView4);
        }

        // Show All Information About All Read Accesses Point
        void ShowInfi(DataGridView DGV)
        {
            #region Read All Access Points

            APs.Clear();
            DGV.Rows.Clear();
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
                        //Console.WriteLine("Found network with SSID {0}.",
                        //    System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString());
                        //Console.WriteLine("Signal: {0}%.", network.linkQuality);
                        //Console.WriteLine("BSS Type: {0}.", network.dot11BssType);
                        //Console.WriteLine("MAC: {0}.", tMac);
                        //Console.WriteLine("RSSID:{0}", rss.ToString());
                        //Culculate the distance
                        double p1m = -18.5;   //1.8
                        double pr = rss;
                        double p1m_pr = p1m - pr;
                        double n = 4.8;   //4.8
                        double pwor = (p1m_pr / (10 * n));
                        double d = Math.Pow(10, pwor);
                        // Display The Sorted List In DataGrad View
                        DGV.ColumnCount = 6;
                        DGV.Columns[0].Name = "SSID";
                        DGV.Columns[1].Name = "Quality";
                        DGV.Columns[2].Name = "BSS-type";
                        DGV.Columns[3].Name = "MAC";
                        DGV.Columns[4].Name = "RSSID";
                        DGV.Columns[5].Name = "Distance";
                        string[] row = new string[] { System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString(),
                            network.linkQuality.ToString(),
                            network.dot11BssType.ToString(),
                            tMac,
                            rss.ToString(),
                            d.ToString() };
                        DGV.Rows.Add(row);


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

        private void button8_Click(object sender, EventArgs e) // draw AP coordinat system
        {
            // draw
            System.Drawing.Graphics gf;
            gf = pictureBox7.CreateGraphics();
            gf.Clear(Color.White);
            Pen myPen = new Pen(System.Drawing.Color.Green, 4);
            for (var i = 0; i < 1000; i += 100) // drow y
            {
                gf.DrawLine(myPen, i, 0, i, 500);
            }
                
            myPen = new Pen(System.Drawing.Color.Green, 5);
            for (var i = 0; i < 500; i += 100)
            {
                gf.DrawLine(myPen, 0, i, 1000, i);
            }

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
            dr.Close();

            // set AP position in coordinat system
            Pen pen;
            int rate = 4; // 1m in real place = 4m in coordinat
            int r, g, b;
            Random rand = new Random();
           
            foreach (var item in myPoints)
            {
                //pen = new Pen(Brushes.Yellow, w);
                r = rand.Next(0, 255);
                g = 0;
                b = rand.Next(0, 255);
                pen = new Pen(Color.FromArgb(r,g,b),3);
                gf.DrawRectangle(pen, new Rectangle(Convert.ToInt32(item.x - item.maxrssid) * rate, Convert.ToInt32(item.y - item.maxrssid) * rate,
                    Convert.ToInt32(2 * item.maxrssid) * rate, Convert.ToInt32(2 * item.maxrssid) * rate));
                //w++;
            }
            
        }

        private void button7_Click(object sender, EventArgs e) // update records in pan2page
        {
            da.Update(ds);
        }

        private void button10_Click(object sender, EventArgs e) // calculate the position in x,y and z
        {
            Result res = CalcPosition(APs);
            if(res != null)
            {
                textBox1.Text = res.x.ToString();
                textBox2.Text = res.y.ToString();
                textBox3.Text = res.z.ToString();
            }
           

        }

        // Calculat the position
        Result CalcPosition(List<rAP> list)
        {
            // Show All Information About All Read Accesses Point
            ShowInfi(dataGridView4);
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
                // Display The Matched List In DataGrad View
                dataGridView5.Rows.Clear();
                dataGridView5.ColumnCount = 4;
                dataGridView5.Columns[0].Name = "SSID";
                dataGridView5.Columns[1].Name = "mac";
                dataGridView5.Columns[2].Name = "name";
                dataGridView5.Columns[3].Name = "distance";
                foreach (APmatched ap in listMatched)
                {
                    string[] row = new string[] { ap.SSID,ap.MAC, ap.name, ap.distance.ToString() };
                    dataGridView5.Rows.Add(row);

                }
                // Variable Initiation
                // Possible My ListMatched Stor 5 Access Point In This Case I Select Three Only EX: a0, a1, b0 from sorted matched point list
                var r1 = listMatched[0].distance;
                //var x1 = listMatched[0].x;
                //var y1 = listMatched[0].y;
                double x1 = 0;
                double y1 = 0;

                int aPositionInCoodinat = listMatched[0].name[1]- 48;
                int aNumber = listMatched[0].name[2] - 48;

                var r2 = listMatched[1].distance;
                //var d = listMatched[1].x;
                //var y2 = listMatched[1].y;
                double d = 12;
                double y2 = 0;

                // set initial values
                double r3 = 0;
                double i = 0;
                double j = 0;
                int bPositionInCoodinat = 0;
                int bNumber = 0;
                if (!listMatched[2].name.Contains("a"))
                {
                    r3 = listMatched[2].distance;   //
                    //i = listMatched[2].x;
                    //j = listMatched[2].y;
                    i = 7;
                    j = 4;

                    bPositionInCoodinat = listMatched[2].name[1] - 48;
                    bNumber = listMatched[2].name[2] - 48;
                }
                else if (listMatched.Count >= 4 && !listMatched[2].name.Contains("a"))
                {
                    r3 = listMatched[3].distance;
                    //i = listMatched[3].x;
                    //j = listMatched[3].y;
                    i = 7;
                    j = 4;

                    bPositionInCoodinat = listMatched[0].name[1] - 48;
                    bNumber = listMatched[2].name[2] - 48;
                }


                result.x = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(d, 2)) / (2 * d);
                result.y = ((Math.Pow(r1, 2) - Math.Pow(r3, 2) + Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * j)) - ((i * result.x) / j);
                result.z = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(result.x, 2) + Math.Pow(result.y, 2));

                result.x += 25 * Convert.ToInt32(aPositionInCoodinat);
                result.y += 25 * Convert.ToInt32(bPositionInCoodinat);
                //if (bNumber < aNumber)
                //{
                  //  result.y += 25;
                //}
                

                //----------------------------------- Draw
                System.Drawing.Graphics gf;
                gf = pictureBox8.CreateGraphics();
                Pen myPen = new Pen(System.Drawing.Color.Green, 3);
                for (var k = 0; k < 1000; k+= 100) // drow y
                {
                    gf.DrawLine(myPen, k, 0, k, 500);
                }

                myPen = new Pen(System.Drawing.Color.Green, 3);
                for (var k = 0; k < 500; k += 100)
                {
                    gf.DrawLine(myPen, 0, k, 1000, k);
                }

                // set location in coordinat
                myPen = new Pen(System.Drawing.Color.Red, 4);
                int rate = 4;
                gf.DrawLine(myPen, float.Parse((result.x * rate).ToString()), float.Parse((result.y * rate).ToString()),
                    float.Parse((result.x * rate).ToString()), float.Parse((result.y * rate).ToString())+1);

                
            }
            else { MessageBox.Show("not enouph, at lest 3 access point Matched with access point stored in my database"); }

            // return the result x,y and z
            return result;
            

            
        }

        private void button9_Click(object sender, EventArgs e) // refresh button in pan3page the readed AP in area
        {
            ShowInfi(dataGridView4);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void t_tick(Object sender, EventArgs e)
        {
            if ( txt_id.Text != "")
            {

                //Result res = CalcPosition(APs);
                //if(res != null)
                //{
                //    textBox1.Text = res.x.ToString();
                //    textBox2.Text = res.y.ToString();
                //    textBox3.Text = res.z.ToString();
                //}



                

                MySqlCommand cmd = new MySqlCommand("p2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //if(Int32.Parse(textBox5.Text.ToString()) > 5 )
                   
                //cmd.Parameters.AddWithValue("new_x", Int32.Parse(textBox5.Text.ToString()));
                cmd.Parameters.AddWithValue("id", Int32.Parse(txt_id.Text.ToString()));
                MySqlDataReader dr = cmd.ExecuteReader();
                /*DataTable dt = new DataTable();
                dt.Load(dr);*/
                dr.Read();
                pictureBox9.Image = Image.FromFile( @"Photo\" + dr[0].ToString());
                 
                str = dr[1].ToString();

                dr.Close();
                //con.Close();

                //str = "وضع تصميم المتحف المعماري الفرنسي مارسيل دورنون عام 1897 ليقام بالمنقطة الشمالية لميدان التحرير «الإسماعيلية سابقاً» على امتداد ثكنات الجيش البريطاني بالقاهرة عند قصر النيل، واحتفل بوضع حجر الأساس في 1 أبريل 1897 في حضور الخديوي عباس حلمي الثاني ورئيس مجلس النظار «الوزراء» وكل أعضاء وزارته، وتم الانتهاء من المشروع علي يد الألماني هرمان جرابو.[4][6][7] في نوفمبر 1903 عينت مصلحة الآثار المهندس المعماري الإيطالي إليساندرو بارازنتي الذي تسلم مفاتيح المتحف منذ التاسع من مارس 1902 ونقل المجموعات الأثرية من قصر الخديوي إسماعيل بالجيزة إلى المتحف الجديد وهي العملية التي استُخدم خلالها خمسة آلاف عربة خشبية، أما الآثار الضخمة فقد تم نقلها على قطارين سيراً ذهاباً وعودة نحو تسع عشرة مرة بين الجيزة وقصر النيل. وقد حملت الشحنة الأولى نحو ثمانية وأربعين تابوتاً حجرياً، تزن ما يزيد على ألف طن إجمالاً. إلا أن عملية النقل قد شابتها الفوضى بعض الوقت. وتم الانتهاء من عمليات النقل في 13 يوليو 1902، كما تم نقل ضريح مارييت إلى حديقة المتحف، تلبيةً لوصيته التي عبر فيها عن رغبته في أن يستقر جثمانه بحديقة المتحف مع الآثار التي قضى وقتا طويلاً في تجميعها خلال حيات";
                string str1 = "        ";
                label7.Text = "       ";
                
                for (int j = 0 , i = 0; i < str.Length; i++)
                {
                    j++;
                    //label7.Text += str[i];
                    str1 += str[i];
                    if (j > 100 && str[i] == ' ')
                    {
                        j = 0;
                        str1 = str1 + "\n";
                    }

                }

                label7.Text = str1;
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {
            

        }

        private void button11_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Images Files | *.JPG; *.PNG; GIF";
            //openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.FileName = @"C:\Users\Hossam\Desktop\New folder (6)\new";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox9.Image = Image.FromFile(openFileDialog1.FileName);
            }


            for (int i = openFileDialog1.FileName.Length - 1; i > 0; i--)
            {
                if (openFileDialog1.FileName[i] == '\\')
                    break;
                path += openFileDialog1.FileName[i];
            }

            for (int i = 0, j = path.Length - 1; i < path.Length; i++, j--)
                img_name += path[j];

            MessageBox.Show(img_name);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE details SET img_path = @path WHERE location_id = @id ";
                cmd.Parameters.AddWithValue("@path", img_name);
                cmd.Parameters.AddWithValue("@id", txt_id.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Update Successfuly");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txt_id_TextChanged(object sender, EventArgs e)
        {
            t_tick(sender, e);
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
