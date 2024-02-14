
using Microsoft.Extensions.Options;
using UpdateService.Options;
using UpdateService.Services.VersionChecker;

namespace UpdateService.Services.FileSystem
{
    public class FileSystemHandler : IFileSystemHandler
    {
        private readonly IOptionsMonitor<FileSystemOptions> _options;
        private readonly IVersionCompareHandler _versionCompareHandler;

        public FileSystemHandler(IOptionsMonitor<FileSystemOptions> options, IVersionCompareHandler versionCompareHandler)
        {
            _options = options;
            _versionCompareHandler = versionCompareHandler;
        }

        public Task<Stream?> DownloadVersion(string applicationName, string version)
        {
            var options = _options.CurrentValue;

            if (string.IsNullOrEmpty(options.Path))
            {
                return Task.FromResult<Stream?>(null);
            }

            var filepath = Path.Combine(options.Path, applicationName, version, $"{applicationName}.exe");

            if (string.IsNullOrEmpty(filepath) || !File.Exists(filepath))
            {
                return Task.FromResult<Stream?>(null);

            }

            return Task.FromResult<Stream?>(new FileStream(filepath, FileMode.Open, FileAccess.Read));
        }

        public Task<string?> GetCurrentVersion(string applicationName)
        {
            var options = _options.CurrentValue;

            var basePath = Path.Combine(options.Path, applicationName);
            string? currentVersion = null;

            foreach(var directory in Directory.GetDirectories(basePath))
            {
                // if someone messes with the file system during a run
                if (!Directory.Exists(directory))
                {
                    continue;
                }

                var filePath = Path.Combine(directory, $"{applicationName}.exe");

                if (!File.Exists(filePath))
                {
                    continue;
                }
                var version = Path.GetFileName(directory);

                if (_versionCompareHandler.CompareVersions(version, currentVersion) > 0)
                {
                    currentVersion = version;
                }
            }

            return Task.FromResult(currentVersion);
        }

        public Task<IReadOnlyCollection<string>> GetApplicationNames()
        {
            var options = _options.CurrentValue;

            var basePath = Path.Combine(options.Path);
            var result = new List<string>();

            foreach (var directory in Directory.GetDirectories(basePath))
            {
                result.Add(Path.GetFileName(directory));
            }

            return Task.FromResult<IReadOnlyCollection<string>>(result.AsReadOnly());
        }
    }
}
