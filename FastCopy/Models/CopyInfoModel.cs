using FastCopy.Basic;
using FastCopy.Common;
using FastCopy.DataBase;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FastCopy.Models
{
    public class CopyInfoModel : NotifactionObject
    {
        public int Id { get; set; }

        public string Guid { get; set; }
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
        /// 修改时间
        /// </summary>
        private DateTime? m_ModifyDateTime;
        public DateTime? ModifyDateTime
        {
            get
            {
                return m_ModifyDateTime;
            }
            set
            {
                m_ModifyDateTime=value;
                this.RaisePropertyChange("ModifyDateTime");
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
        private string m_Result;
        public string Result
        {
            get
            {
                return m_Result;
            }
            set
            {
                m_Result = value;
                this.RaisePropertyChange("Result");
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
        /// 
        /// </summary>
        private string m_ParentId;
        public string ParentId
        {
            get
            {
                return m_ParentId;
            }
            set
            {
                m_ParentId = value;
                this.RaisePropertyChange("ParentId");
            }
        }
        /// <summary>
        /// 是否展开
        /// </summary>
        private bool? m_IsExpended = false;
        public bool? IsExpended
        {
            get
            {
                return m_IsExpended;
            }
            set
            {
                m_IsExpended = value;
                this.RaisePropertyChange("IsExpended");
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string m_Remark;
        public string Remark
        {
            get
            {
                return m_Remark;
            }
            set
            {
                m_Remark = value;
                this.RaisePropertyChange("Remark");
            }
        }
        /// <summary>
        /// 置顶
        /// </summary>
        private bool? m_IsTop=false;
        public bool? IsTop
        {
            get
            {
                return m_IsTop;
            }
            set
            {
                m_IsTop = value;
                this.RaisePropertyChange("IsTop");
            }
        }
        private Visibility m_TopVisible=Visibility.Collapsed;
        [NotMapped]
        [JsonIgnore]
        public Visibility TopVisible
        {
            get
            {
                return m_TopVisible;
            }
            set
            {
                m_TopVisible = value;
                this.RaisePropertyChange("TopVisible");
            }
        }
        private List<CopyInfoModel> m_Children=new List<CopyInfoModel>();
        [NotMapped]
        [JsonIgnore]
        public List<CopyInfoModel> Children
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
        /// <summary>
        /// 执行程序图标
        /// </summary>
        private BitmapImage m_ExeIcon;
        [NotMapped]
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
        /// <summary>
        /// 进度条最大进度数
        /// </summary>
        private int m_ProgressMaxNum=100;
        [NotMapped]
        [JsonIgnore]
        public int ProgressMaxNum
        {
            get
            {
                return m_ProgressMaxNum;
            }
            set
            {
                m_ProgressMaxNum = value;
                this.RaisePropertyChange("ProgressMaxNum");
            }
        }
        /// <summary>
        /// 进度条当前进度数
        /// </summary>
        private int m_ProgressValue=0;
        [NotMapped]
        [JsonIgnore]
        public int ProgressValue
        {
            get
            {
                return m_ProgressValue;
            }
            set
            {
                m_ProgressValue = value;
                this.RaisePropertyChange("ProgressValue");
            }
        }
        /// <summary>
        /// 结果背景颜色
        /// </summary>
        private SolidColorBrush m_BackGroundColor;
        [NotMapped]
        [JsonIgnore]
        public SolidColorBrush BackGroundColor
        {
            get
            {
                return m_BackGroundColor;
            }
            set
            {
                m_BackGroundColor = value;
                this.RaisePropertyChange("BackGroundColor");
            }
        }
        /// <summary>
        /// 用来展示层级关系
        /// </summary>
        private Thickness m_GridMargin;
        [NotMapped]
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
        /// 展开状态类型
        /// </summary>
        private TriangleType m_TriangleType = TriangleType.NotFill;
        [NotMapped]
        [JsonIgnore]
        public TriangleType TriangleType
        {
            get
            {
                return m_TriangleType;
            }
            set
            {
                m_TriangleType = value;
                this.RaisePropertyChange("TriangleType");
            }
        }
        private Visibility m_IsVisible = Visibility.Visible;
        [NotMapped]
        [JsonIgnore]
        public Visibility IsVisible
        {
            get
            {
                return m_IsVisible;
            }
            set
            {
                m_IsVisible = value;
                this.RaisePropertyChange("IsVisible");
            }
        }
        /// <summary>
        /// 显示暂停，和停止复制按钮
        /// </summary>
        private Visibility m_IsPauseVisible = Visibility.Collapsed;
        [NotMapped]
        [JsonIgnore]
        public Visibility IsPauseVisible
        {
            get
            {
                return m_IsPauseVisible;
            }
            set
            {
                m_IsPauseVisible = value;
                this.RaisePropertyChange("IsPauseVisible");
            }
        }
        [NotMapped]
        [JsonIgnore]
        public Task CopyTask { get; set; }
        [NotMapped]
        [JsonIgnore]
        public CancellationTokenSource CancellationTokenSource { get; set; }
        [NotMapped]
        [JsonIgnore]
        public ManualResetEvent ManualResetEvent { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool IsCopyPaused = false;
        private int? m_TodoStatus = -1;
        public int? TodoStatus
        {
            get
            {
                return m_TodoStatus;
            }
            set
            {
                m_TodoStatus = value;
                this.RaisePropertyChange("TodoStatus");
            }
        }
        [NotMapped]
        /// <summary>
        /// 执行程序图标
        /// </summary>
        private BitmapImage m_TodoIcon;
        [NotMapped]
        [JsonIgnore]
        public BitmapImage TodoIcon
        {
            get
            {
                return m_TodoIcon;
            }
            set
            {
                m_TodoIcon = value;
                this.RaisePropertyChange("TodoIcon");
            }
        }
        private Visibility m_TodoStatusVisible = Visibility.Collapsed;
        [NotMapped]
        [JsonIgnore]
        public Visibility TodoStatusVisible
        {
            get
            {
                return m_TodoStatusVisible;
            }
            set
            {
                m_TodoStatusVisible = value;
                this.RaisePropertyChange("TodoStatusVisible");
            }
        }
        public CopyInfoModel Clone()
        {
            CopyInfoModel copyInfo = new CopyInfoModel();
            copyInfo.Guid = System.Guid.NewGuid().ToString();
            copyInfo.SourceAddress = SourceAddress;
            copyInfo.TargetAddress = TargetAddress;
            copyInfo.IsChecked= IsChecked;
            copyInfo.ParentId = ParentId;
            copyInfo.Remark = Remark;
            copyInfo.IsTop = IsTop;
            copyInfo.TopVisible = TopVisible;
            copyInfo.Children = Children;
            copyInfo.ExeIcon= ExeIcon;
            copyInfo.GridMargin = GridMargin;
            copyInfo.TriangleType = TriangleType;
            copyInfo.IsVisible = IsVisible;
            copyInfo.IsPauseVisible = IsPauseVisible;
            copyInfo.TodoStatus= TodoStatus;
            copyInfo.TodoIcon= TodoIcon;
            copyInfo.TodoStatusVisible= TodoStatusVisible;
            return copyInfo;
        }
    }
}
