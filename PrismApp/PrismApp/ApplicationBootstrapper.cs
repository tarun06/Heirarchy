using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace PrismApp
{
    public static class ApplicationBootstrapper
    {
        public static IConfiguration BuildConfiguration(ISystemFolders systemFolders)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            foreach (var folder in systemFolders.ConfigurationFolders)
            {
                var filePath = GetEnvConfig(folder);
                if (!File.Exists(filePath))
                {
                    continue;
                }

                builder = builder
                  // Load base config file for all environments.
                  .AddJsonFile(filePath, optional: false, reloadOnChange: false);
            }
            return builder.Build();

            string GetEnvConfig(string folder)
            {
                return Path.Combine(folder, "appsettings.json");
            }
        }

        public static void InitializeLogging(IConfiguration configuration, ISystemFolders systemFolders)
        {
            if (!Enum.TryParse<LogEventLevel>(configuration["Serilog:MinimumLevel:Default"], out var logLevel))
            {
                logLevel = LogEventLevel.Information;
            }

            var path = Path.Combine(systemFolders.LogsFolder);
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Is(logLevel)
                 .WriteTo.File(path, rollingInterval : RollingInterval.Day)
                 .CreateLogger();
        }
    }
}