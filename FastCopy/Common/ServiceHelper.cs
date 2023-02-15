using FastCopy.Config;
using FastCopy.DataBase;
using FastCopy.log;
using FastCopy.Net;
using FastCopy.ViewModels;
using FastCopy.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Common
{
    public class ServiceHelper
    {
        public static ServiceProvider ServiceProvider;
        public static IServiceCollection Init()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IMigrationDb, MigrationDb>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ITcpService, TcpService>();
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<FastCopyDbContext>();

            #region 界面相关
            services.AddScoped<FastCopyView>();
            services.AddScoped<FastCopyViewModel>();
            services.AddScoped<SettingView>();
            services.AddScoped<SettingViewModel>();
            services.AddScoped<FtpView>();
            services.AddScoped<FtpViewModel>();
            ServiceProvider = services.BuildServiceProvider();
            #endregion 界面相关
            return services;
        }
    }
}
