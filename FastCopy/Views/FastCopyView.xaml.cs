using FastCopy.Common;
using FastCopy.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;

namespace FastCopy.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FastCopyView : Window
    {
        private IConfigService m_ConfigService;
        public FastCopyView()
        {
            InitializeComponent();
            m_ConfigService = ServiceHelper.ServiceProvider.GetService<IConfigService>();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_ConfigService.IsCloseToTaskBar)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
