using FastCopy.Basic;
using FastCopy.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FastCopy.ViewModels
{
    public class NotifyIconViewModel: NotifactionObject
    {
        public ICommand ShowWindow
        {
            get; set;
        }
        public ICommand CloseWindow
        {
            get;set;
        }
        public NotifyIconViewModel()
        {
            InitCommand();
        }
        private void InitCommand()
        {
            ShowWindow = new DelegateCommand(ShowWindowExecute);
            CloseWindow = new DelegateCommand(CloseWindowExecute);
        }
        private void ShowWindowExecute()
        {
            if (Application.Current.MainWindow == null) 
            {
                Application.Current.MainWindow = new FastCopyView();
                Application.Current.MainWindow.Show();
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Application.Current.MainWindow.Show();
            }
        }
        private void CloseWindowExecute()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
