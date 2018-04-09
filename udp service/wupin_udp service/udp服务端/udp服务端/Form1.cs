using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;



namespace udp服务端
{


    public partial class Form1 : Form
    {

        static Socket server;
        
        public Form1()
        {

            InitializeComponent();
            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                server.Bind(new IPEndPoint(IPAddress.Parse("172.16.112.128"), 6666));//绑定端口号和IP
                //this.textBox1.Text = "服务端已经开启";
                Thread t = new Thread(ReciveMsg);//开启接收消息线程
                t.Start();

            }
            catch { }
        }
        /// <summary>
        /// 服务器端不停的接收客户端发来的消息
        /// </summary>
        /// <param name="o"></param>
        void ReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[26];
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                string message = byteToHexStr(buffer);
                string check = message.Substring(0, 2);//取出甲烷的数值
                string check2 = message.Substring(44, 4);//取出甲烷的数值
                if (check.Equals("14") && check2.Equals("55D5"))
                {
                    //氧气
                    string o2 = message.Substring(24, 4);//取出甲烷的数值
                    double o2d = double.Parse(o2); //string转double方法
                    //Double f1 = Convert.ToInt32(temp1, 10) / 1.0;  //string转double方法
                    Double o2num = 0.0;
                    if (o2d > 200)
                    {
                        o2num = (o2d - 200) * 25 / 800; //string转double方法
                    
                    }
                    if (o2num < 17)
                    {
                        textBox2.ForeColor = Color.Red;
                    }
                    else {
                        textBox2.ForeColor = SystemColors.Highlight;
                    }
                    textBox2.Text = String.Format("{0:0.0}", o2num) + "%";//自定义精确到小数点后两位
                    //一氧化碳
                    string co = message.Substring(8, 4);//取出甲烷的数值
                    double cod = double.Parse(co); //string转double方法
                    Double conum = 0.0;
                    if (cod > 400)
                    {
                        conum = (cod - 400) * 1000 / 1600; //string转double方法
                        conum = conum + 2;
                    }
                    if (conum > 24)
                    {
                        textBox1.ForeColor = Color.Red;
                    }
                    else
                    {
                        textBox1.ForeColor = SystemColors.Highlight;
                    }
                    textBox1.Text = String.Format("{0:0}", conum) + "PPM";
                    //二氧化碳
                    string co2 = message.Substring(28, 4);//取出甲烷的数值
                    double co2d = double.Parse(co2); //string转double方法
                    Double co2num = 0.0;
                    if (co2d > 200)
                    {
                        co2num = (co2d - 200) * 5 / 800; //string转double方法
                    }
                    if (co2num > 1)
                    {
                        textBox5.ForeColor = Color.Red;
                    }
                    else
                    {
                        textBox5.ForeColor = SystemColors.Highlight;
                    }
                    textBox5.Text = String.Format("{0:0.00}", co2num) + "%";
                    //温度
                    string wd = message.Substring(32, 4);//取出甲烷的数值
                    double wdd = double.Parse(wd); //string转double方法
                    Double wdnum = 0.0;
                    if (wdd > 200)
                    {
                        wdnum = (wdd - 200) * 100 / 800; //string转double方法
                        wdnum = wdnum - 0.2;
                    }
                    textBox6.Text = String.Format("{0:0.0}", wdnum) + "℃";
                    //湿度
                    string sd = message.Substring(36, 4);//取出甲烷的数值
                    double sdd = double.Parse(sd); //string转double方法
                    Double sdnum = 0.0;
                    if (sdd > 200)
                    {
                        sdnum = (sdd - 200) * 100 / 800; //string转double方法
                        sdnum = sdnum - 0.2;
                    }
                    textBox4.Text = String.Format("{0:0.0}", sdnum) + "%RH";
                    //大气压
                    string dqy = message.Substring(12, 4);//取出甲烷的数值
                    double dqyd = double.Parse(dqy); //string转double方法
                    Double dqynum = 0.0;
                    if (dqyd > 400)
                    {
                        dqynum = (dqyd - 400) * 5 / 1600; //string转double方法
                        //dqynum = dqynum - 25;
                    }
                    textBox3.Text = String.Format("{0:0.00}", dqynum) + "Kpa";
                    //WriteLogs("log", "接收", dqy);//调用WriteLogs创建日志
                }
               

                //WriteLogs("log", "接收", message);//调用WriteLogs创建日志
               

            }
        }

        public static void WriteLogs(string fileName, string type, string content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + fileName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + type + "-->" + content);
                    //sw.WriteLine("");
                    sw.Close();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }



        // 16进制字符串转字节数组   格式为 string sendMessage = "00 01 00 00 00 06 FF 05 00 64 00 00";
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }


        // 字节数组转16进制字符串   
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");//ToString("X2") 为C#中的字符串格式控制符
                }
            }
            return returnStr;
        }
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://zglakj.com");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }


    }
}
                   

