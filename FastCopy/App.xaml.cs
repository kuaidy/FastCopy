using FastCopy.Common;
using FastCopy.DataBase;
using FastCopy.Net;
using FastCopy.ViewModels;
using FastCopy.Views;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CopyFast
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _taskbar;
        public App()
        {

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _taskbar = (TaskbarIcon)FindResource("Taskbar");
            //SqliteHelper.Init();
            //ConfigHelper.Init();
            //注册服务
            ServiceHelper.Init();
            FastCopyViewModel fastCopyViewModel = ServiceHelper.ServiceProvider.GetService<FastCopyViewModel>();
            FastCopyView fastCopyView = ServiceHelper.ServiceProvider.GetService<FastCopyView>();
            fastCopyViewModel.FastCopyView = fastCopyView;
            //FastCopyViewModel fastCopyViewModel = new FastCopyViewModel(fastCopyView);
            fastCopyView.DataContext = fastCopyViewModel;
            fastCopyView.Closed += fastCopyViewModel.FastCopyView_Closed;
            fastCopyView.Deactivated += fastCopyViewModel.FastCopyView_Closed;
            fastCopyView.Show();
            base.OnStartup(e);
        }
    }
}