using FastCopy.Basic;
using FastCopy.DataBase;
using FastCopy.Models;
using FastCopy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FastCopy.ViewModels
{
    public class FtpViewModel : NotifactionObject
    {
        public CopyInfoModel CopyInfo;
        private readonly FastCopyDbContext _fastCopyDbContext;

        private FtpInfoModel _CurrentFtpInfo = new FtpInfoModel();
        public FtpInfoModel CurrentFtpInfo
        {
            get
            {
                return _CurrentFtpInfo;
            }
            set
            {
                _CurrentFtpInfo = value;
                this.RaisePropertyChange("CurrentFtpInfo");
            }
        }

        private FtpView _ftpView;

        public ICommand TestConnectCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public FtpViewModel(FastCopyDbContext fastCopyDbContext, FtpView ftpView)
        {
            _fastCopyDbContext = fastCopyDbContext;
            _ftpView = ftpView;
            InitCommand();
        }

        public void InitData(CopyInfoModel copyInfo)
        {
            CopyInfo = copyInfo;
            GetFtpInfoByCopyInfoGuid();
        }

        private void GetFtpInfoByCopyInfoGuid()
        {
            FtpInfoModel ftpInfoModel = _fastCopyDbContext.FtpInfos.Where(x => x.CopyInfoId == CopyInfo.Guid).FirstOrDefault();
            if (ftpInfoModel != null)
            {
                CurrentFtpInfo.Guid = ftpInfoModel.Guid;
                CurrentFtpInfo.Ip = ftpInfoModel.Ip;
                CurrentFtpInfo.Port = ftpInfoModel.Port;
                CurrentFtpInfo.Path = ftpInfoModel.Path;
                CurrentFtpInfo.IsPassiveMode = ftpInfoModel.IsPassiveMode;
                CurrentFtpInfo.UserName = ftpInfoModel.UserName;
                CurrentFtpInfo.Password = ftpInfoModel.Password;
                CurrentFtpInfo.CopyInfoId = ftpInfoModel.CopyInfoId;
            }
        }

        private void InitCommand()
        {
            TestConnectCommand = new DelegateCommand(TestConnectCommandExecute);
            ConfirmCommand = new DelegateCommand(ConfirmCommandExecute);
            CancelCommand = new DelegateCommand(CancelCommandExecute);
        }
        private void TestConnectCommandExecute()
        {
            
        }
        private void ConfirmCommandExecute()
        {
            FtpInfoModel ftpInfoModel = _fastCopyDbContext.FtpInfos.Where(x => x.Guid == CurrentFtpInfo.Guid).FirstOrDefault();
            if (ftpInfoModel != null)
            {
                ftpInfoModel.Ip = CurrentFtpInfo.Ip;
                ftpInfoModel.Port = CurrentFtpInfo.Port;
                ftpInfoModel.Path = CurrentFtpInfo.Path;
                ftpInfoModel.UserName = CurrentFtpInfo.UserName;
                ftpInfoModel.Password = CurrentFtpInfo.Password;
                ftpInfoModel.IsPassiveMode = CurrentFtpInfo.IsPassiveMode;
                _fastCopyDbContext.FtpInfos.Update(ftpInfoModel);
            }
            else
            {
                CurrentFtpInfo.Guid = Guid.NewGuid().ToString();
                CurrentFtpInfo.CopyInfoId = CopyInfo.Guid;
                _fastCopyDbContext.FtpInfos.Add(CurrentFtpInfo);
            }
            CopyInfo.TargetAddress = $"ftp://{CurrentFtpInfo.Ip}:{CurrentFtpInfo.Port}{CurrentFtpInfo.Path}";
            _fastCopyDbContext.CopyInfos.Update(CopyInfo);
            _fastCopyDbContext.SaveChanges();

        }
        private void CancelCommandExecute()
        {
            _ftpView.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
