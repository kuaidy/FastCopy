using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Net
{
    public interface IFtpService
    {
        bool Connect();
        bool Upload(string fileName);
        bool Download(string fileName);
    }
}
