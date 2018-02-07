using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleWifi;
using NativeWifi;
using System.Net.NetworkInformation;

namespace WifiProject
{

    public partial class Form1 : Form
    {
        // Global Variable initiation
        Result result = new Result();  // Location in (x, y, z)
        List<rAP> APs = new List<rAP>();    // List For read All Access Point In This Area
        List<myPoint> myPoints = new List<myPoint>();   // List For My Access Point Information
        List<APmatched> listMatched = new List<APmatched>();    // List For Matched Access Point


        // Initialization Of Access Point Position From Database
        void init()
        {
            myPoints.Add(new myPoint(0, 0, "54BE537457F4", "mostafa", "a0"));
            myPoints.Add(new myPoint(14, 0, "9CC17283712C", "open", "a1"));
            myPoints.Add(new myPoint(4, 5, "20F3A399D904", "tjs", "b0"));
        }

        // Show All Information About All Read Accesses Point
        void ShowInfi () {
            #region Read All Access Points

            APs.Clear();
            dataGridView2.Rows.Clear();
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
                        dataGridView2.ColumnCount = 6;
                        dataGridView2.Columns[0].Name = "SSID";
                        dataGridView2.Columns[1].Name = "Quality";
                        dataGridView2.Columns[2].Name = "BSS-type";
                        dataGridView2.Columns[3].Name = "MAC";
                        dataGridView2.Columns[4].Name = "RSSID";
                        dataGridView2.Columns[5].Name = "Distance";
                        string[] row = new string[] { System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString(),
                            network.linkQuality.ToString(),
                            network.dot11BssType.ToString(),
                            tMac,
                            rss.ToString(),
                            d.ToString() };
                        dataGridView2.Rows.Add(row);

                        
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

        // Calculat the position
        void CalcPosition(List<rAP> list)
        {
            // Sort The List Of Access Point Based in RSSID
            list.Sort();

            // Display The Sorted List In DataGrad View
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "SSID";
            dataGridView1.Columns[1].Name = "Quality";
            dataGridView1.Columns[2].Name = "BSS-type";
            dataGridView1.Columns[3].Name = "MAC";
            dataGridView1.Columns[4].Name = "RSSID";
            dataGridView1.Columns[5].Name = "Distance";
            foreach (rAP ap in list)
            {
                string[] row = new string[] { ap.SSID, ap.Quality.ToString(), ap.BSS, ap.MAC, ap.RSSID.ToString(), ap.distance.ToString() };
                dataGridView1.Rows.Add(row);

            }
            

            // Match Access Point With My Point, That Sorted in Database
            if(list.Count >= 3)
            {
                listMatched.Clear();
                foreach (rAP ap in list)
                {
                    foreach(myPoint myap in myPoints) 
                    {
                        if(ap.MAC == myap.mac)
                        {
                            listMatched.Add(new APmatched(ap.SSID, myap.name, ap.MAC, ap.distance, myap.x, myap.y, myap.rssidat1m, myap.noise));
                        }
                    }
                    
                }
            }
            else { MessageBox.Show("not enouph, at lest 3 access point to do this"); }

            // Calculate the Result
            if (listMatched.Count >= 3)
            {
                // Sort The Matched List
                listMatched.Sort();
                // Display The Matched List In DataGrad View
                dataGridView3.Rows.Clear();
                dataGridView3.ColumnCount = 3;
                dataGridView3.Columns[0].Name = "SSID";
                dataGridView3.Columns[1].Name = "name";
                dataGridView3.Columns[2].Name = "distance";
                foreach (APmatched ap in listMatched)
                {
                    string[] row = new string[] { ap.SSID, ap.name, ap.distance.ToString() };
                    dataGridView3.Rows.Add(row);
                    
                }
                // Variable Initiation
                // Possible My ListMatched Stor 5 Access Point In This Case I Select Three Only EX: a0, a1, b0
                var r1 = listMatched[0].distance;
                var x1 = listMatched[0].x;
                var y1 = listMatched[0].y;

                var r2 = listMatched[1].distance;
                var d = listMatched[1].x;
                var y2 = listMatched[1].y;

                double r3 =0;
                double i = 0;
                double j = 0;
                if(!listMatched[2].name.Contains("a"))
                {
                    r3 = listMatched[2].distance - 5;   //
                    i = listMatched[2].x;
                    j = listMatched[2].y;
                }
                else if (listMatched.Count >= 4 && !listMatched[2].name.Contains("a"))
                {
                    r3 = listMatched[3].distance;
                    i = listMatched[3].x;
                    j = listMatched[3].y;
                }
                

                result.x = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(d, 2)) / (2 * d);
                result.y = ((Math.Pow(r1, 2) - Math.Pow(r3, 2) + Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * j)) - ((i * result.x) / j);
                result.z = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(result.x, 2) + Math.Pow(result.y, 2));

            }
            else { MessageBox.Show("not enouph, at lest 3 access point Matched with access point stored in my database"); }

            // Display The Result In Text View
            textBox1.Text = result.x.ToString();
            textBox2.Text = result.y.ToString();
            textBox3.Text = result.z.ToString();

        }

        

        private static Wifi wifi;

        public Form1()
        {
            InitializeComponent();
            init();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ShowInfi();
        }

        private void but_reload_Click(object sender, EventArgs e)
        {
            ShowInfi();
        }

        private void but_connect_Click(object sender, EventArgs e)
        {
            CalcPosition(APs);

        }
            

    }

    class myPoint
    {
        public double x;
        public double y;
        public string mac;
        public string ssid;
        public string name;
        public double rssidat1m;
        public double maxrssid;
        public double noise;

        public myPoint(double x, double y, string mac, string ssid, string name)
        {
            this.x = x;
            this.y = y;
            this.mac = mac;
            this.ssid = ssid;
            this.name = name;
        }
        public myPoint(double x, double y, string mac, string ssid, string name, double rssidat1m, double maxrssid, double noise)
        {
            this.x = x;
            this.y = y;
            this.mac = mac;
            this.ssid = ssid;
            this.name = name;
            this.rssidat1m = rssidat1m;
            this.maxrssid = maxrssid;
            this.noise = noise;

        }

    }
    class rAP : IComparable<rAP>
    {
        public string SSID;
        public string MAC;
        public double Quality;
        public string BSS;
        public double RSSID;
        public double distance;

        public rAP(string SSID, string MAC, double Quality, string BSS, double RSSID, double distance)
        {
            this.SSID = SSID;
            this.MAC = MAC;
            this.Quality = Quality;
            this.BSS = BSS;
            this.RSSID = RSSID;
            this.distance = distance;
        }

        public int CompareTo(rAP y)
        {
            return -this.RSSID.CompareTo(y.RSSID); // to store it from big to small
        }
    }
    class APmatched : IComparable<APmatched>
    {
        public string SSID;
        public string name;
        public string MAC;
        public double distance;
        public double x;
        public double y;
        public double rssidat1m;
        public double noise;

        public APmatched(string SSID, string name, string MAC, double distance, double x, double y, double rssidat1m, double noise)
        {
            this.SSID = SSID;
            this.name = name;
            this.MAC = MAC;
            this.distance = distance;
            this.x = x;
            this.y = y;
            this.rssidat1m = rssidat1m;
            this.noise = noise;
        }

        public int CompareTo(APmatched o)
        {
            return this.name.CompareTo(o.name);
        }
    }
    class Result
    {
        public double x;
        public double y;
        public double z;

        public Result() { }
        public Result(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }

}
