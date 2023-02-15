using FastCopy.Basic;
using FastCopy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.ViewModels
{
    public class NoticeViewModel : NotifactionObject
    {
        private List<CopyInfoModel> SuccCopyInfos = new List<CopyInfoModel>();
        private List<CopyInfoModel> FailCopyInfos = new List<CopyInfoModel>();


        private string m_NoticeInfo;
        public string NoticeInfo
        {
            get
            {
                return m_NoticeInfo;
            }
            set
            {
                m_NoticeInfo = value;
                this.RaisePropertyChange("NoticeInfo");
            }
        }

        public NoticeViewModel(List<CopyInfoModel> succCopyInfos, List<CopyInfoModel> failCopyInfos)
        {
            SuccCopyInfos = succCopyInfos;
            FailCopyInfos = failCopyInfos;
            GetNoticeShowInfo();
        }

        private void GetNoticeShowInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("复制成功：\r\n");
            foreach (CopyInfoModel copyInfoModel in SuccCopyInfos)
            {
                string fileName = Path.GetFileName(copyInfoModel.SourceAddress);
                stringBuilder.Append(fileName);
                stringBuilder.Append("\r\n");
            }
            stringBuilder.Append("复制失败：\r\n");
            foreach (CopyInfoModel copyInfoModel in FailCopyInfos)
            {
                string fileName = Path.GetFileName(copyInfoModel.SourceAddress);
                stringBuilder.Append(fileName);
                stringBuilder.Append("\r\n");
            }
            NoticeInfo = stringBuilder.ToString();
        }
    }
}
