using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Common
{
    public class ConstantParameter
    {
        public const string Version = "1.0";
        public const string SrcAddress = "源地址";
        public const string TargetAddress = "目标地址";

        public const string AutoMode = "自动模式";
        public const string BakMode = "备份模式";
        public const string SyncMode = "同步模式";
        public const string ClockMode = "定时模式";

        /// <summary>
        /// 自动更新消息端口
        /// </summary>
        public const int AutoUpdatePort = 54321;
        /// <summary>
        /// 快捷方式
        /// </summary>
        public const string ShotCut = "FastCopy";
        /// <summary>
        /// windows的自启动路径
        /// </summary>
        public static string SystemStartPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            }
        }
        /// <summary>
        /// 获取程序的完整路径
        /// </summary>
        public static string AppFullPath
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        #region 配置类型
        public const string NormalSet= "常规配置";
        public const string OpenFileSet = "文件打开配置";
        #endregion
    }
}
