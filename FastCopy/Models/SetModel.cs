using FastCopy.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Models
{
    public class SetModel : NotifactionObject
    {
        public int Id { get; set; }

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
        private List<DetailSetModel> m_DetailSetModels = new List<DetailSetModel>();
        public List<DetailSetModel> DetailSetModels
        {
            get
            {
                return m_DetailSetModels;
            }
            set
            {
                m_DetailSetModels = value;
                this.RaisePropertyChange("DetailSetModels");
            }
        }
    }

    public class DetailSetModel : NotifactionObject
    {
        public int Id { get; set; }
        private string m_CName;
        public string CName
        {
            get
            {
                return m_CName;
            }
            set
            {
                m_CName = value;
                this.RaisePropertyChange("CName");
            }
        }
        private string m_EName;
        public string EName
        {
            get
            {
                return m_EName;
            }
            set
            {
                m_EName = value;
                this.RaisePropertyChange("EName");
            }
        }
        private string m_Value;
        public string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
                this.RaisePropertyChange("Value");
            }
        }
        /// <summary>
        /// 配置类型
        /// </summary>
        private string m_Type;
        public string Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
                this.RaisePropertyChange("Type");
            }
        }
    }
}
