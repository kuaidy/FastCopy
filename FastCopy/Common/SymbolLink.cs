using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Common
{
    public class SymbolLink
    {
        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string symbolicFileName,string targetFileName,UInt32 flag);
    }
}
