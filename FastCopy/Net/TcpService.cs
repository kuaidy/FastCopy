using FastCopy.log;
using FastCopy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class TcpService : ITcpService
    {
        private readonly ILogService m_LogService;

        public ObservableCollection<CopyInfoModel> CopyInfos { get; set; }



        public TcpService(ILogService logService)
        {
            m_LogService = logService;
        }

        public void Listen()
        {
            Task.Run(() =>
            {
                TcpListener TcpListener = null;
                try
                {
                    IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).First();
                    int port = 54321;
                    Byte[] bytes = new Byte[512];
                    string data;
                    TcpListener = new TcpListener(ip, port);
                    TcpListener.Start();
                    byte[] buffer = new byte[512];

                    //Thread tReceiveMsg = new Thread(ReceiveFile);
                    //tReceiveMsg.Start();
                    //tReceiveMsg.IsBackground = true;

                    Task.Run(() =>
                    {
                        AcceptMessage(TcpListener);
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    //TcpListener.Stop();
                }
            });
        }

        public void AcceptMessage(TcpListener tcpListener)
        {
            string data = string.Empty;
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                if (tcpClient.Connected)
                {
                    int i;
                    byte[] buffer = new byte[512];
                    NetworkStream networkStream = tcpClient.GetStream();
                    i = networkStream.Read(buffer, 0, buffer.Length);
                    data = Encoding.ASCII.GetString(buffer, 0, i);
                    if (!string.IsNullOrEmpty(data)) 
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(()=> {
                            CopyInfoModel copyInfoModel = new CopyInfoModel();
                            copyInfoModel.SourceAddress = data;
                            copyInfoModel.Status = "是否接收？";
                            if (CopyInfos != null)
                            {
                                CopyInfos.Add(copyInfoModel);
                            }
                        });
                        
                    }
                    tcpClient.Close();
                }

            }
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        private static void ReceiveFile()
        {
            try
            {
                //byte[] buffer = new byte[512];
                //int count = 0;
                //TcpClient tcpClient = TcpListener.AcceptTcpClient();
                //if (tcpClient.Connected)
                //{
                //    NetworkStream networkStream = tcpClient.GetStream();
                //    if (networkStream != null)
                //    {
                //        FileStream fs = new FileStream("E:\\test.txt", FileMode.Create, FileAccess.Write);
                //        while ((count = networkStream.Read(buffer, 0, buffer.Length)) != 0)
                //        {
                //            fs.Write(buffer, 0, count);
                //        }
                //        fs.Flush();
                //        networkStream.Flush();
                //        networkStream.Close();
                //        tcpClient.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMessage(string message, string ip, int port)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse(ip), port);
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
        public void SendFile(string fileName, string ip, string port)
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
