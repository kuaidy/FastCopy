using FastCopy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Net
{
    public interface ITcpService
    {
        ObservableCollection<CopyInfoModel> CopyInfos { get; set; }
        void Listen();
        void AcceptMessage(TcpListener tcpListener);
        void SendMessage(string message, string ip, int port);
        void SendFile(string fileName, string ip, string port);
    }
}
