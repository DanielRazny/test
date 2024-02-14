using Microsoft.Extensions.Options;
using System;
using UpdateService.Options;
using UpdateService.Services.Cache;
using UpdateService.Services.Database;
using UpdateService.Services.FileSystem;

namespace UpdateService.Services
{
    public class ApplicationVersionHandler : IApplicationVersionHandler
    {
        private readonly IOptionsMonitor<SwitchOptions> _options;
        private readonly IApplicationVersionCache _cache;
        private readonly IFileSystemHandler _fileSystemHandler;
        private readonly IDatabaseHandler _databaseHandler;

        public ApplicationVersionHandler(
            IOptionsMonitor<SwitchOptions> options,
            IApplicationVersionCache applicationVersionCache,
            IFileSystemHandler fileSystemHandler,
            IDatabaseHandler databaseHandler) 
        {
            _options = options;
            _cache = applicationVersionCache;
            _fileSystemHandler = fileSystemHandler;
            _databaseHandler = databaseHandler;
        }

        public Task<string?> GetCurrentVersion(string applicationName)
            => _cache.GetCurrentVersion(applicationName);

        private IBaseApplicationVersionHandler GetVersionHandler()
        {
            var option = _options.CurrentValue;

            if (string.Equals(option?.Type, "database", StringComparison.OrdinalIgnoreCase))
            {
                return _databaseHandler;
            }

            return _fileSystemHandler;
        }

        // since this method lacks await => exceptions might not be catchable here
        public Task<Stream?> DownloadVersion(string applicationName, string version)
        {
            return GetVersionHandler().DownloadVersion(applicationName, version);
        }

        public async Task CheckApplicationVersions()
        {
            var handler = GetVersionHandler();
            var applicationNames = await handler
                .GetApplicationNames()
                .ConfigureAwait(false);

            foreach (var application in applicationNames)
            {
                var version = await handler
                    .GetCurrentVersion(application)
                    .ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(version))
                {
                    continue;
                }

                _cache.SetApplicationVersion(application, version);
            }
        }

        public Task<IReadOnlyCollection<string>> GetApplicationNames()
        {
            // todo cache or remove
            return GetVersionHandler().GetApplicationNames();
        }
    }
}
