using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrismApp.Services
{
    public sealed class SystemFolders : ISystemFolders
    {
        private const string SystemRootFolderName = "SuperVisorApp";

        private readonly string _appDataFolder = GetAppDataFolder();

        private readonly string _baseFolder = AppDomain.CurrentDomain.BaseDirectory;

        private readonly string _systemRootFolder = GetSystemRootFolder();

        public IEnumerable<string> ConfigurationFolders => new[]
        {
            _baseFolder,
            _appDataFolder,
            _systemRootFolder
        }.Where(f => f != null)!;

        public string LogsFolder => GetAppDataFolder();

        private static string GetAppDataFolder()
        {
            var localAppdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return Path.Combine(localAppdata, SystemRootFolderName);
        }

        private static string GetSystemRootFolder()
        {
            var pathRoot = Path.GetPathRoot(Environment.SystemDirectory);
            if (pathRoot != null
                && Directory.Exists(Path.Combine(pathRoot, SystemRootFolderName)))
            {
                return Path.Combine(pathRoot, SystemRootFolderName);
            }

            return null;
        }
    }
}