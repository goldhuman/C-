﻿using System;
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

namespace tcp服务端
{
    

    public partial class Form1 : Form
    {
        bool defeat = false;

        private void button14_Click(object sender, EventArgs e)//联防
        {
            Send("06 05 00 00 00 00 CC 7D");
            Thread.Sleep(500);
            Send("07 05 00 00 00 00 CD AC");
            if (button14.Text == "联防")
            {

                defeat = true;
                this.button14.Text = "撤防";
            }

            else if (button14.Text == "撤防")
            {

                defeat = false;
                this.button14.Text = "联防";
            }
        }

            
      

        public Form1()
        {
            InitializeComponent();
            /*textBox2.ForeColor = Color.Green;
            textBox2.ReadOnly = true;*/
            

            try
            {
                //点击开始监听时 在服务端创建一个负责监听IP和端口号的Socket
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Any;
                //创建对象端口
                IPEndPoint point = new IPEndPoint(ip, 10000);

                socketWatch.Bind(point);//绑定端口号
                //ShowMsg("一氧化碳浓度");
                socketWatch.Listen(10);//设置监听

                //创建监听线程
                Thread thread = new Thread(Listen);
                thread.IsBackground = true;
                thread.Start(socketWatch);

                Thread thread2 = new Thread(repeatsend);
                thread2.IsBackground = true;
                thread2.Start();
            }
            catch { }
        }
        //传感器接收
        void repeatsend()
        {
            try
            {
                while (true)
                {
                   
                    Thread.Sleep(500);//等待500毫秒
                    Send("03 03 00 00 00 02 C5 E9");
                    Thread.Sleep(500);//等待500毫秒
                    Send("01 03 00 05 00 01 94 0B");
                    Thread.Sleep(500);//等待500毫秒
                    Send("04 03 00 06 00 01 64 5E");
                    Thread.Sleep(500);//等待500毫秒
                    Send("02 03 00 06 00 01 64 38");
                    //Thread.Sleep(500);//等待500毫秒
                    //Send("03 03 00 00 00 02 C5 E9");

                    if (dandeng1) {
                        Thread.Sleep(500);
                        Send("08 05 00 00 FF 00 8C A3");//单灯打开
                        dandeng1 = false;
                    }

                    else if (dandeng2)
                    {
                        Thread.Sleep(500);
                        Send("08 05 00 00 00 00 CD 53");//单灯关闭
                        dandeng2 = false;
                    }
                    else if (baojing1)
                    {
                        Thread.Sleep(500);
                        Send("06 05 00 00 FF 00 8D 8D");//报警打开
                        baojing1 = false;
                    }
                    else if (baojing2)
                    {
                        Thread.Sleep(500);
                        Send("06 05 00 00 00 00 CC 7D");//报警关闭
                        baojing2 = false;
                    }
                    else if (fengji1)
                    {
                        Thread.Sleep(500);
                        Send("07 05 00 00 FF 00 8C 5C");//风机打开
                        fengji1 = false;
                    }
                    else if (fengji2)
                    {
                        Thread.Sleep(500);
                        Send("07 05 00 00 00 00 CD AC");//风机关闭
                        fengji2 = false;
                    }
                    else if (hongdeng1)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 03 01 00 3D DE");//红灯打开
                        hongdeng1 = false;
                    }
                    else if (hongdeng2)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 03 01 00 3D DE");//红灯关闭
                        hongdeng2 = false;
                    }
                    else if (huangdeng1)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 01 01 00 9C 1E");//黄灯打开
                        huangdeng1 = false;
                    }
                    else if (huangdeng2)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 01 00 00 9D 8E");//黄灯关闭
                        huangdeng2 = false;
                    }
                    else if (baideng11)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 02 01 00 6C 1E");//白灯1打开
                        baideng11 = false;
                    }
                    else if (baideng12)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 02 00 00 6D 8E");//白灯1关闭
                        baideng12 = false;
                    }
                    else if (baideng21)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 00 01 00 CD DE");//白灯2打开
                        baideng21 = false;
                    }
                    else if (baideng22)
                    {
                        Thread.Sleep(500);
                        Send("05 05 00 00 00 00 CC 4E");//白灯2关闭
                        baideng22 = false;
                    }

                }
            }
            catch { }
        }
        /// <summary>
        /// 等待客户端的连接 并且创建与之通信的Socket
        /// </summary>
        Socket socketSend;
        void Listen(object o)
        {
            try
            {
                Socket socketWatch = o as Socket;
                while (true)
                {
                    socketSend = socketWatch.Accept();//等待接收客户端连接
                    /*ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功!");
                    开启一个新线程，执行接收消息方法
                    Thread r_thread = new Thread(Received);
                    r_thread.IsBackground = true;
                    r_thread.Start(socketSend);*/
                    Thread r_thread2 = new Thread(Received);
                    r_thread2.IsBackground = true;
                    r_thread2.Start();
                }
            }
            catch { }
        }
        /// <summary>
        /// 服务器端不停的接收客户端发来的消息
        /// </summary>
        /// <param name="o"></param>
 
         bool sendflag = true;
         bool baojing1 = false;
         bool baojing2 = false;
         bool fengji1 = false;
         bool fengji2 = false;
         bool dandeng1 = false;
         bool dandeng2 = false;
         bool hongdeng1 = false;
         bool hongdeng2 = false;
         bool huangdeng1 = false;
         bool huangdeng2 = false;
         bool baideng11 = false;
         bool baideng12 = false;
         bool baideng21 = false;
         bool baideng22 = false;
      
        void Received(object o)
        {
            try
            {
                while (true)
                {
                    //客户端连接服务器成功后，服务器接收客户端发送的消息
                    byte[] buffer = new byte[9];
                    //实际接收到的有效字节数
                    int len = socketSend.Receive(buffer);
                    if (len == 0)
                      {
                        break;
                      }
    
                    //string str = Encoding.UTF8.GetString(buffer, 0, len);
                    string str = byteToHexStr(buffer);//接收到的字符串，待解析
                    string temp= str.Substring(0, 16);

                    WriteLogs("log", "接收", str);//调用WriteLogs创建日志

                    if (temp.Equals("07050000FF008C5C"))
                      {
                        pictureBox10.Image = imageList6.Images[0];//风机打开图片显示
                    }
                    else if (temp.Equals("070500000000CDAC"))
                          {
                        pictureBox10.Image = imageList8.Images[0];//风机关闭图片显示
                    }
                    else if (temp.Equals("06050000FF008D8D"))
                          {
                        pictureBox1.Image = imageList1.Images[0];//报警器打开图片显示
                    }
                    else if (temp.Equals("060500000000CC7D"))
                          {
                        pictureBox1.Image = imageList7.Images[0];//报警器关闭图片显示
                    }
                    else if (temp.Equals("08050000FF008CA3"))
                          {
                        pictureBox9.Image = imageList5.Images[0];//单灯打开图片显示
                    }
                    else if (temp.Equals("080500000000CD53"))
                          {
                        pictureBox9.Image = imageList2.Images[0];//单灯关闭图片显示
                    }
                    else if (temp.Equals("0505000301003DDE"))
                          {
                        pictureBox7.Image = imageList3.Images[0];//红灯打开图片显示
                    }
                    else if (temp.Equals("0505000300003C4E"))
                          {
                        pictureBox7.Image = imageList2.Images[0];//红灯关闭图片显示
                    }
                    else if (temp.Equals("0505000101009C1E"))
                          {
                        pictureBox8.Image = imageList4.Images[0];//黄灯打开图片显示
                    }
                    else if (temp.Equals("0505000100009D8E"))
                          {
                        pictureBox7.Image = imageList2.Images[0];//黄灯关闭图片显示
                    }
                    else if (temp.Equals("0505000201006C1E"))
                          {
                        pictureBox11.Image = imageList5.Images[0];//白灯1打开图片显示
                    }
                    else if (temp.Equals("0505000200006D8E"))
                          {
                        pictureBox11.Image = imageList2.Images[0];//白灯1关闭图片显示
                    }
                    else if (temp.Equals("050500000100CDDE"))
                          {
                        pictureBox12.Image = imageList5.Images[0];//白灯2打开图片显示
                    }
                    else if (temp.Equals("050500000000CC4E"))
                          {
                        pictureBox12.Image = imageList2.Images[0];//白灯2关闭图片显示
                    }

                    int start = 1, length = 2;
                    String fileName = str.Substring(start - 1, length);
                    if (fileName.Equals("03"))
                      {
                        if (str.Length > 0)
                          {
                    
                            int start1 = 11, length1 = 4;//温度值表示
                            String temp1 = str.Substring(start1 - 1, length1);//将十六进制数转化为十进制数，精确到小数点后表示
                            //string hexString1 = temp1;
                            String temp11 = str.Substring(14, 2);
                            String temp12 = str.Substring(16, 2);

                        if(!temp11.Equals("00")&&!temp12.Equals("00"))//若数据传输中发生丢包或乱码，显示数值不会大幅变动。
                          {
                                Double f1 = Convert.ToInt32(temp1, 16) / 10.0;  
                                textBox1.Text = f1.ToString() + "℃";
                                if (defeat)
                                  {
                                try
                                    {
                                    if (f1 >= 35)
                                      {
                                        Send("06 05 00 00 FF 00 8D 8D");//报警器打开
                                       // Send("07 05 00 00 FF 00 8C 5C");//风机打开
                                      }
                                    }

                                catch { }
  
                          }
                        }

                          
                            int start2 = 7, length2 = 4;//湿度值表示
                            String temp2 = str.Substring(start2 - 1, length2);//将十六进制数转化为十进制数，精确到小数点后表示
                            String temp21 = str.Substring(14, 2);
                            String temp22 = str.Substring(16, 2);

                        if (!temp21.Equals("00") && !temp22.Equals("00"))//若数据传输中发生丢包或乱码，显示数值不会大幅变动。
                          {
                                Double f2 = Convert.ToInt32(temp2, 16) / 10.0;
                                //textBox1.Text = temp;
                                textBox2.Text = f2.ToString() + "%";
                          }
                        }
                     }
                    else if (fileName.Equals("01"))
                           { 
                             int start3 = 7, length3 = 4;//CO2值表示
                             String temp3 = str.Substring(start3 - 1, length3);//将十六进制数转化为十进制数，精确到小数点后表示
                             //string hexString2 = temp2;
                             String temp31 = str.Substring(12, 2);
                             String temp32 = str.Substring(14, 2);

                             if (!temp31.Equals("00") && !temp32.Equals("00"))//若数据传输中发生丢包或乱码，显示数值不会大幅变动。
                               {
                                  Double f3 = Convert.ToInt32(temp3, 16);
                                  //textBox1.Text = temp;
                                  textBox3.Text = f3.ToString() + " PPM";
                                 if (defeat)
                                   {
                                    try
                                       {
                                        if (f3 >= 500)
                                          {
                                            Send("06 05 00 00 FF 00 8D 8D");//报警器打开
                                           }
                                        }
                                    catch { }
                                }
                            }
                      }

                     else if (fileName.Equals("04"))
                            {
                            int start4 = 7, length4 = 4;//O2值表示
                            String temp4 = str.Substring(start4 - 1, length4);//将十六进制数转化为十进制数，精确到小数点后表示
                            //string hexString2 = temp2;
                            String temp41 = str.Substring(12, 2);
                            String temp42 = str.Substring(14, 2);

                        if (!temp41.Equals("00") && !temp42.Equals("00"))//若数据传输中发生丢包或乱码，显示数值不会大幅变动。
                           {
                            Double f4 = Convert.ToInt32(temp4, 16) / 10.0;
                            //textBox1.Text = temp;
                            textBox4.Text = f4.ToString() + " PPM";
                            if (defeat)
                              {
                                try
                                   {
                                    if (f4 >= 1800)
                                      {
                                        Send("06 05 00 00 FF 00 8D 8D");//报警器打开
                                      }
                                    }
                                 catch { }
                            }
                        }
                    }
                    else if (fileName.Equals("02"))
                           {
                            int start5 = 7, length5 = 4;//CO值表示
                            String temp5 = str.Substring(start5 - 1, length5);//将十六进制数转化为十进制数，精确到小数点后表示
                            //string hexString2 = temp2;
                            String temp51 = str.Substring(12, 2);
                            String temp52 = str.Substring(14, 2);
                         if (!temp51.Equals("00") && !temp52.Equals("00"))//若数据传输中发生丢包或乱码，显示数值不会大幅变动。
                           {
                            Double f5 = Convert.ToInt32(temp5, 16) / 10.0;
                            //textBox1.Text = temp;
                            textBox5.Text = f5.ToString() + " PPM";
                           }
                        }
                    }
                 }   

            catch { }

       }

        public static void WriteLogs(string fileName, string type, string content)//创建接收日志后在bin目录生成.txt文档
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
        
        /// <summary>
        /// 服务器向客户端发送消息
        /// </summary>
        /// <param name="str"></param>
        void Send(string str)
            {
            if (sendflag)
                {
                    try
                       {
                        //byte[] buffer = Encoding.UTF8.GetBytes(str);
                        byte[] buffer = HexStrTobyte(str);
                        socketSend.Send(buffer);
                        //ShowMsg(socketSend.RemoteEndPoint + ":" + str);
                        }

                    catch{ }

                }
        }






        private void Form1_Load(object sender, EventArgs e)
        {
         Control.CheckForIllegalCrossThreadCalls = false;
        }

    
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

      
        private void bt_send_Click(object sender, EventArgs e)//报警器打开
        {
            //Send("06 05 00 00 FF 00 8D 8D");
            //pictureBox1.Image = imageList1.Images[0];
            baojing1 = true;
        }
        private void button1_Click(object sender, EventArgs e)//报警器关闭
        {
            //Send("06 05 00 00 00 00 CC 7D");
            //pictureBox1.Image = imageList7.Images[0];
            baojing2 = true;
        }

        private void button2_Click(object sender, EventArgs e)//风机打开
        {
            //Send("07 05 00 00 FF 00 8C 5C"); 
            //pictureBox10.Image = imageList6.Images[0];
            fengji1 = true;

        }

        private void button3_Click(object sender, EventArgs e)//风机关闭
        {
            //Send("07 05 00 00 00 00 CD AC");
           // pictureBox10.Image = imageList8.Images[0];
            fengji2 = true;
        }

        private void button4_Click(object sender, EventArgs e)//单灯打开
        {
            //Send("08 05 00 00 FF 00 8C A3");
            dandeng1 = true;
            //pictureBox9.Image = imageList5.Images[0];
        }

        private void button5_Click(object sender, EventArgs e)//单灯关闭
        {
            //Send("08 05 00 00 00 00 CD 53");
             dandeng2 = true;
            //pictureBox9.Image = imageList2.Images[0];
        }

        private void button6_Click(object sender, EventArgs e)//红灯开
        {
            //Send("05 05 00 03 01 00 3D DE");
            hongdeng1 = true;
            //pictureBox7.Image = imageList3.Images[0];
        }

        private void button7_Click(object sender, EventArgs e)//红灯关
        {
            //Send("05 05 00 03 00 00 3C 4E");
            //pictureBox7.Image = imageList2.Images[0];
            hongdeng2 = true;

        }

        private void button8_Click(object sender, EventArgs e)//黄灯开
        {
            //Send("05 05 00 01 01 00 9C 1E");
            //pictureBox8.Image = imageList4.Images[0];
            huangdeng1 = true;

        }

        private void button9_Click(object sender, EventArgs e)//黄灯关
        {
           // Send("05 05 00 01 00 00 9D 8E");
           // pictureBox8.Image = imageList2.Images[0];
            huangdeng2 = true;
        }

        private void button11_Click(object sender, EventArgs e)//白光1关
        {
            //Send("05 05 00 02 00 00 6D 8E");
            //pictureBox11.Image = imageList2.Images[0];
            baideng12 = true;
        }

        private void button10_Click(object sender, EventArgs e)//白光1开
        {
            //Send("05 05 00 02 01 00 6C 1E");
            //pictureBox11.Image = imageList5.Images[0];
            baideng11 = true;
        }

        private void button12_Click(object sender, EventArgs e)//白光2开
        {
            //Send("05 05 00 00 01 00 CD DE");
            //pictureBox12.Image = imageList5.Images[0];
            baideng21 = true;
        }

        private void button13_Click(object sender, EventArgs e)//白光2关
        {
            //Send("05 05 00 00 00 00 CC 4E");
           // pictureBox12.Image = imageList2.Images[0];
            baideng22 = true;
        }
        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("    ");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
       
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
         
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }

        private void 报警_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_3(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

    

    
       

    }
}