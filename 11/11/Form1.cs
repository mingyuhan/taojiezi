using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketSend;
        private void button2_Click(object sender, EventArgs e)
        {
            socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("192.168.181.1"), 8001);
            socketSend.Connect(point);
            showMsg("连接成功!");
            Thread th = new Thread(Receive);
            th.IsBackground = true;
            th.Start();
        }
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    Invoke(new Action(() => {//在线程里修改界面
                        showMsg(socketSend.RemoteEndPoint + ":" + str);
                    }));
                }
            }
            catch
            { }
        }
        void showMsg(string s)
        {
            textBox2.AppendText(s + "\r\n");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            socketSend.Send(buffer);
        }
    }
}
