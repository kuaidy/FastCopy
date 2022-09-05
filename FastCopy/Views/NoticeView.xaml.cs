using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FastCopy.Views
{
    /// <summary>
    /// NoticeView.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeView : Window
    {
        public NoticeView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeToContent = SizeToContent.Height;
            this.Left = SystemParameters.WorkArea.Right - this.Width;
            this.Top = SystemParameters.WorkArea.Bottom;
            var animation = new DoubleAnimation {
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                To = SystemParameters.WorkArea.Bottom - this.Height
            };
            this.BeginAnimation(TopProperty, animation);
        }
    }
}
