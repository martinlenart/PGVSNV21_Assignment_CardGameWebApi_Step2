using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CardGameWebApi
{
    public sealed class AppConfig
    {
        private static AppConfig _instance = null;
        private static readonly object instanceLock = new();
        private static IConfigurationRoot _configuration;

#if DEBUG
        private string _appsettingfile = "appsettings.Development.json";
#else
        private string _appsettingfile = "appsettings.json";
#endif
        private AppConfig()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(_appsettingfile, optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                lock (instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppConfig();
                    }
                    return _configuration;
                }
            }
        }
    }

    #region Application specific extensions
    //IConfiguration Extensions fitting this applications appsettings profile
    public static class ConfigurationExtensions
    {
        public static Version GetAppVersion(this IConfiguration config)
        {
            var appVersion = AppConfig.ConfigurationRoot.GetSection("AppVersion");

            var major = appVersion.GetValue<int>("major");
            var minor = appVersion.GetValue<int>("minor");
            var build = appVersion.GetValue<int>("build");
            var release = appVersion.GetValue<int>("release");
            return new Version(major, minor, build, release);
        }
        public static string GetAppDeveloper(this IConfiguration config)
        {
            return AppConfig.ConfigurationRoot.GetValue<string>("AppDeveloper");
        }
        public static string GetAppTitle(this IConfiguration config)
        {
            return AppConfig.ConfigurationRoot.GetValue<string>("AppTitle");
        }

        public static string GetAppId(this IConfiguration config) =>
                $"Title: { AppConfig.ConfigurationRoot.GetAppTitle()}" + 
                $"\nVersion: {AppConfig.ConfigurationRoot.GetAppVersion().ToString()}" +
                $"\nDeveloper: {AppConfig.ConfigurationRoot.GetAppDeveloper()}";
    }
    #endregion
}
