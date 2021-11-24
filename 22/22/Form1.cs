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

namespace _22
{
    public partial class Form1 : Form
    {
        string str = "没有改变";
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketSend;
      

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //点击开始侦听的时候，服务器创建一个负责监听IP地址跟端口号的Socket
                Socket socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Any;
                //创建端口对象
                IPEndPoint point = new IPEndPoint(ip, 8001);
                //绑定
                socketwatch.Bind(point);

                showMsg("监听成功！");
                socketwatch.Listen(10);
                //创建一个线程
                Thread th = new Thread(listen);
                th.IsBackground = true;
                th.Start(socketwatch);

            }
            catch
            {
                showMsg("监听失败");
            }
            
            void listen(Object o)
            {
                try
                {

                    Socket socketwatch = o as Socket;
                    int i = 0;
                    while (true)
                    {
                        //等待客户端的连接
                        socketSend = socketwatch.Accept();

                        str = socketSend.RemoteEndPoint.ToString() + ":" + "连接成功！";
                        Invoke(new Action(() => {//在线程里修改界面
                            showMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功！");
                        }));
                        Thread th = new Thread(Receive);
                        th.IsBackground = true;
                        th.Start(socketSend);
                    }
                }
                catch
                { }
            }
            //接收客户端发送的信息
            void Receive(Object o)
            {
                try
                {
                    Socket socketSend = o as Socket;
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
            void showMsg(string str)
            {
                textBox1.AppendText(str + "\r\n");
            }
        //服务器给客户端发送消息

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox2.Text;
            Console.WriteLine(str);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            socketSend.Send(buffer);
        }
    }
}
