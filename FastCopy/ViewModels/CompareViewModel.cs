using FastCopy.Basic;
using FastCopy.Common;
using FastCopy.Migrations;
using FastCopy.Models;
using FastCopy.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace FastCopy.ViewModels
{
    public class CompareViewModel : NotifactionObject
    {
        public CompareViewModel(CompareView compareView)
        {
            m_CompareView = compareView;
            m_CompareView.Loaded += CompareView_Loaded;
            InitCommand();
        }
        ScrollViewer sv1, sv2;
        private void CompareView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //分别获取两个DataGrid的ScrollViewer
            sv1 = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(m_CompareView.SourceDataGrid, 0), 0) as ScrollViewer;
            sv2 = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(m_CompareView.TargetDataGrid, 0), 0) as ScrollViewer;

            //关联ScrollChanged事件
            sv1.ScrollChanged += new ScrollChangedEventHandler(sv1_ScrollChanged);
            sv2.ScrollChanged += new ScrollChangedEventHandler(sv2_ScrollChanged);
        }
        void sv1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            sv2.ScrollToVerticalOffset(sv1.VerticalOffset);
        }

        void sv2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            sv1.ScrollToVerticalOffset(sv2.VerticalOffset);
        }

        private CompareView m_CompareView;
        private string m_SourceDir;
        public string SourceDir
        {
            get
            {
                return m_SourceDir;
            }
            set
            {
                m_SourceDir = value;
                GetFiles(m_SourceDir, SourceFiles);
                this.RaisePropertyChange("SourceDir");
            }
        }
        private ObservableCollection<CompareFileInfo> m_SourceFiles = new ObservableCollection<CompareFileInfo>();
        public ObservableCollection<CompareFileInfo> SourceFiles
        {
            get
            {
                return m_SourceFiles;
            }
            set
            {
                m_SourceFiles = value;
                this.RaisePropertyChange("SourceFiles");
            }
        }
        private CompareFileInfo m_SelectedSourceFile = null;
        public CompareFileInfo SelectedSourceFile
        {
            get
            {
                return m_SelectedSourceFile;
            }
            set
            {
                m_SelectedSourceFile = value;
                this.RaisePropertyChange("SelectedSourceFile");
            }
        }
        private string m_TargetDir;
        public string TargetDir
        {
            get
            {
                return m_TargetDir;
            }
            set
            {
                m_TargetDir = value;
                GetFiles(m_TargetDir, TargetFiles);
                this.RaisePropertyChange("TargetDir");
            }
        }
        private ObservableCollection<CompareFileInfo> m_TargetFiles = new ObservableCollection<CompareFileInfo>();
        public ObservableCollection<CompareFileInfo> TargetFiles
        {
            get
            {
                return m_TargetFiles;
            }
            set
            {
                m_TargetFiles = value;
                this.RaisePropertyChange("TargetFiles");
            }
        }
        private CompareFileInfo m_SelectedTargetFile = new CompareFileInfo();
        public CompareFileInfo SelectedTargetFile
        {
            get
            {
                return m_SelectedTargetFile;
            }
            set
            {
                m_SelectedTargetFile = value;
                this.RaisePropertyChange("SelectedTargetFile");
            }
        }
        private void InitData()
        {

        }
        public ICommand OpenSourceCommand { get; set; }
        public ICommand OpenTargetCommand { get; set; }
        public ICommand SourceFileDoubleClickCommand { get; set; }
        public ICommand TargetFileDoubleClickCommand { get; set; }
        private void InitCommand()
        {
            OpenSourceCommand = new DelegateCommand(OpenSourceCommandExecute);
            OpenTargetCommand = new DelegateCommand(OpenTargetCommandExecute);
            SourceFileDoubleClickCommand = new DelegateCommand(SourceFileDoubleClickCommandExecute);
            TargetFileDoubleClickCommand = new DelegateCommand(TargetFileDoubleClickCommandExecute);
        }
        private void OpenSourceCommandExecute()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                SourceDir = folderBrowserDialog.SelectedPath;
                GetFiles(SourceDir, SourceFiles);
            }
            Compare();
        }
        private void OpenTargetCommandExecute()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {

                TargetDir = folderBrowserDialog.SelectedPath;
                GetFiles(TargetDir, TargetFiles);
            }
            Compare();
        }
        private void GetFiles(string path, ObservableCollection<CompareFileInfo> compareFileInfos)
        {
            compareFileInfos.Clear();
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                CompareFileInfo compareFileInfo = new CompareFileInfo();
                compareFileInfo.Name = directoryInfo.Name;
                compareFileInfo.ModifyTime = directoryInfo.LastWriteTime.ToString();
                compareFileInfo.ExeIcon = ImageHelper.GetFileIcon(directory);
                compareFileInfo.CurrentPath = directoryInfo.FullName;
                compareFileInfo.Level = 0;
                compareFileInfos.Add(compareFileInfo);
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                CompareFileInfo compareFileInfo = new CompareFileInfo();
                compareFileInfo.Name = fileInfo.Name;
                compareFileInfo.Size = fileInfo.Length.ToString();
                compareFileInfo.ModifyTime = fileInfo.LastWriteTime.ToString();
                compareFileInfo.ExeIcon = ImageHelper.GetFileIcon(file);
                compareFileInfo.CurrentPath = fileInfo.FullName;
                compareFileInfo.Level = 0;
                compareFileInfos.Add(compareFileInfo);
            }
        }
        private void Compare()
        {
            if (SourceFiles == null || TargetFiles == null) return;
            if (SourceFiles.Count == 0 || TargetFiles.Count == 0) return;
            //去掉两边的空白
            for (int i = 0; i < SourceFiles.Count(); ++i)
            {
                if (string.IsNullOrEmpty(SourceFiles[i].Name))
                {
                    SourceFiles.Remove(SourceFiles[i]);
                    i--;
                }
            }
            for (int i = 0; i < TargetFiles.Count(); ++i)
            {
                if (string.IsNullOrEmpty(TargetFiles[i].Name))
                {
                    TargetFiles.Remove(TargetFiles[i]);
                    i--;
                }
            }
            for (int i = 0; i < SourceFiles.Count(); ++i)
            {
                if (string.IsNullOrEmpty(SourceFiles[i].Name)) continue;
                int sourceIndex = i;
                var targetFile = TargetFiles.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name == SourceFiles[i].Name&&x.Level== SourceFiles[i].Level);
                if (targetFile != null && targetFile.Count() > 0)
                {
                    if (SourceFiles[i].ModifyTime.CompareTo(targetFile.First().ModifyTime) > 0)
                    {
                        SourceFiles[i].FontColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                    }
                    else if (SourceFiles[i].ModifyTime.CompareTo(targetFile.First().ModifyTime) < 0)
                    {
                        targetFile.First().FontColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                    }
                    int targetIndex = TargetFiles.IndexOf(targetFile.First());
                    for (int j = sourceIndex; j < targetIndex; ++j)
                    {
                        CompareFileInfo compareFileInfo = new CompareFileInfo();
                        SourceFiles.Insert(j, compareFileInfo);
                    }
                }
                else
                {
                    CompareFileInfo compareFileInfo = new CompareFileInfo();
                    TargetFiles.Insert(sourceIndex, compareFileInfo);
                }
            }
        }
        private void SourceFileDoubleClickCommandExecute()
        {
            ExpendDir(SelectedSourceFile,SourceFiles);
            int selectedIndex=SourceFiles.IndexOf(SelectedSourceFile);
            ExpendDir(TargetFiles[selectedIndex], TargetFiles);
        }
        private void TargetFileDoubleClickCommandExecute()
        {
            ExpendDir(SelectedTargetFile, TargetFiles);
            int selectedIndex = TargetFiles.IndexOf(SelectedTargetFile);
            ExpendDir(SourceFiles[selectedIndex], SourceFiles);
        }
        private void ExpendDir(CompareFileInfo compareFileInfo,ObservableCollection<CompareFileInfo> compareFileInfos)
        {
            if (compareFileInfo != null)
            {
                if (!Directory.Exists(compareFileInfo.CurrentPath))
                {
                    return;
                }
                if (!compareFileInfo.IsExpend)
                {
                    int startIndex = compareFileInfos.IndexOf(compareFileInfo); ;
                    string[] directories = Directory.GetDirectories(compareFileInfo.CurrentPath);
                    foreach (string directory in directories)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                        CompareFileInfo tmpCompareFileInfo = new CompareFileInfo();
                        tmpCompareFileInfo.Name = directoryInfo.Name;
                        tmpCompareFileInfo.ModifyTime = directoryInfo.LastWriteTime.ToString();
                        tmpCompareFileInfo.ExeIcon = ImageHelper.GetFileIcon(directory);
                        double leftMargin = SelectedSourceFile.GridMargin.Left;
                        tmpCompareFileInfo.GridMargin = new Thickness(leftMargin + 10, 0, 0, 0);
                        tmpCompareFileInfo.CurrentPath = directoryInfo.FullName;
                        tmpCompareFileInfo.Level = SelectedSourceFile.Level + 1;
                        compareFileInfo.Children.Add(tmpCompareFileInfo);
                        compareFileInfos.Insert(++startIndex, tmpCompareFileInfo);
                    }
                    string[] files = Directory.GetFiles(SelectedSourceFile.CurrentPath);
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        CompareFileInfo tmpCompareFileInfo = new CompareFileInfo();
                        tmpCompareFileInfo.Name = fileInfo.Name;
                        tmpCompareFileInfo.Size = fileInfo.Length.ToString();
                        tmpCompareFileInfo.ModifyTime = fileInfo.LastWriteTime.ToString();
                        tmpCompareFileInfo.ExeIcon = ImageHelper.GetFileIcon(file);
                        double leftMargin = SelectedSourceFile.GridMargin.Left;
                        tmpCompareFileInfo.GridMargin = new Thickness(leftMargin + 10, 0, 0, 0);
                        tmpCompareFileInfo.CurrentPath = fileInfo.FullName;
                        tmpCompareFileInfo.Level = SelectedSourceFile.Level + 1;
                        compareFileInfo.Children.Add(tmpCompareFileInfo);
                        compareFileInfos.Insert(++startIndex, tmpCompareFileInfo);
                    }
                    compareFileInfo.IsExpend = true;
                }
                else
                {
                    RetractDir(compareFileInfo,compareFileInfos);
                    compareFileInfo.IsExpend = false;
                }
                Compare();
            }
        }
        private void RetractDir(CompareFileInfo compareFileInfo, ObservableCollection<CompareFileInfo> compareFileInfos)
        {
            if (compareFileInfo.Children.Count == 0)
            {
                compareFileInfos.Remove(compareFileInfo);
            }
            else
            {
                for (int i = 0; i < compareFileInfo.Children.Count; ++i)
                {
                    RetractDir(compareFileInfo.Children[i], compareFileInfos);
                }
            }
        }
    }
}
