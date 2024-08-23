using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGV
{
    public partial class Form1 : Form
    {
        string portname;
        String DebugHex;
        public Form1()
        {
            InitializeComponent();
           // readnotepad();
            
            serialPort1.PortName = portname;
            serialPort1.BaudRate = 9600;

            try
            {
                serialPort1.Open();
                label1.Text = "COM CONNECTED";
                label1.ForeColor = System.Drawing.Color.Black;
            }
            catch 
            {
                label1.Text = "COM NOT CONNECT";
                label1.ForeColor = System.Drawing.Color.Red;
            }

            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 0;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Show();
            //panel1.Visible = false;
            Thread.Sleep(3000);
            // this.Close();
            panel1.Visible = true;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            mtc mtc = new mtc();
            mtc.Show();
           // this.Close();
            Form1 form = new Form1();
            form.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {


            if (serialPort1.IsOpen == false)
            {

                try
                {
                    serialPort1.Open();
                    label1.Text = "COM CONNECTED";
                    label1.ForeColor = System.Drawing.Color.Black;
                }
                catch
                {
                    label1.Text = "COM NOT CONNECT";
                    label1.ForeColor = System.Drawing.Color.Red;
                }

                 
            }
            serialPort1.WriteLine("STRT#0000000000");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("RST#0000000000");
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
           // String datain = serialPort1.ReadLine();

            char Value = (char)serialPort1.ReadByte();

            DebugHex = DebugHex + Value.ToString();
            //  if (before != Value)
            //      Console.WriteLine(DebugHex);
            if (Value == '#')
            {
                int panjang = DebugHex.Length;
                //  if(panjang>0)

                string[] subs = DebugHex.Split('#');
                label1.Text = subs[0];
                if (subs[0] == "START")
                {
                    label1.Text = "AGV RUN FORWARD";
                }
                else if (subs[0] == "LOADING")
                {
                    label1.Text = "BOX LOADING";
                }
                else if (subs[0] == "BACK")
                {
                    label1.Text = "AGV RUN BACKWARD";
                }
                else if (subs[0] == "BATT")
                {
                    //label1.Text = "AGV RUN BACKWARD";

                    progressBar1.Value = Int32.Parse(subs[1]);
                }
                else
                {
                    string[] subs2 = DebugHex.Split('/');
                    //label1.Text = "AGV RUN BACKWARD";
                    String disp = "LINE " + subs[0] + "  COND :" + subs[1];
                    
                    label1.Text = disp;
                }
                //   countCrimp = Convert.ToInt32(subs[0]);
                DebugHex = "";
            }

            /*
            if (words[0] == "START")
            {
                label1.Text = "AGV RUN FORWARD";
            }
            else if (words[0] == "LOADING")
            {
                label1.Text = "BOX LOADING";
            }
            else if (words[0] == "BACK")
            {
                label1.Text = "AGV RUN BACKWARD";
            }
            else if (words[0] == "BATT")
            {
                //label1.Text = "AGV RUN BACKWARD";

                progressBar1.Value = Int32.Parse(words[1]);
            }
            */

        }

        private void readnotepad()
        {

            string[] lines = System.IO.File.ReadAllLines(@"COM_SELECT.txt");

            Console.WriteLine(lines[0]);
            portname = lines[0];


        }

        private void button6_Click(object sender, EventArgs e)
        {
            Shutdown();
        }

        void Shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown",
                                               mboShutdownParams, null);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
