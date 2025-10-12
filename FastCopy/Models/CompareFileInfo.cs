using FastCopy.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastCopy.Models
{
    public class CompareFileInfo : NotifactionObject
    {
        private string m_Name;
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                this.RaisePropertyChange("Name");
            }
        }
        private string m_Size;
        public string Size
        {
            get
            {
                return m_Size;
            }
            set
            {
                m_Size = value;
                this.RaisePropertyChange("Size");
            }
        }
        private string m_ModifyTime;
        public string ModifyTime
        {
            get { return m_ModifyTime; }
            set
            {
                m_ModifyTime = value;
                this.RaisePropertyChange("ModifyTime");
            }
        }
        private System.Windows.Media.Brush m_FontColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
        public System.Windows.Media.Brush FontColor
        {
            get
            {
                return m_FontColor;
            }
            set
            {
                m_FontColor = value;
                this.RaisePropertyChange("FontColor");
            }
        }
        /// <summary>
        /// 用来展示层级关系
        /// </summary>
        private Thickness m_GridMargin;
        [JsonIgnore]
        public Thickness GridMargin
        {
            get
            {
                return m_GridMargin;
            }
            set
            {
                m_GridMargin = value;
                this.RaisePropertyChange("GridMargin");
            }
        }
        /// <summary>
        /// 执行程序图标
        /// </summary>
        private BitmapImage m_ExeIcon;
        [JsonIgnore]
        public BitmapImage ExeIcon
        {
            get
            {
                return m_ExeIcon;
            }
            set
            {
                m_ExeIcon = value;
            }
        }
        private bool m_IsExpend = false;
        public bool IsExpend
        {
            get
            {
                return m_IsExpend;
            }
            set
            {
                m_IsExpend = value;
                this.RaisePropertyChange("IsExpend");
            }
        }
        private string m_CurrentPath;
        public string CurrentPath
        {
            get
            {
                return m_CurrentPath;
            }
            set
            {
                m_CurrentPath = value;
                this.RaisePropertyChange("CurrentPath");
            }
        }
        public ObservableCollection<CompareFileInfo> m_Children = new ObservableCollection<CompareFileInfo>();
        public ObservableCollection<CompareFileInfo> Children
        {
            get
            {
                return m_Children;
            }
            set
            {
                m_Children = value;
                this.RaisePropertyChange("Children");
            }
        }
        private int m_Level;
        public int Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
                this.RaisePropertyChange("Level");
            }
        }
    }
}
