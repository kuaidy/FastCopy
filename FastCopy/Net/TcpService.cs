using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FastCopy.Net
{
    public class TcpService
    {
        static TcpListener TcpListener;
        public static void Listen()
        {
            Task.Run(() =>
            {
                try
                {
                    IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).First();
                    int port = 54321;
                    Byte[] bytes = new Byte[512];
                    string data;
                    TcpListener = new TcpListener(ip, port);
                    TcpListener.Start();

                    Thread tReceiveMsg = new Thread(ReceiveFile);
                    tReceiveMsg.Start();
                    tReceiveMsg.IsBackground = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        private static async void ReceiveFile()
        {
            byte[] buffer = new byte[512];
            int count = 0;
            try
            {
                TcpClient tcpClient = await TcpListener.AcceptTcpClientAsync();
                if (tcpClient.Connected)
                {
                    NetworkStream networkStream = tcpClient.GetStream();
                    if (networkStream != null)
                    {
                        FileStream fs = new FileStream("E:\\test.txt", FileMode.Create, FileAccess.Write);
                        while ((count = networkStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            fs.Write(buffer, 0, count);
                        }
                        fs.Flush();
                        networkStream.Flush();
                        networkStream.Close();
                        tcpClient.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public static void SendMessage(string message, string ip, string port)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse(ip), int.Parse(port));
            NetworkStream networkStream = tcpClient.GetStream();
            byte[] byteMess = Encoding.UTF8.GetBytes(message);

            int length = 0;
            while (length < byteMess.Length)
            {
                byte[] buffer = new byte[512];
                if (byteMess.Length < buffer.Length) 
                {
                    networkStream.Write(byteMess, 0, byteMess.Length);
                }
                else 
                {
                    networkStream.Write(byteMess, 0, buffer.Length);
                }
                length += buffer.Length;
            }
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        public static void SendFile(string fileName, string ip, string port)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse(ip), int.Parse(port));
            NetworkStream networkStream = tcpClient.GetStream();
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            int size = 0;
            int length = 0;
            while (length < fileStream.Length)
            {
                byte[] buffer = new byte[512];
                size = fileStream.Read(buffer, 0, buffer.Length);
                networkStream.Write(buffer, 0, size);
                length += size;
            }
            fileStream.Flush();
            networkStream.Flush();
            fileStream.Close();
            networkStream.Close();
        }
    }
}
