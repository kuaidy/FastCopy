using FastCopy.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FastCopy.ViewModels
{
    public class FtpViewModel
    {
        public ICommand TestConnectCommand { get; set; }
        private void InitCommand()
        {
            TestConnectCommand = new DelegateCommand(TestConnectCommandExecute);
        }
        private void TestConnectCommandExecute()
        {

        }
    }
}
