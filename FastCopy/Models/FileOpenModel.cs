using FastCopy.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Models
{
    public class FileOpenModel:NotifactionObject
    {
        public int Id { get; set; }
        private string m_AppPath;
        public string AppPath
        {
            get
            {
                return m_AppPath;
            }
            set
            {
                m_AppPath = value;

            }
        }
    }
}
