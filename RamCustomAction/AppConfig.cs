using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RamCustomAction
{
    public sealed class AppConfig
    {
        private static AppConfig _instance = null;
        private static readonly object padlock = new object();
        private readonly string _appId = string.Empty;
        private readonly string _appSecret = string.Empty;
        private readonly string _tenantId = string.Empty;

        private AppConfig()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "settings/appsettings.json"), optional: false)
                .Build();

            _appId = configuration.GetValue<string>("MicrosoftAppId");
            _appSecret = configuration.GetValue<string>("MicrosoftAppPassword");
            _tenantId = configuration.GetValue<string>("MicrosoftAppTenantId");
        }

        public static AppConfig Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppConfig();
                    }
                    return _instance;
                }
            }
        }

        public string AppId
        {
            get => _appId;
        }

        public string AppSecret
        {
            get => _appSecret;
        }

        public string TenantId
        {
            get => _tenantId;
        }
    }
}
