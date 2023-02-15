using FastCopy.Common;
using System;
using System.Collections;
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
        //System.Threading.Timer m_Timer;

        public NoticeView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeToContent = SizeToContent.Height;
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                To = SystemParameters.WorkArea.Bottom - this.Height * (ConstantParameter.QueueNoticeWindows.Count > 5 ? 5 : ConstantParameter.QueueNoticeWindows.Count)
            };
            this.BeginAnimation(TopProperty, animation);
        }

        private void Animation_Loaded_Completed(object sender, EventArgs e)
        {
            //if (ConstantParameter.QueueNoticeWindows.Count > ConstantParameter.QueueNoticeWindowsCount)
            //{
            //    int count = ConstantParameter.QueueNoticeWindows.Count - ConstantParameter.QueueNoticeWindowsCount;
            //    for (int i = 0; i < count; i++)
            //    {
            //        var view = ConstantParameter.QueueNoticeWindows.Peek();
            //        ConstantParameter.QueueNoticeWindows.Dequeue();
            //        if (view is NoticeView)
            //        {
            //            NoticeView nView = view as NoticeView;
            //            nView.Window_Close();
            //        }
            //    }
            //}
        }

        public void Window_Close()
        {
            this.Dispatcher.Invoke(() =>
            {
                var animation = new DoubleAnimation(SystemParameters.WorkArea.Right, TimeSpan.FromSeconds(1));
                animation.Completed += Animation_Closed_Completed;
                this.BeginAnimation(LeftProperty, animation);
            });
        }

        private void Animation_Closed_Completed(object sender, EventArgs e)
        {
            int i = 0;
            IEnumerator enumerator = ConstantParameter.QueueNoticeWindows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (i < 4)
                {
                    if (enumerator.Current is NoticeView)
                    {
                        NoticeView noticeView = enumerator.Current as NoticeView;
                        var doubleAnimation = new DoubleAnimation(noticeView.Top + noticeView.Height, TimeSpan.FromSeconds(1));
                        noticeView.BeginAnimation(TopProperty, doubleAnimation);
                    }
                }
                i++;
            }
            this.Close();
        }
    }
}
