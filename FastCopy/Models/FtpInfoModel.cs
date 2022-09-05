using FastCopy.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Models
{
    public class FtpInfoModel: NotifactionObject
    {
        /// <summary>
        /// 服务器
        /// </summary>
        private string m_Ip;
        public string Ip
        {
            get
            {
                return m_Ip;
            }
            set
            {
                m_Ip = value;
                this.RaisePropertyChange("Ip");
            }
        }
        /// <summary>
        /// 路径
        /// </summary>
        private string m_Path;
        public string Path
        {
            get
            {
                return m_Path;
            }
            set
            {
                m_Path = value;
                this.RaisePropertyChange("Path");
            }
        }
        /// <summary>
        /// 端口
        /// </summary>
        private string m_Port;
        public string Port
        {
            get
            {
                return m_Port;
            }
            set
            {
                m_Port = value;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        private string m_UserName;
        public string UserName
        {
            get
            {
                return m_UserName;
            }
            set
            {
                m_UserName = value;
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        private string m_Password;
        public string Password
        {
            get
            {
                return m_Password;
            }
            set
            {
                m_Password = value;
            }
        }
    }
}
