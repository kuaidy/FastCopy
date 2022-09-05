using FastCopy.Basic;
using FastCopy.Common;
using FastCopy.DataBase;
using FastCopy.Models;
using FastCopy.Net;
using FastCopy.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FastCopy.ViewModels
{
    public class FastCopyViewModel : NotifactionObject
    {
        #region 属性

        public FastCopyView FastCopyView;

        private CopyInfoModel m_CopyInfo = new CopyInfoModel();
        public CopyInfoModel CopyInfo
        {
            get
            {
                return m_CopyInfo;
            }
            set
            {
                m_CopyInfo = value;
                this.RaisePropertyChange("CopyInfo");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<CopyInfoModel> m_CopyInfos = new ObservableCollection<CopyInfoModel>();
        public ObservableCollection<CopyInfoModel> CopyInfos
        {
            get
            {
                return m_CopyInfos;
            }
            set
            {
                m_CopyInfos = value;
                this.RaisePropertyChange("CopyInfos");
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        private int m_Count = 0;
        public int Count
        {
            get
            {
                return m_Count;
            }
            set
            {
                m_Count = value;
                this.RaisePropertyChange("Count");
            }
        }
        private bool m_IsCheckedAll = false;
        public bool IsCheckedAll
        {
            get
            {
                return m_IsCheckedAll;
            }
            set
            {
                m_IsCheckedAll = value;
                this.RaisePropertyChange("IsCheckedAll");
                if (m_IsCheckedAll)
                {
                    foreach (CopyInfoModel copyInfoModel in CopyInfos)
                    {
                        copyInfoModel.IsChecked = true;
                    }
                }
                else
                {
                    foreach (CopyInfoModel copyInfoModel in CopyInfos)
                    {
                        copyInfoModel.IsChecked = false;
                    }
                }
            }
        }
        /// <summary>
        /// 自动模式
        /// </summary>
        private bool m_AutoIsChecked = false;
        public bool AutoIsChecked
        {
            get
            {
                return m_AutoIsChecked;
            }
            set
            {
                m_AutoIsChecked = value;
                this.RaisePropertyChange("AutoIsChecked");
                FileWatctStart();
                ShowCopyMode();
                SetMode("AutoIsChecked", ConstantParameter.AutoMode, value ? "1" : "0");
            }
        }
        /// <summary>
        /// 备份模式
        /// </summary>
        private bool m_BakIsChecked = false;
        public bool BakIsChecked
        {
            get
            {
                return m_BakIsChecked;
            }
            set
            {
                m_BakIsChecked = value;
                this.RaisePropertyChange("BakIsChecked");
                ShowCopyMode();
                SetMode("BakIsChecked", ConstantParameter.BakMode, value ? "1" : "0");
            }
        }
        /// <summary>
        /// 同步模式
        /// </summary>
        private bool m_SyncIsChecked = false;
        public bool SyncIsChecked
        {
            get
            {
                return m_SyncIsChecked;
            }
            set
            {
                m_SyncIsChecked = value;
                this.RaisePropertyChange("SyncIsChecked");
                ShowCopyMode();
                SetMode("SyncIsChecked", ConstantParameter.SyncMode, value ? "1" : "0");
            }
        }
        /// <summary>
        /// 定时模式
        /// </summary>
        private bool m_ClockIsChecked = false;
        public bool ClockIsChecked
        {
            get
            {
                return m_ClockIsChecked;
            }
            set
            {
                m_ClockIsChecked = value;
                this.RaisePropertyChange("ClockIsChecked");
                ShowCopyMode();
                SetMode("ClockIsChecked", ConstantParameter.ClockMode, value ? "1" : "0");
            }
        }
        private DataGridCellInfo m_CurrentCell;
        public DataGridCellInfo CurrentCell
        {
            get
            {
                return m_CurrentCell;
            }
            set
            {
                m_CurrentCell = value;
                this.RaisePropertyChange("CurrentCell");
            }
        }
        private string m_CopyMode;
        public string CopyMode
        {
            get
            {
                return m_CopyMode;
            }
            set
            {
                m_CopyMode = value;
                this.RaisePropertyChange("CopyMode");
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
            }
        }
        #endregion
        public ICommand AddCommand
        {
            get; set;
        }
        public ICommand CopyCommand
        {
            get; set;
        }
        public ICommand DelCommand
        {
            get; set;
        }
        public ICommand SelectFileCommand
        {
            get; set;
        }
        public ICommand SelectDirCommand
        {
            get; set;
        }
        public ICommand SelectDestDirCommand
        {
            get; set;
        }
        public ICommand OpenDirCommand { get; set; }
        public ICommand SetCommand { get; set; }
        public ICommand CheckUpdateCommand { get; set; }
        public ICommand AddFtpPathCommand
        {
            get; set;
        }
        public ICommand OpenFileCommand { get; set; }
        public FastCopyViewModel()
        {
            InitCommand();
        }
        public FastCopyViewModel(FastCopyView fastCopyView)
        {
            InitCommand();
            InitData();
            FastCopyView = fastCopyView;
            FastCopyView.Closed += FastCopyView_Closed;
        }
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FastCopyView_Closed(object sender, EventArgs e)
        {
            SaveCopyInfos();
            SaveModes();
        }
        private void InitCommand()
        {
            AddCommand = new DelegateCommand(AddCommandExecute);
            CopyCommand = new DelegateCommand(CopyCommandExecute);
            DelCommand = new DelegateCommand(DelCommandExecute);
            SelectFileCommand = new DelegateCommand<object>(SelectFileCommandExecute);
            SelectDirCommand = new DelegateCommand(SelectDirCommandExecute);
            OpenDirCommand = new DelegateCommand(OpenDirCommandExecute);
            SetCommand = new DelegateCommand(SetCommandExecute);
            CheckUpdateCommand = new DelegateCommand(CheckUpdateCommandExecute);
            AddFtpPathCommand = new DelegateCommand(AddFtpPathCommandExecute);
            OpenFileCommand = new DelegateCommand(OpenFileCommandExecute);
        }
        /// <summary>
        /// 添加
        /// </summary>
        private void AddCommandExecute()
        {
            //if (string.IsNullOrEmpty(CopyInfo.SourceAddress))
            //{
            //    System.Windows.MessageBox.Show("源地址不能为空。","提示");
            //    return;
            //}
            //if (string.IsNullOrEmpty(CopyInfo.TargetAddress))
            //{
            //    System.Windows.MessageBox.Show("目标地址不能为空。", "提示"); 
            //    return;
            //}

            CopyInfoModel copyInfoModel = new CopyInfoModel();
            copyInfoModel.SourceAddress = "";
            copyInfoModel.TargetAddress = "";
            CopyInfos.Add(copyInfoModel);
            UpdateCount();
        }
        /// <summary>
        /// 复制
        /// </summary>
        private void CopyCommandExecute()
        {
            TcpService.SendMessage("SendMessage", "192.168.2.58","54321");
            try
            {
                List<CopyInfoModel> succCopyInfoModels = new List<CopyInfoModel>();
                List<CopyInfoModel> failCopyInfoModels = new List<CopyInfoModel>();
                for (int i = 0; i < CopyInfos.Count; i++)
                {
                    CopyInfoModel copyInfo = CopyInfos[i];
                    if (copyInfo.IsChecked)
                    {
                        if (File.Exists(copyInfo.SourceAddress))
                        {
                            try
                            {
                                string fileName = Path.GetFileName(copyInfo.SourceAddress);
                                if (string.IsNullOrEmpty(copyInfo.TargetAddress))
                                {
                                    copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    copyInfo.Status = string.Format("成功{0}个，失败{1}个", 0, 1);
                                    failCopyInfoModels.Add(copyInfo);
                                }
                                else
                                {
                                    string destFileName = Path.Combine(copyInfo.TargetAddress, fileName);
                                    if (BakIsChecked)
                                    {
                                        if (File.Exists(destFileName))
                                        {
                                            string extension = Path.GetExtension(destFileName);
                                            string fileNameNoExt = Path.GetFileNameWithoutExtension(destFileName);
                                            string bakDestFileName = Path.Combine(copyInfo.TargetAddress, fileNameNoExt + "_bak" + extension);
                                            if (File.Exists(bakDestFileName))
                                            {
                                                DateTime destFileTime = File.GetLastWriteTime(destFileName);
                                                DateTime bakDestFileTime = File.GetLastWriteTime(bakDestFileName);
                                                TimeSpan timeSpan = bakDestFileTime - destFileTime;
                                                FileStream destFileStream = File.OpenRead(destFileName);
                                                double destFileLength = destFileStream.Length;
                                                FileStream bakDestFileStream = File.OpenRead(bakDestFileName);
                                                double bakDestFileLength = bakDestFileStream.Length;
                                                destFileStream.Close();
                                                bakDestFileStream.Close();
                                                if (timeSpan.TotalSeconds != 0 || destFileLength != bakDestFileLength)
                                                {
                                                    File.Copy(destFileName, bakDestFileName, true);
                                                }
                                            }
                                        }
                                    }
                                    if (ConfigHelper.IsCopyNewFile)
                                    {
                                        if (File.Exists(destFileName))
                                        {
                                            DateTime sourceFileTime = File.GetLastWriteTime(copyInfo.SourceAddress);
                                            DateTime targetFileTime = File.GetLastWriteTime(destFileName);
                                            TimeSpan timeSpan = sourceFileTime - targetFileTime;
                                            if (timeSpan.TotalMilliseconds > 0)
                                            {
                                                File.Copy(copyInfo.SourceAddress, destFileName, true);
                                                copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                copyInfo.Status = string.Format("成功{0}个，失败{1}个", 1, 0);
                                                succCopyInfoModels.Add(copyInfo);
                                            }
                                        }
                                        else
                                        {
                                            File.Copy(copyInfo.SourceAddress, destFileName, true);
                                            copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            copyInfo.Status = string.Format("成功{0}个，失败{1}个", 1, 0);
                                            succCopyInfoModels.Add(copyInfo);
                                        }
                                    }
                                    else
                                    {
                                        File.Copy(copyInfo.SourceAddress, destFileName, true);
                                        copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        copyInfo.Status = string.Format("成功{0}个，失败{1}个", 1, 0);
                                        succCopyInfoModels.Add(copyInfo);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                copyInfo.Status = string.Format("成功{0}个，失败{1}个", 0, 1);
                                failCopyInfoModels.Add(copyInfo);
                            }
                        }
                        else if (Directory.Exists(copyInfo.SourceAddress))
                        {
                            int successNum = 0;
                            int failNum = 0;
                            CopyFolder(copyInfo.SourceAddress, copyInfo.TargetAddress, ref successNum, ref failNum);
                            copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            copyInfo.Status = string.Format("成功{0}个，失败{1}个", successNum, failNum);
                            if (failNum > 0)
                            {
                                failCopyInfoModels.Add(copyInfo);
                            }

                        }
                        else
                        {
                            copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            copyInfo.Status = string.Format("源地址无效");
                            failCopyInfoModels.Add(copyInfo);
                        }
                    }
                }
                Notice(succCopyInfoModels, failCopyInfoModels);
            }
            catch 
            {

            }
        }
        /// <summary>
        /// 复制文件夹
        /// </summary>
        private void CopyFolder(string sourceFolder, string destFolder, ref int successNum, ref int failNum)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }
                string[] files = Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    try
                    {

                        string fileName = Path.GetFileName(file);
                        string destFileName = Path.Combine(destFolder, fileName);
                        if (BakIsChecked)
                        {
                            if (File.Exists(destFileName))
                            {
                                string extension = Path.GetExtension(fileName);
                                string fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
                                string timeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                                string bakDestFileName = Path.Combine(destFolder, fileNameNoExt + timeStr + extension);
                                File.Move(destFileName, bakDestFileName);
                            }
                        }
                        File.Copy(file, destFileName, true);
                        successNum++;

                    }
                    catch (Exception ex)
                    {
                        failNum++;
                    }
                }
                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string folderName = Path.GetFileName(folder);
                    string dest = Path.Combine(destFolder, folderName);
                    CopyFolder(folder, dest, ref successNum, ref failNum);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        private void DelCommandExecute()
        {
            if (System.Windows.MessageBox.Show("是否删除选中的记录？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                List<CopyInfoModel> copyInfoModels = CopyInfos.Where(x => x.IsChecked == true).ToList();
                foreach (CopyInfoModel copyInfoModel in copyInfoModels)
                {
                    CopyInfos.Remove(copyInfoModel);
                    CopyInfoModel dbCopyInfoModel = SqliteHelper.FastCopyDbContext.CopyInfos.Single(x => x.Id == copyInfoModel.Id);
                    if (dbCopyInfoModel != null)
                    {
                        SqliteHelper.FastCopyDbContext.CopyInfos.Remove(dbCopyInfoModel);
                    }
                }
            }
            UpdateCount();
        }
        /// <summary>
        /// 选择源文件
        /// </summary>
        private void SelectFileCommandExecute(object obj)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Title = "选择文件";
            openDlg.Filter = "所有文件 (*.*)|*.*";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                string headerStr = CurrentCell.Column.Header.ToString();
                switch (headerStr)
                {
                    case ConstantParameter.SrcAddress:
                        {
                            if (CurrentCell.Item != null)
                            {
                                CopyInfoModel copyInfoModel = CurrentCell.Item as CopyInfoModel;
                                copyInfoModel.SourceAddress = openDlg.FileName;
                            }
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// 选择目录
        /// </summary>
        private void SelectDirCommandExecute()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string headerStr = CurrentCell.Column.Header.ToString();
                switch (headerStr)
                {
                    case ConstantParameter.SrcAddress:
                        {
                            if (CurrentCell.Item != null)
                            {
                                CopyInfoModel copyInfoModel = CurrentCell.Item as CopyInfoModel;
                                copyInfoModel.SourceAddress = folderBrowserDialog.SelectedPath;
                            }
                            break;
                        }
                    case ConstantParameter.TargetAddress:
                        {
                            if (CurrentCell.Item != null)
                            {
                                CopyInfoModel copyInfoModel = CurrentCell.Item as CopyInfoModel;
                                copyInfoModel.TargetAddress = folderBrowserDialog.SelectedPath;
                            }
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// 打开文件路径
        /// </summary>
        private void OpenDirCommandExecute()
        {
            string headerStr = CurrentCell.Column.Header.ToString();
            switch (headerStr)
            {
                case ConstantParameter.SrcAddress:
                    {
                        if (CurrentCell.Item != null)
                        {
                            CopyInfoModel copyInfoModel = CurrentCell.Item as CopyInfoModel;
                            string path = Path.GetDirectoryName(copyInfoModel.SourceAddress);
                            Process.Start("explorer.exe", path);
                        }
                        break;
                    }
                case ConstantParameter.TargetAddress:
                    {
                        if (CurrentCell.Item != null)
                        {
                            CopyInfoModel copyInfoModel = CurrentCell.Item as CopyInfoModel;
                            string path = Path.GetDirectoryName(copyInfoModel.TargetAddress);
                            Process.Start("explorer.exe", path);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// 打开设置
        /// </summary>
        private void SetCommandExecute()
        {
            SettingView settingView = new SettingView();
            SettingViewModel settingViewModel = new SettingViewModel(settingView);
            settingView.DataContext = settingViewModel;
            settingView.ShowDialog();
        }
        /// <summary>
        /// 更新数量
        /// </summary>
        private void UpdateCount()
        {
            Count = CopyInfos.Count;
        }
        public static TcpClient TcpClient;
        public static NetworkStream NetworkStream;
        private void CheckUpdateCommandExecute()
        {
            //向局域网发送广播消息
            UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));


            //if (TcpClient == null) 
            //{
            //    TcpClient = new TcpClient("192.168.2.58", 54321);
            //}
            //NetworkStream = TcpClient.GetStream();
            //var message = "GetVersion";
            //byte[] bytes = Encoding.ASCII.GetBytes(message);
            //NetworkStream.Write(bytes, 0, bytes.Length);
            //Task.Run(() =>
            //{
            //    Byte[] receiveData = new Byte[256];
            //    int count;
            //    while ((count = NetworkStream.Read(receiveData, 0, receiveData.Length)) != 0)
            //    {
            //        string str = Encoding.ASCII.GetString(receiveData, 0, count);
            //        System.Windows.MessageBox.Show(str);
            //    }
            //});
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        private void OpenFileCommandExecute()
        {
            try
            {
                string headerStr = CurrentCell.Column.Header.ToString();
                switch (headerStr)
                {
                    case ConstantParameter.SrcAddress:
                        {
                            if (CurrentCell.Item != null)
                            {
                                CopyInfoModel copyInfoModel = CurrentCell.Item as CopyInfoModel;
                                if (!string.IsNullOrEmpty(copyInfoModel.SourceAddress))
                                {
                                    if (File.Exists(copyInfoModel.SourceAddress))
                                    {
                                        string fileExt = Path.GetExtension(copyInfoModel.SourceAddress).TrimStart('.');
                                        DetailSetModel openfileSet = ConfigHelper.DetailSetModels.Find(x => !string.IsNullOrEmpty(x.Value) && x.Type == ConstantParameter.OpenFileSet && x.Value.Contains(fileExt));
                                        if (openfileSet != null)
                                        {
                                            if (File.Exists(openfileSet.CName))
                                            {
                                                Process.Start(openfileSet.CName, copyInfoModel.SourceAddress);
                                            }
                                            else
                                            {
                                                string msg = string.Format("执行程序（{0}）不存在", openfileSet.CName);
                                                System.Windows.MessageBox.Show(msg, "提示");
                                            }
                                        }
                                        else
                                        {
                                            ProcessStartInfo processStartInfo = new ProcessStartInfo();
                                            processStartInfo.UseShellExecute = true;
                                            processStartInfo.Verb = "runas";
                                            processStartInfo.FileName = copyInfoModel.SourceAddress;
                                            //processStartInfo.Arguments = copyInfoModel.SourceAddress;
                                            Process.Start(processStartInfo);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            GetCopyInfos();
            GetModes();
        }
        /// <summary>
        /// 获取复制列表数据
        /// </summary>
        private void GetCopyInfos()
        {
            try
            {
                List<CopyInfoModel> copyInfoModels = SqliteHelper.FastCopyDbContext.CopyInfos.OrderBy(x => x.Sort).ToList();
                foreach (CopyInfoModel copyInfoModel in copyInfoModels)
                {
                    string fileExt = Path.GetExtension(copyInfoModel.SourceAddress).TrimStart('.');
                    if (!string.IsNullOrEmpty(fileExt))
                    {
                        DetailSetModel openfileSet = ConfigHelper.DetailSetModels.Find(x => !string.IsNullOrEmpty(x.Value) && x.Type == ConstantParameter.OpenFileSet && x.Value.Contains(fileExt));
                        if (openfileSet != null)
                        {
                            if (File.Exists(openfileSet.CName))
                            {
                                Icon icon = Icon.ExtractAssociatedIcon(openfileSet.CName);
                                Bitmap bitmap = icon.ToBitmap();
                                BitmapImage image = new BitmapImage();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    bitmap.Save(ms, ImageFormat.Png);
                                    image.BeginInit();
                                    image.StreamSource = ms;
                                    image.CacheOption = BitmapCacheOption.OnLoad;
                                    image.EndInit();
                                    image.Freeze();
                                }
                                copyInfoModel.ExeIcon = image;
                            }
                        }
                        else
                        {
                            Icon icon = Icon.ExtractAssociatedIcon(copyInfoModel.SourceAddress);
                            Bitmap bitmap = icon.ToBitmap();
                            BitmapImage image = new BitmapImage();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bitmap.Save(ms, ImageFormat.Png);
                                image.BeginInit();
                                image.StreamSource = ms;
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.EndInit();
                                image.Freeze();
                            }
                            copyInfoModel.ExeIcon = image;
                        }
                    }
                    CopyInfos.Add(copyInfoModel);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 获取模式
        /// </summary>
        private void GetModes()
        {
            DetailSetModels = SqliteHelper.FastCopyDbContext.DetailSetModels.ToList();
            if (ConfigHelper.IsSaveModes)
            {
                foreach (DetailSetModel detailSetModel in DetailSetModels)
                {
                    switch (detailSetModel.EName)
                    {
                        case "AutoIsChecked":
                            {
                                AutoIsChecked = detailSetModel.Value == "1" ? true : false;
                                break;
                            }
                        case "BakIsChecked":
                            {
                                BakIsChecked = detailSetModel.Value == "1" ? true : false;
                                break;
                            }
                        case "SyncIsChecked":
                            {
                                SyncIsChecked = detailSetModel.Value == "1" ? true : false;
                                break;
                            }
                        case "ClockIsChecked":
                            {
                                ClockIsChecked = detailSetModel.Value == "1" ? true : false;
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// 文件夹监控启动
        /// </summary>
        private async void FileWatctStart()
        {
            await Task.Run(() =>
            {
                while (AutoIsChecked)
                {
                    CopyCommandExecute();
                }
            });
        }
        /// <summary>
        /// 显示操作模式
        /// </summary>
        private void ShowCopyMode()
        {
            CopyMode = string.Empty;
            if (AutoIsChecked)
            {
                CopyMode += ConstantParameter.AutoMode + " ";
            }
            if (BakIsChecked)
            {
                CopyMode += ConstantParameter.BakMode + " ";
            }
            if (SyncIsChecked)
            {
                CopyMode += ConstantParameter.SyncMode + " ";
            }
            if (ClockIsChecked)
            {
                CopyMode += ConstantParameter.ClockMode + " ";
            }
        }
        /// <summary>
        /// 添加ftp地址
        /// </summary>
        public void AddFtpPathCommandExecute()
        {
            FtpViewModel ftpViewModel = new FtpViewModel();
            FtpView ftpView = new FtpView();
            ftpView.DataContext = ftpViewModel;
            ftpView.ShowDialog();
        }
        /// <summary>
        /// 保存复制列表数据
        /// </summary>
        private void SaveCopyInfos()
        {
            try
            {
                for (int i = 0; i < CopyInfos.Count; i++)
                {
                    CopyInfos[i].Sort = i.ToString();
                    SqliteHelper.FastCopyDbContext.CopyInfos.Update(CopyInfos[i]);
                }
                SqliteHelper.FastCopyDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 保存复制列表数据
        /// </summary>
        private void SaveModes()
        {
            try
            {
                foreach (DetailSetModel detailSetModel in DetailSetModels)
                {
                    DetailSetModel detailSet = SqliteHelper.FastCopyDbContext.DetailSetModels.ToList().Find(x => x.EName == detailSetModel.EName);
                    if (detailSet != null)
                    {
                        SqliteHelper.FastCopyDbContext.DetailSetModels.Update(detailSet);
                    }
                    else
                    {
                        SqliteHelper.FastCopyDbContext.DetailSetModels.Add(detailSet);
                    }
                }
                SqliteHelper.FastCopyDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 保存复制模式
        /// </summary>
        private void SetMode(string eName, string cName, string value)
        {
            DetailSetModel detailSet = DetailSetModels.Find(x => x.EName == eName);
            if (detailSet != null)
            {
                foreach (DetailSetModel detailSetModel in DetailSetModels)
                {
                    if (detailSetModel.EName == eName)
                    {
                        detailSetModel.Value = value;
                    }
                }
            }
            else
            {
                DetailSetModel detailSetModel = new DetailSetModel();
                detailSetModel.EName = eName;
                detailSetModel.CName = cName;
                detailSetModel.Value = value;
                DetailSetModels.Add(detailSetModel);
                SqliteHelper.FastCopyDbContext.DetailSetModels.Add(detailSetModel);
            }

        }
        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="succCopyInfos"></param>
        /// <param name="failCopyInfos"></param>
        private void Notice(List<CopyInfoModel> succCopyInfos, List<CopyInfoModel> failCopyInfos)
        {
            if (AutoIsChecked && ConfigHelper.IsCopyNewFile||!AutoIsChecked)
            {
                if (succCopyInfos.Count > 0 || failCopyInfos.Count > 0)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        NoticeViewModel noticeViewModel = new NoticeViewModel(succCopyInfos, failCopyInfos);
                        NoticeView noticeView = new NoticeView();
                        noticeView.DataContext = noticeViewModel;
                        noticeView.Show();
                    });
                }
            }
        }
    }
}
