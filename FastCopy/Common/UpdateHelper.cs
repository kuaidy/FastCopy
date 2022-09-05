using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows;

namespace FastCopy.Common
{
    public class UpdateHelper
    {
        public static void Init()
        {
            Task.Run(() =>
            {
                try
                {
                    //int port = 54321;
                    //Byte[] bytes = new Byte[256];
                    //TcpListener tcpListener = TcpListener.Create(port);
                    //tcpListener.Start();
                    //string response = string.Empty;
                    //while (true)
                    //{
                    //    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    //    var netStream = tcpClient.GetStream();
                    //    int i;
                    //    while ((i=netStream.Read(bytes, 0, bytes.Length))!=0)
                    //    {
                    //        response = Encoding.ASCII.GetString(bytes,0,i);
                    //    }
                    //    byte[] data = Encoding.ASCII.GetBytes(ConstantParameter.Version);
                    //    netStream.Write(data);
                    //}
                }
                catch (Exception ex)
                {

                }
            });
        }
        public static List<string> GetAllIp()
        {
            return null;
        }
    }
}
