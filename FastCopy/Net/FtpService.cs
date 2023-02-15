using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Net
{
    public class FtpService : IFtpService
    {
        public bool Connect()
        {
            return false;
        }

        public bool Download(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool Upload(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
