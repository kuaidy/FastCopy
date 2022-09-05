using FastCopy.Basic;
using FastCopy.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastCopy.Models
{
    public class CopyInfoModel : NotifactionObject
    {
        public int Id { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool m_IsChecked = false;
        public bool IsChecked
        {
            get
            {
                return m_IsChecked;
            }
            set
            {
                m_IsChecked = value;
                this.RaisePropertyChange("IsChecked");
            }
        }
        /// <summary>
        /// 源地址
        /// </summary>
        private string m_SourceAddress;
        public string SourceAddress
        {
            get
            {
                return m_SourceAddress;
            }
            set
            {
                m_SourceAddress = value;
                this.RaisePropertyChange("SourceAddress");
            }
        }
        /// <summary>
        /// 目标地址
        /// </summary>
        private string m_TargetAddress;
        public string TargetAddress
        {
            get
            {
                return m_TargetAddress;
            }
            set
            {
                m_TargetAddress = value;
                this.RaisePropertyChange("TargetAddress");
            }
        }
        /// <summary>
        /// 时间
        /// </summary>
        private string m_CopyTime;
        public string CopyTime
        {
            get
            {
                return m_CopyTime;
            }
            set
            {
                m_CopyTime = value;
                this.RaisePropertyChange("CopyTime");
            }
        }
        /// <summary>
        /// 结果
        /// </summary>
        private string m_Status;
        public string Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
                this.RaisePropertyChange("Status");
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        private string m_Sort;
        public string Sort
        {
            get
            {
                return m_Sort;
            }
            set
            {
                m_Sort = value;
                this.RaisePropertyChange("Sort");
            }
        }
        /// <summary>
        /// 执行程序图标
        /// </summary>
        private BitmapImage m_ExeIcon;
        [NotMapped]
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
    }
}
