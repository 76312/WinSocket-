using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace WinSocket通信
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Any;
            IPEndPoint port = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
            socketWatch.Bind(port);
            ShowMsg("监听开始...");
            socketWatch.Listen(10);
            Thread th = new Thread(Listen);
            th.IsBackground = true;
            th.Start(socketWatch);
        }

        void Listen(object c)
        {
            Socket socketWatch = c as Socket;
            while (true) 
            {
                Socket socketSend = socketWatch.Accept();
                ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
                Thread th = new Thread(receiveData);
                th.IsBackground = true;
                th.Start(socketSend);
            }
        }

        void receiveData(object c)
        {
            Socket socketSend = c as Socket;
            while(true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                int r = socketSend.Receive(buffer);
                if (r == 0) break;
                string str = Encoding.UTF8.GetString(buffer,0,r);
                ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + str);
            }
        }

        void ShowMsg(string str)
        {
            txtRec.AppendText(str + "\r\n");
        }
    }
}
