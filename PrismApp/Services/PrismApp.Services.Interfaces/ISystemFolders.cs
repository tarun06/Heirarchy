using System.Collections.Generic;

public interface ISystemFolders
{
    IEnumerable<string> ConfigurationFolders { get; }

    string LogsFolder { get; }
}
