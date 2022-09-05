using FastCopy.Basic;
using FastCopy.Models;
using FastCopy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using FastCopy.Common;
using FastCopy.DataBase;
using System.Collections.ObjectModel;

namespace FastCopy.ViewModels
{
    public class SettingViewModel : NotifactionObject
    {
        private SetModel m_SetModel = new SetModel();
        public SetModel SetModel
        {
            get
            {
                return m_SetModel;
            }
            set
            {
                m_SetModel = value;
                this.RaisePropertyChange("SetModel");
            }
        }
        /// <summary>
        /// 所有的设置
        /// </summary>
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
            }
        }
        /// <summary>
        /// 打开文件设置
        /// </summary>
        private ObservableCollection<DetailSetModel> m_OpenFileSetInfos = new ObservableCollection<DetailSetModel>();
        public ObservableCollection<DetailSetModel> OpenFileSetInfos
        {
            get
            {
                return m_OpenFileSetInfos;
            }
            set
            {
                m_OpenFileSetInfos = value;
                this.RaisePropertyChange("OpenFileSetInfos");
            }
        }
        /// <summary>
        /// 关闭缩小到任务栏
        /// </summary>
        private bool m_IsCloseToTaskBar = false;
        public bool IsCloseToTaskBar
        {
            get
            {
                return m_IsCloseToTaskBar;
            }
            set
            {
                m_IsCloseToTaskBar = value;
                ConfigHelper.IsCloseToTaskBar = value;
                SetSetData("IsCloseToTaskBar", "关闭时最小化到任务栏",value?"1":"0", ConstantParameter.NormalSet);
                this.RaisePropertyChange("IsCloseToTaskBar");
            }
        }
        /// <summary>
        /// 自动更新
        /// </summary>
        private bool m_IsAutoUpdate = false;
        public bool IsAutoUpdate
        {
            get
            {
                return m_IsAutoUpdate;
            }
            set
            {
                m_IsAutoUpdate = value;
                ConfigHelper.IsAutoUpdate = value;
                SetSetData("IsAutoUpdate", "启动检查更新", value ? "1" : "0", ConstantParameter.NormalSet);
                this.RaisePropertyChange("IsAutoUpdate");
            }
        }
        /// <summary>
        /// 开机自启动
        /// </summary>
        private bool m_IsAutoStart = false;
        public bool IsAutoStart
        {
            get
            {
                return m_IsAutoStart;
            }
            set
            {
                m_IsAutoStart = value;
                ConfigHelper.IsAutoStart = value;
                SetSetData("IsAutoStart", "开机自启动", value ? "1" : "0", ConstantParameter.NormalSet);
                SetAutoStart(value);
                this.RaisePropertyChange("IsAutoStart");
            }
        }
        /// <summary>
        /// 只复制比较新的文件
        /// </summary>
        private bool m_IsCopyNewFile = false;
        public bool IsCopyNewFile
        {
            get
            {
                return m_IsCopyNewFile;
            }
            set
            {
                m_IsCopyNewFile = value;
                ConfigHelper.IsCopyNewFile = value;
                SetSetData("IsCopyNewFile", "只复制文件时间最新的文件", value ? "1" : "0", ConstantParameter.NormalSet);
                this.RaisePropertyChange("IsCopyNewFile");
            }
        }
        /// <summary>
        /// 保存程序运行模式
        /// </summary>
        private bool m_IsSaveModes = false;
        public bool IsSaveModes
        {
            get
            {
                return m_IsSaveModes;
            }
            set
            {
                m_IsSaveModes = value;
                ConfigHelper.IsSaveModes = value;
                SetSetData("IsSaveModes", "关闭软件保存当前模式", value ? "1" : "0", ConstantParameter.NormalSet);
                this.RaisePropertyChange("IsSaveModes");
            }
        }
        /// <summary>
        /// 打开文件配置
        /// </summary>
        private bool m_IsSelectedOpenFileSet;
        public bool IsSelectedOpenFileSet
        {
            get
            {
                return m_IsSelectedOpenFileSet;
            }
            set
            {
                m_IsSelectedOpenFileSet = value;
                this.RaisePropertyChange("IsSelectedOpenFileSet");
            }
        }
        public SettingView SettingView;
        public SettingViewModel(SettingView settingView)
        {
            SettingView = settingView;
            settingView.Closed += SettingView_Closed;
            InitData();
        }

        private void SettingView_Closed(object sender, EventArgs e)
        {
            SaveSetDetails();
        }
        private void InitData()
        {
            GetSetData();
        }
        /// <summary>
        /// 获取设置的数据
        /// </summary>
        private void GetSetData()
        {
            //常规配置
            DetailSetModels = SqliteHelper.FastCopyDbContext.DetailSetModels.Where(x=>x.Type== ConstantParameter.NormalSet).ToList();
            foreach (DetailSetModel detailSetModel in DetailSetModels)
            {
                switch (detailSetModel.EName)
                {
                    case "IsCloseToTaskBar": 
                        {
                            IsCloseToTaskBar = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsAutoUpdate":
                        {
                            IsAutoUpdate = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsAutoStart":
                        {
                            IsAutoStart = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsCopyNewFile":
                        {
                            IsCopyNewFile = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsSaveModes":
                        {
                            IsSaveModes = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                }
            }
            //文件打开配置
            var items= SqliteHelper.FastCopyDbContext.DetailSetModels.Where(x => x.Type == ConstantParameter.OpenFileSet).ToList();
            foreach(var item in items) 
            {
                OpenFileSetInfos.Add(item);
            }
        }
        /// <summary>
        /// 保存设置的数据
        /// </summary>
        /// <param name="eName"></param>
        /// <param name="cName"></param>
        /// <param name="value"></param>
        private void SetSetData(string eName,string cName,string value,string type)
        {
            DetailSetModel detailSetModel= DetailSetModels.Find(x => x.EName == eName);
            if (detailSetModel != null)
            {
                foreach (DetailSetModel detailSet in DetailSetModels) 
                {
                    if (detailSet.EName == eName)
                    {
                        detailSet.Value = value;
                    }
                }
            }
            else
            {
                DetailSetModel detailSet = new DetailSetModel();
                detailSet.EName = eName;
                detailSet.CName = cName;
                detailSet.Value = value;
                detailSet.Type = type;
                DetailSetModels.Add(detailSet);
                SqliteHelper.FastCopyDbContext.DetailSetModels.Add(detailSet);
            }
        }
        /// <summary>
        /// 保存设置到数据库
        /// </summary>
        private void SaveSetDetails()
        {
            try
            {
                //常规配置
                foreach (DetailSetModel detailSet in DetailSetModels)
                {
                    DetailSetModel detailSetModel= SqliteHelper.FastCopyDbContext.DetailSetModels.ToList().Find(x => x.EName == detailSet.EName);
                    if (detailSetModel != null)
                    {
                        SqliteHelper.FastCopyDbContext.DetailSetModels.Update(detailSet);
                    }
                    else
                    {
                        SqliteHelper.FastCopyDbContext.DetailSetModels.Add(detailSet);
                    }
                }
                //文件打开配置
                foreach (DetailSetModel item in OpenFileSetInfos) 
                {
                    DetailSetModel detailSetModel = SqliteHelper.FastCopyDbContext.DetailSetModels.ToList().Find(x=>x.Id==item.Id);
                    if (detailSetModel != null)
                    {
                        SqliteHelper.FastCopyDbContext.DetailSetModels.Update(item);
                    }
                    else
                    {
                        item.Type = ConstantParameter.OpenFileSet;
                        SqliteHelper.FastCopyDbContext.DetailSetModels.Add(item);
                    }
                }
                SqliteHelper.FastCopyDbContext.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }
        /// <summary>
        /// 设置开机自启动
        /// </summary>
        private void SetAutoStart(bool autoStart=true)
        {
            if (autoStart)
            {
                List<string> shortCutPaths = GetQuickFromFolder(ConstantParameter.SystemStartPath,ConstantParameter.AppFullPath);
                if (shortCutPaths != null && shortCutPaths.Count > 1)
                {
                    for(int i = 1; i < shortCutPaths.Count; i++)
                    {
                        File.Delete(shortCutPaths[i]);
                    }
                }
                else if(shortCutPaths==null||shortCutPaths.Count<1)
                {
                    CreatShorCut(ConstantParameter.SystemStartPath, ConstantParameter.ShotCut, ConstantParameter.AppFullPath);
                }
            }
            else
            {
                List<string> shortcutPaths = GetQuickFromFolder(ConstantParameter.SystemStartPath, ConstantParameter.AppFullPath);
                if(shortcutPaths != null&& shortcutPaths.Count > 0)
                {
                    foreach (string shortcutPath in shortcutPaths)
                    {
                        File.Delete(shortcutPath);
                    }
                }
            }
        }
        /// <summary>
        /// 获取快捷方式
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        private List<string> GetQuickFromFolder(string directory,string targetPath)
        {
            List<string> shortCuts = new List<string>();
            string[] files = Directory.GetFiles(directory);
            if (files == null || files.Length < 1)
            {
                return null;
            }
            foreach(string file in files)
            {
                string ext = Path.GetExtension(file);
                if (ext == ".lnk")
                {
                    string tmpStr = GetAppPathFromShotCut(file);
                    if (tmpStr == targetPath)
                    {
                        shortCuts.Add(file);
                    }
                }
            }
            return shortCuts;
        }
        /// <summary>
        /// 通过快捷方式获取程序路径
        /// </summary>
        /// <returns></returns>
        private string GetAppPathFromShotCut(string shortCutFile)
        {
            if (File.Exists(shortCutFile))
            {
                IWshRuntimeLibrary.WshShell wshShell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut wshShortcut =(IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(shortCutFile);
                return wshShortcut.TargetPath;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 创建自启动快捷方式
        /// </summary>
        /// <returns></returns>
        private bool CreatShorCut(string directory,string shortCutName,string targetPath,string description=null,string iconLocation=null)
        {
            try
            {
                if (!Directory.Exists(directory)) 
                {
                    Directory.CreateDirectory(directory);
                }
                string shortcutPath = Path.Combine(directory, string.Format("{0}.lnk", shortCutName));
                IWshRuntimeLibrary.WshShell wshShell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut wshShortcut =(IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(shortcutPath);
                wshShortcut.TargetPath = targetPath;
                wshShortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                wshShortcut.WindowStyle = 1;
                wshShortcut.Description = description;
                wshShortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;
                wshShortcut.Save();
                return true;
            }
            catch(Exception ex){
                return false;
            }
        }
    }
}