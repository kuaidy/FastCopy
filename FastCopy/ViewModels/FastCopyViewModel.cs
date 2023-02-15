using FastCopy.Basic;
using FastCopy.Common;
using FastCopy.Config;
using FastCopy.DataBase;
using FastCopy.Models;
using FastCopy.Net;
using FastCopy.Views;
using System;
using System.Collections;
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
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace FastCopy.ViewModels
{
    public class FastCopyViewModel : NotifactionObject
    {
        #region 属性
        private readonly FastCopyDbContext m_FastCopyDbContext;
        private readonly IMigrationDb m_MigrationDb;
        private readonly ITcpService m_TcpService;
        private readonly IConfigService m_ConfigService;

        System.Threading.Timer m_Timer;

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
        /// 复制信息
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
        private List<CopyInfoModel> AllCopyInfos = new List<CopyInfoModel>();
        private List<CopyInfoModel> ShowCopyInfos = new List<CopyInfoModel>();
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
                if (m_AutoIsChecked)
                {
                    FileWatctStart();
                    //CopyFilesExecute();
                }
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
        /// <summary>
        /// 当前单元格
        /// </summary>
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
        /// <summary>
        /// 当前行索引
        /// </summary>
        private int m_CurrentIndex;
        public int CurrentIndex
        {
            get
            {
                return m_CurrentIndex;
            }
            set
            {
                m_CurrentIndex = value;
                this.RaisePropertyChange("CurrentIndex");
            }
        }
        private CopyInfoModel m_CurrentItem;
        public CopyInfoModel CurrentItem
        {
            get
            {
                return m_CurrentItem;
            }
            set
            {
                m_CurrentItem = value;
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

        /// <summary>
        /// 检索内容
        /// </summary>
        private string m_SearchText;
        public string SearchText
        {
            get
            {
                return m_SearchText;
            }
            set
            {
                m_SearchText = value;
                Search();
                this.RaisePropertyChange("SearchText");
            }
        }

        #endregion 属性
        #region 命令
        public ICommand AddCommand
        {
            get; set;
        }
        public ICommand AddChildCommand { get; set; }
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
        public ICommand ExpendedCommand { get; set; }
        public ICommand PackCommand { get; set; }
        public ICommand TaskPauseCommand { get; set; }
        public ICommand TaskCancelCommand { get; set; }
        #endregion 命令

        public FastCopyViewModel()
        {
            InitCommand();
        }
        public FastCopyViewModel(FastCopyDbContext fastCopyDbContext, IMigrationDb migrationDb, ITcpService tcpService, IConfigService configService)
        {
            m_FastCopyDbContext = fastCopyDbContext;
            m_MigrationDb = migrationDb;
            m_TcpService = tcpService;
            m_ConfigService = configService;

            m_Timer = new System.Threading.Timer(NoticeWindowChange, null, 5000, 5000);

            InitTcpService(tcpService);
            InitCommand();
            InitData();

        }

        private void InitTcpService(ITcpService tcpService)
        {
            if (m_TcpService != null)
            {
                m_TcpService.Listen();
            }
        }
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FastCopyView_Closed(object sender, EventArgs e)
        {
            SaveCopyInfos();
            SaveModes();
        }
        private void InitCommand()
        {
            AddCommand = new DelegateCommand(AddCommandExecute);
            AddChildCommand = new DelegateCommand(AddChildCommandExecute);
            CopyCommand = new DelegateCommand(CopyFilesExecute);
            DelCommand = new DelegateCommand(DelCommandExecute);
            SelectFileCommand = new DelegateCommand<object>(SelectFileCommandExecute);
            SelectDirCommand = new DelegateCommand(SelectDirCommandExecute);
            OpenDirCommand = new DelegateCommand(OpenDirCommandExecute);
            SetCommand = new DelegateCommand(SetCommandExecute);
            CheckUpdateCommand = new DelegateCommand(CheckUpdateCommandExecute);
            AddFtpPathCommand = new DelegateCommand(AddFtpPathCommandExecute);
            OpenFileCommand = new DelegateCommand(OpenFileCommandExecute);
            ExpendedCommand = new DelegateCommand<object>(ExpendedCommandExecute);
            PackCommand = new DelegateCommand(PackCommandExecute);
            TaskPauseCommand = new DelegateCommand<object>(TaskPauseCommandExecute);
            TaskCancelCommand = new DelegateCommand<object>(TaskCancelCommandExecute);
        }
        /// <summary>
        /// 添加同级
        /// </summary>
        private void AddCommandExecute()
        {
            try
            {
                CopyInfoModel copyInfoModel = new CopyInfoModel();
                copyInfoModel.Guid = Guid.NewGuid().ToString();
                copyInfoModel.SourceAddress = "";
                copyInfoModel.TargetAddress = "";
                if (CurrentCell.Item != null)
                {
                    CopyInfoModel currentItem = CurrentCell.Item as CopyInfoModel;
                    copyInfoModel.ParentId = currentItem.ParentId;
                    copyInfoModel.GridMargin = currentItem.GridMargin;
                }
                //CopyInfos.Add(copyInfoModel);
                if (CurrentIndex >= 0)
                {
                    CopyInfos.Insert(CurrentIndex + 1, copyInfoModel);
                    ShowCopyInfos.Insert(CurrentIndex + 1, copyInfoModel);
                }
                else
                {
                    CopyInfos.Add(copyInfoModel);
                    ShowCopyInfos.Add(copyInfoModel);
                }
                m_FastCopyDbContext.CopyInfos.Add(copyInfoModel);
                UpdateCount();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 添加子级
        /// </summary>
        private void AddChildCommandExecute()
        {
            try
            {
                CopyInfoModel copyInfoModel = new CopyInfoModel();
                copyInfoModel.Guid = Guid.NewGuid().ToString();
                copyInfoModel.SourceAddress = "";
                copyInfoModel.TargetAddress = "";
                if (CurrentCell.Item != null)
                {
                    CopyInfoModel currentItem = CurrentCell.Item as CopyInfoModel;
                    copyInfoModel.ParentId = currentItem.Guid;
                    double leftMargin = currentItem.GridMargin.Left;
                    copyInfoModel.GridMargin = new Thickness(10 + leftMargin, 0, 0, 0);
                    currentItem.Children.Add(copyInfoModel);
                }
                //CopyInfos.Add(copyInfoModel);
                if (CurrentIndex >= 0)
                {
                    CopyInfos.Insert(CurrentIndex + 1, copyInfoModel);
                    ShowCopyInfos.Insert(CurrentIndex + 1, copyInfoModel);
                }
                else
                {
                    CopyInfos.Add(copyInfoModel);
                    ShowCopyInfos.Add(copyInfoModel);
                }
                m_FastCopyDbContext.CopyInfos.Add(copyInfoModel);
                UpdateCount();
            }
            catch (Exception ex)
            {

            }
        }
        List<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();
        /// <summary>
        /// 文件夹监控启动
        /// </summary>
        private async void FileWatctStart()
        {
            foreach (CopyInfoModel copyInfoModel in CopyInfos)
            {
                if (copyInfoModel.IsChecked)
                {
                    if (File.Exists(copyInfoModel.SourceAddress))
                    {
                        string path = Path.GetDirectoryName(copyInfoModel.SourceAddress);
                        var watcher = new FileSystemWatcher(path);
                        watcher.NotifyFilter = NotifyFilters.Attributes |
                                               NotifyFilters.CreationTime |
                                               NotifyFilters.DirectoryName |
                                               NotifyFilters.FileName |
                                               NotifyFilters.LastAccess |
                                               NotifyFilters.LastWrite |
                                               NotifyFilters.Security |
                                               NotifyFilters.Size;
                        watcher.Changed += FileChanged;
                        watcher.Created += FileChanged;
                        watcher.Deleted += FileChanged;
                        watcher.Error += Watcher_Error;

                        watcher.IncludeSubdirectories = true;
                        watcher.EnableRaisingEvents = true;
                    }
                    else if (Directory.Exists(copyInfoModel.SourceAddress))
                    {
                        var watcher = new FileSystemWatcher(copyInfoModel.SourceAddress);
                        watcher.NotifyFilter = NotifyFilters.Attributes |
                                               NotifyFilters.CreationTime |
                                               NotifyFilters.DirectoryName |
                                               NotifyFilters.FileName |
                                               NotifyFilters.LastAccess |
                                               NotifyFilters.LastWrite |
                                               NotifyFilters.Security |
                                               NotifyFilters.Size;
                        watcher.Changed += FileChanged;
                        watcher.Created += FileChanged;
                        watcher.Deleted += FileChanged;

                        watcher.IncludeSubdirectories = true;
                        watcher.EnableRaisingEvents = true;
                    }
                }
            }
        }

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 文件发生改变，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileChanged(object sender, FileSystemEventArgs e)
        {
            Task.Run(() =>
            {
                CopyCommandExecute();
            });
        }

        private async void CopyFilesExecute()
        {
            await Task.Run(() =>
            {
                //do
                //{
                CopyCommandExecute();
                //}
                //while (AutoIsChecked);
            });
        }


        /// <summary>
        /// 复制
        /// </summary>
        private void CopyCommandExecute()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                int taskNum = 3;
                TaskFactory taskFactory = new TaskFactory();
                List<CopyInfoModel> succCopyInfoModels = new List<CopyInfoModel>();
                List<CopyInfoModel> failCopyInfoModels = new List<CopyInfoModel>();
                List<CopyInfoModel> needCopyInfos = new List<CopyInfoModel>();
                foreach (CopyInfoModel copyInfoModel in CopyInfos)
                {
                    if (copyInfoModel.IsChecked)
                    {
                        needCopyInfos.Add(copyInfoModel);
                    }
                }
                foreach (CopyInfoModel copyInfo in needCopyInfos)
                {
                    //CopyInfoModel copyInfo = CopyInfos[i];
                    var cancellationTokenSource = new CancellationTokenSource();
                    var manualResetEvent = new ManualResetEvent(true);
                    copyInfo.CancellationTokenSource = cancellationTokenSource;
                    copyInfo.ManualResetEvent = manualResetEvent;
                    var task = taskFactory.StartNew(() =>
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(copyInfo.TargetAddress, @"([0-9]*)\.([0-9]*)\.([0-9]*)\.([0-9]*)"))
                        {
                            string fileName = Path.GetFileName(copyInfo.SourceAddress);
                            SendFileMessage(copyInfo.TargetAddress, 54321, fileName);
                        }
                        else
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
                                        ChangeStatuColor(copyInfo, false);
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
                                        if (m_ConfigService.IsCopyNewFile)
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
                                                    ChangeStatuColor(copyInfo, true);
                                                }
                                            }
                                            else
                                            {
                                                File.Copy(copyInfo.SourceAddress, destFileName, true);
                                                copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                copyInfo.Status = string.Format("成功{0}个，失败{1}个", 1, 0);
                                                succCopyInfoModels.Add(copyInfo);
                                                ChangeStatuColor(copyInfo, true);
                                            }
                                        }
                                        else
                                        {
                                            File.Copy(copyInfo.SourceAddress, destFileName, true);
                                            copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            copyInfo.Status = string.Format("成功{0}个，失败{1}个", 1, 0);
                                            succCopyInfoModels.Add(copyInfo);
                                            ChangeStatuColor(copyInfo, true);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    copyInfo.Status = string.Format("成功{0}个，失败{1}个", 0, 1);
                                    failCopyInfoModels.Add(copyInfo);
                                    ChangeStatuColor(copyInfo, false);
                                }
                            }
                            else if (Directory.Exists(copyInfo.SourceAddress))
                            {
                                int successNum = 0;
                                int failNum = 0;
                                int fileNum = 0;
                                ScanFolder(copyInfo.SourceAddress, ref fileNum);
                                copyInfo.ProgressMaxNum = fileNum;
                                copyInfo.ProgressValue = 0;
                                copyInfo.Status = string.Empty;
                                copyInfo.IsPauseVisible = Visibility.Visible;
                                CopyFolder(copyInfo.SourceAddress, copyInfo.TargetAddress, ref successNum, ref failNum, copyInfo);
                                copyInfo.CopyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                copyInfo.Status = string.Format("成功{0}个，失败{1}个", successNum, failNum);
                                copyInfo.IsPauseVisible = Visibility.Collapsed;
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
                    }, cancellationTokenSource.Token);
                    copyInfo.CopyTask = task;
                    tasks.Add(task);
                    if (tasks.Count >= taskNum)
                    {
                        //控制任务数量
                        Task.WaitAny(tasks.ToArray());
                        tasks = tasks.Where(t => t.Status == TaskStatus.Running).ToList();
                    }
                }
                Task.WaitAll(tasks.ToArray());
                Notice(succCopyInfoModels, failCopyInfoModels);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 扫描文件夹的文件
        /// </summary>
        private void ScanFolder(string sourceFolder, ref int fileNum)
        {
            string[] files = Directory.GetFiles(sourceFolder);
            fileNum += files.Count();
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                ScanFolder(folder, ref fileNum);
            }
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        private void CopyFolder(string sourceFolder, string destFolder, ref int successNum, ref int failNum, CopyInfoModel copyInfo)
        {
            try
            {
                if (copyInfo.CancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
                copyInfo.ManualResetEvent.WaitOne();
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }
                string[] files = Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    try
                    {
                        //取消线程复制
                        if (copyInfo.CancellationTokenSource.Token.IsCancellationRequested)
                        {
                            return;
                        }
                        //暂停线程复制
                        copyInfo.ManualResetEvent.WaitOne();

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

                        if (m_ConfigService.IsCopyNewFile)
                        {
                            if (File.Exists(destFileName))
                            {
                                DateTime sourceFileTime = File.GetLastWriteTime(copyInfo.SourceAddress);
                                DateTime targetFileTime = File.GetLastWriteTime(destFileName);
                                TimeSpan timeSpan = sourceFileTime - targetFileTime;
                                if (timeSpan.TotalMilliseconds > 0)
                                {
                                    File.Copy(file, destFileName, true);
                                    successNum++;
                                    copyInfo.ProgressValue++;
                                    copyInfo.Status = string.Format("{0}/{1}", copyInfo.ProgressValue, copyInfo.ProgressMaxNum);
                                }
                            }
                            else
                            {
                                File.Copy(file, destFileName, true);
                                successNum++;
                                copyInfo.ProgressValue++;
                                copyInfo.Status = string.Format("{0}/{1}", copyInfo.ProgressValue, copyInfo.ProgressMaxNum);
                            }
                        }
                        else
                        {
                            File.Copy(file, destFileName, true);
                            successNum++;
                            copyInfo.ProgressValue++;
                            copyInfo.Status = string.Format("{0}/{1}", copyInfo.ProgressValue, copyInfo.ProgressMaxNum);
                        }
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
                    CopyFolder(folder, dest, ref successNum, ref failNum, copyInfo);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 上传到ftp服务器
        /// </summary>
        private void CopyToFtp()
        {

        }
        /// <summary>
        /// 发送文件消息
        /// </summary>
        private void SendFileMessage(string ip, int port, string fileName)
        {
            IPAddress localIp = Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).First();
            string message = string.Empty;
            message = localIp + ":" + fileName;
            m_TcpService.CopyInfos = CopyInfos;
            m_TcpService.SendMessage(message, ip, port);
        }

        private void M_TcpService_ChangeCopyInfoEven()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        private void DelCommandExecute()
        {
            if (System.Windows.MessageBox.Show("是否删除选中的记录？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //List<CopyInfoModel> copyInfoModels = CopyInfos.Where(x => x.IsChecked == true).ToList();
                //foreach (CopyInfoModel copyInfoModel in copyInfoModels)
                //{
                //    CopyInfos.Remove(copyInfoModel);
                //    CopyInfoModel dbCopyInfoModel = m_FastCopyDbContext.CopyInfos.Single(x => x.Id == copyInfoModel.Id);
                //    if (dbCopyInfoModel != null)
                //    {
                //        m_FastCopyDbContext.CopyInfos.Remove(dbCopyInfoModel);
                //    }
                //}

                for (int i = 0; i < FastCopyView.dataGrid.SelectedItems.Count; i++)
                {
                    if (FastCopyView.dataGrid.SelectedItems[i] is CopyInfoModel)
                    {
                        CopyInfoModel copyInfoModel = FastCopyView.dataGrid.SelectedItems[i] as CopyInfoModel;
                        CopyInfos.Remove(copyInfoModel);
                        ShowCopyInfos.Remove(copyInfoModel);
                        m_FastCopyDbContext.CopyInfos.Remove(copyInfoModel);
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
                                        DetailSetModel openfileSet = m_ConfigService.DetailSetModels.Find(x => !string.IsNullOrEmpty(x.Value) && x.Type == ConstantParameter.OpenFileSet && x.Value.Contains(fileExt));
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

        private void ExpendedCommandExecute(object obj)
        {
            if (obj is CopyInfoModel)
            {
                CopyInfoModel copyInfoModel = obj as CopyInfoModel;
                if (copyInfoModel != null)
                {
                    if (copyInfoModel.IsExpended == true)
                    {
                        copyInfoModel.IsExpended = false;
                        copyInfoModel.TriangleType = TriangleType.NotFill;
                        RemoveChildren(copyInfoModel);
                    }
                    else if (copyInfoModel.IsExpended == false)
                    {
                        copyInfoModel.IsExpended = true;
                        copyInfoModel.TriangleType = TriangleType.Fill;

                        GetChildren(copyInfoModel);
                    }
                }
            }
        }
        private void GetChildren(CopyInfoModel copyInfoModel)
        {
            List<CopyInfoModel> copyInfoModels = AllCopyInfos.Where(x => x.ParentId == copyInfoModel.Guid).ToList();
            int index = CopyInfos.IndexOf(copyInfoModel);
            foreach (CopyInfoModel copyInfo in copyInfoModels)
            {
                index++;
                double left = copyInfoModel.GridMargin.Left;
                copyInfo.GridMargin = new Thickness(left + 10, 0, 0, 0);
                copyInfo.ExeIcon = ImageHelper.GetFileIcon(copyInfo.SourceAddress);
                CopyInfoModel tmpCopyInfo = AllCopyInfos.Find(x => x.ParentId == copyInfo.Guid);
                if (tmpCopyInfo == null)
                {
                    copyInfo.IsVisible = Visibility.Collapsed;
                }
                else
                {
                    copyInfo.IsVisible = Visibility.Visible;
                }
                copyInfoModel.TriangleType = TriangleType.Fill;
                CopyInfos.Insert(index, copyInfo);
                ShowCopyInfos.Insert(index, copyInfo);
            }
        }
        private void RemoveChildren(CopyInfoModel copyInfoModel)
        {
            List<CopyInfoModel> copyInfoModels = AllCopyInfos.Where(x => x.ParentId == copyInfoModel.Guid).ToList();
            foreach (CopyInfoModel copyInfo in copyInfoModels)
            {
                RemoveChildren(copyInfo);
                CopyInfos.Remove(copyInfo);
                ShowCopyInfos.Remove(copyInfo);
            }
        }

        private void Search()
        {
            CopyInfos.Clear();
            if (string.IsNullOrEmpty(SearchText))
            {
                foreach (CopyInfoModel copyInfoModel in ShowCopyInfos)
                {
                    CopyInfos.Add(copyInfoModel);
                }
            }
            else
            {
                List<CopyInfoModel> copyInfoModels = ShowCopyInfos.Where(x => x.SourceAddress.Contains(SearchText) || x.TargetAddress.Contains(SearchText)).ToList();
                foreach (CopyInfoModel copyInfoModel in copyInfoModels)
                {
                    CopyInfos.Add(copyInfoModel);
                }
            }
        }

        /// <summary>
        /// 文件打包，将层级目录带上
        /// </summary>
        private void PackCommandExecute()
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    List<CopyInfoModel> needPackFiles = CopyInfos.Where(x => x.IsChecked).ToList();
                    foreach (CopyInfoModel needPackFile in needPackFiles)
                    {
                        if (File.Exists(needPackFile.SourceAddress))
                        {
                            string tmpPath = Path.Combine(folderBrowserDialog.SelectedPath, "package");
                            if (!Directory.Exists(tmpPath))
                            {
                                Directory.CreateDirectory(tmpPath);
                            }
                            //string[] paths = needPackFile.SourceAddress.Split(new char[] { '\\', '/' });
                            //for (int i = 0; i < paths.Count(); i++)
                            //{
                            //    if (i > 0 && i < paths.Count() - 1)
                            //    {
                            //        tmpPath = Path.Combine(tmpPath, paths[i]);
                            //        if (!Directory.Exists(tmpPath))
                            //        {
                            //            Directory.CreateDirectory(tmpPath);
                            //        }
                            //    }
                            //}
                            File.Copy(needPackFile.SourceAddress, Path.Combine(tmpPath, Path.GetFileName(needPackFile.SourceAddress)), true);
                        }
                    }
                    System.Windows.MessageBox.Show("打包成功", "提示");
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 暂停复制
        /// </summary>
        /// <param name="obj"></param>
        private void TaskPauseCommandExecute(object obj)
        {
            var item = obj as CopyInfoModel;
            if (item.ManualResetEvent != null)
            {
                if (item.IsCopyPaused)
                {
                    item.ManualResetEvent.Set();
                    item.IsCopyPaused = false;
                }
                else
                {
                    item.ManualResetEvent.Reset();
                    item.IsCopyPaused = true;
                }
            }
        }
        /// <summary>
        /// 取消复制
        /// </summary>
        private void TaskCancelCommandExecute(object obj) 
        {
            var item = obj as CopyInfoModel;
            if (item.CancellationTokenSource != null)
            {
                item.ManualResetEvent.Set();
                item.CancellationTokenSource.Cancel();
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
                AllCopyInfos = m_FastCopyDbContext.CopyInfos.ToList();
                List<CopyInfoModel> copyInfoModels = m_FastCopyDbContext.CopyInfos.Where(x => x.ParentId == null || string.IsNullOrEmpty(x.ParentId)).OrderBy(x => x.Sort).ToList();
                foreach (CopyInfoModel copyInfoModel in copyInfoModels)
                {
                    string fileExt = Path.GetExtension(copyInfoModel.SourceAddress).TrimStart('.');
                    if (!string.IsNullOrEmpty(fileExt))
                    {
                        DetailSetModel openfileSet = m_ConfigService.DetailSetModels.Find(x => !string.IsNullOrEmpty(x.Value) && x.Type == ConstantParameter.OpenFileSet && x.Value.Contains(fileExt));
                        if (openfileSet != null)
                        {
                            if (File.Exists(openfileSet.CName))
                            {
                                copyInfoModel.ExeIcon = ImageHelper.GetFileIcon(openfileSet.CName);
                            }
                        }
                        else
                        {
                            copyInfoModel.ExeIcon = ImageHelper.GetFileIcon(copyInfoModel.SourceAddress);
                        }
                    }
                    else
                    {
                        copyInfoModel.ExeIcon = ImageHelper.GetFileIcon(copyInfoModel.SourceAddress);
                    }
                    CopyInfoModel copyInfo = AllCopyInfos.Find(x => x.ParentId == copyInfoModel.Guid);
                    if (copyInfo == null)
                    {
                        copyInfoModel.IsVisible = Visibility.Collapsed;
                    }
                    else
                    {
                        copyInfoModel.IsVisible = Visibility.Visible;
                    }
                    CopyInfos.Add(copyInfoModel);
                    ShowCopyInfos.Add(copyInfoModel);
                    if (copyInfoModel.IsExpended == true)
                    {
                        GetChildren(copyInfoModel);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 获取模式
        /// </summary>
        private void GetModes()
        {
            DetailSetModels = m_FastCopyDbContext.DetailSetModels.ToList();
            if (m_ConfigService.IsSaveModes)
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
            FtpViewModel ftpViewModel = ServiceHelper.ServiceProvider.GetService<FtpViewModel>();
            FtpView ftpView = ServiceHelper.ServiceProvider.GetService<FtpView>();
            ftpViewModel.InitData(CurrentItem);
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
                    CopyInfoModel copyInfoModel = m_FastCopyDbContext.CopyInfos.ToList().Find(x => x.Id == CopyInfos[i].Id);
                    if (copyInfoModel != null)
                    {
                        m_FastCopyDbContext.CopyInfos.Update(CopyInfos[i]);
                    }
                    else
                    {
                        m_FastCopyDbContext.CopyInfos.Add(CopyInfos[i]);
                    }
                }
                int count = m_FastCopyDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
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
                    DetailSetModel detailSet = m_FastCopyDbContext.DetailSetModels.ToList().Find(x => x.EName == detailSetModel.EName);
                    if (detailSet != null)
                    {
                        m_FastCopyDbContext.DetailSetModels.Update(detailSet);
                    }
                    else
                    {
                        m_FastCopyDbContext.DetailSetModels.Add(detailSet);
                    }
                }
                m_FastCopyDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
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
                m_FastCopyDbContext.DetailSetModels.Add(detailSetModel);
            }

        }
        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="succCopyInfos"></param>
        /// <param name="failCopyInfos"></param>
        private void Notice(List<CopyInfoModel> succCopyInfos, List<CopyInfoModel> failCopyInfos)
        {
            if (!AutoIsChecked)
            {
                if (succCopyInfos.Count > 0 || failCopyInfos.Count > 0)
                {
                    m_Timer.Dispose();
                    m_Timer = new System.Threading.Timer(NoticeWindowChange, null, 5000, 5000);

                    FastCopyView.Dispatcher.BeginInvoke(() =>
                    {
                        NoticeViewModel noticeViewModel = new NoticeViewModel(succCopyInfos, failCopyInfos);
                        NoticeView noticeView = new NoticeView();
                        ConstantParameter.QueueNoticeWindows.Enqueue(noticeView);
                        noticeView.DataContext = noticeViewModel;
                        noticeView.Topmost = true;
                        noticeView.Left = SystemParameters.WorkArea.Right - noticeView.Width;
                        noticeView.Top = SystemParameters.WorkArea.Bottom;
                        noticeView.Show();
                    });
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void NoticeWindowChange(object state)
        {
            if (ConstantParameter.QueueNoticeWindows.Count > 0)
            {
                var view = ConstantParameter.QueueNoticeWindows.Peek();
                ConstantParameter.QueueNoticeWindows.Dequeue();
                if (view is NoticeView)
                {
                    NoticeView nView = view as NoticeView;
                    nView.Window_Close();
                }
            }
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                foreach (CopyInfoModel copyInfoModel in CopyInfos)
                {
                    if (copyInfoModel.IsChecked)
                    {
                        TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(copyInfoModel.CopyTime);
                        if (timeSpan.TotalSeconds > 10)
                        {
                            copyInfoModel.BackGroundColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                        }
                    }
                }
            });
        }
        /// <summary>
        /// 改变颜色
        /// </summary>
        private void ChangeStatuColor(CopyInfoModel copyInfoModel, bool result)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (result)
                {
                    copyInfoModel.BackGroundColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                }
                else
                {
                    copyInfoModel.BackGroundColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                }
            });
        }
    }
}
